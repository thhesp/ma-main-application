﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using WebAnalyzer.Util;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Models.DataModel;

using System.Threading;
using WebAnalyzer.Events;

using WebAnalyzer.Controller;

using WebAnalyzer.Models.AlgorithmModel;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class AnalysisExportControl : BaseInteractionObject
    {

        private HTMLUI _form;

        private ExperimentModel _exp;

        private ExperimentParticipant _participant;

        private String _folderPath;

        private TestModel _testrun;

        private String _filename;

        private ExportController.EXPORT_FORMATS _exportFormat;

        private Algorithm.ALGORITHM_TYPES _algorithmType;

        private Boolean _containGazeData = false;

        public AnalysisExportControl(HTMLUI form)
        {
            _form = form;
        }

        public ExperimentModel Experiment
        {
            get { return _exp; }
            set { _exp = value; }
        }

        public void setFilename(String filename)
        {
            _filename = filename;
        }

        public void setExportFormat(String format)
        {
            _exportFormat = (ExportController.EXPORT_FORMATS) Enum.Parse(typeof(ExportController.EXPORT_FORMATS), format, true);

            if (_exportFormat == ExportController.EXPORT_FORMATS.CSV)
            {
                DisplayError("Das Fomat CSV wird derzeit leider noch nicht unterstützt.");
            }
        }

        public void setAlgorithmType(String format)
        {
            switch (format)
            {
                case "distance":
                    _algorithmType = Algorithm.ALGORITHM_TYPES.DISTANCE;
                    break;
                case "iview":
                    _algorithmType = Algorithm.ALGORITHM_TYPES.IVIEW_EVENTS;
                    break;
            }
        }

        public void setContainGazeData(Boolean containGazeData)
        {
            _containGazeData = containGazeData;
        }

        public void export()
        {
            Boolean errorWhileExporting = false;

            ShowSaveIndicator();

            try
            {
                if (_testrun != null && _folderPath != "" && _filename != "")
                {
                    ExportController.ExportExperimentTestRun(_testrun, _participant, _folderPath, _filename, _exportFormat);

                    String rawDataPath = LoadController.GetAssociatedRawDataForTestdata(_exp, _participant, _testrun.Filename);

                    RawTrackingData rawData = LoadController.LoadRawData(rawDataPath);

                    if (rawData != null)
                    {
                        ExportController.ExportExperimentRawData(rawData, _participant, _folderPath, _filename, _exportFormat);
                    }
                    else
                    {
                        Logger.Log("Could not load the raw data for this test.");

                        DisplayError("Die Rawdaten zu diesem Test konnten nicht geladen werden.");

                        errorWhileExporting = true;
                    }


                }
                else
                {
                    if (_testrun == null)
                    {
                        Logger.Log("Testdaten konnten nicht geladen werden...");

                        DisplayError("Testdaten konnten nicht geladen werden...");

                    }

                    Logger.Log("Path: " + _folderPath);
                    Logger.Log("Filename: " + _filename);

                    errorWhileExporting = true;
                }
            }
            catch (Exception e)
            {
                Logger.Log("Exception while exporting: " + e.Message);

                errorWhileExporting = true;
            }

            HideSaveIndicator();

            if (errorWhileExporting)
            {
                Logger.Log("Die Daten konnten nicht erfolgreich exportiert werden.");

                DisplayError("Die Daten konnten nicht erfolgreich exportiert werden.");
            }
            else
            {
                Logger.Log("Die Daten konnten erfolgreich exportiert werden.");

                DisplaySuccess("Die Daten konnten erfolgreich exportiert werden.", "Export erfolgreich");
            }
        }

        public void analyse()
        {
            ShowSaveIndicator();

            if (_testrun != null && _folderPath != "" && _filename != "")
            {
                // after extracting the algorithm Data, the Export will be triggered from there
                extractAlgorithmData();
            }
            else
            {
                if (_testrun == null)
                {
                    Logger.Log("Testdaten konnten nicht geladen werden...");
                }

                Logger.Log("Path: " + _folderPath);
                Logger.Log("Filename: " + _filename);
            }
        }

        private void extractAlgorithmData()
        {
            switch (_algorithmType)
            {
                case Algorithm.ALGORITHM_TYPES.DISTANCE:
                    EvaluteJavaScript("createDistanceAlgorithm();");
                    break;
                case Algorithm.ALGORITHM_TYPES.IVIEW_EVENTS:
                    useIViewEvents();
                    break;
            }
        }

        public void selectParticipant()
        {
            _form.BeginInvoke((Action)delegate
            {
                SelectParticipantForm selectParticipant = new SelectParticipantForm(_exp);

                selectParticipant.SelectParticipant += On_SelectParticipant;

                if (selectParticipant.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //_form.ReloadPage();
                    
                }
            });
        }

        public void selectTestrun()
        {
            _form.BeginInvoke((Action)delegate
            {
                SelectTestrunForm selectTestrun = new SelectTestrunForm(_exp, _participant);

                selectTestrun.SelectTestrun += On_SelectTestrun;

                if (selectTestrun.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //_form.ReloadPage();

                }
            });
        }

        public void selectFolder()
        {
            _folderPath = @SelectFolderDialog();


            this.EvaluteJavaScript("setFolderPath('" + _folderPath + "');");
        }

        private String SelectFolderDialog()
        {
            // done in a new thread (kinda is a hack) because of the STAThread problem.
            //http://stackoverflow.com/questions/6860153/exception-when-using-folderbrowserdialog
            string selectedPath = "";
            var t = new Thread((ThreadStart)(() =>
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                fbd.ShowNewFolderButton = false;
                if (fbd.ShowDialog() == DialogResult.Cancel)
                {
                    selectedPath = null;
                    return;
                }


                selectedPath = fbd.SelectedPath;
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            return selectedPath;
        }

        private void On_SelectParticipant(object source, SelectParticipantForTestEvent e)
        {
            Logger.Log("Selected Participant " + e.UID);

            _participant = _exp.GetParticipantByUID(e.UID);

            this.EvaluteJavaScript("setParticipant('"+_participant.Identifier+"');");
        }

        private void On_SelectTestrun(object source, SelectTestrunToLoadEvent e)
        {
            Logger.Log("Selected Testrun " + e.Path);

            //_testrun load

            _testrun = LoadController.LoadTest(e.Path);

            if (_testrun == null)
            {
                DisplayError("Die ausgewählten Testdaten konnte nicht geladen werden.");
            }
            else
            {
                this.EvaluteJavaScript("setTestrun('" + e.Created + "');");
            }

            
        }

        public void ShowSaveIndicator()
        {
            Logger.Log("show save indicator");
            EvaluteJavaScript("showSaveIndicator();");

        }

        public void HideSaveIndicator()
        {
            Logger.Log("hide save indicator");
            EvaluteJavaScript("hideSaveIndicator();");
        }

        private void ExportData()
        {
            Boolean errorWhileExporting = false;

            try
            {
                ExportController.SaveExperimentFixations(_testrun, _participant, _folderPath, _filename, _exportFormat, _containGazeData);
                ExportController.SaveExperimentAOI(_exp, _testrun, _participant, _folderPath, _filename, _exportFormat, _containGazeData);
                ExportController.SaveExperimentSaccades(_testrun, _participant, _folderPath, _filename, _exportFormat, _containGazeData);
            }
            catch (Exception e)
            {
                errorWhileExporting = true;

                Logger.Log("Exception while exporting: " + e.Message);
            }

            HideSaveIndicator();

            if (errorWhileExporting)
            {
                Logger.Log("Die Daten konnten nicht erfolgreich exportiert werden.");

                DisplayError("Die Daten konnten nicht erfolgreich exportiert werden.");
            }
            else
            {
                Logger.Log("Die Daten konnten erfolgreich exportiert werden.");

                DisplaySuccess("Die Daten konnten erfolgreich exportiert werden.", "Export erfolgreich");
            }
        }

        public void useDistanceAlgorithm(double minimumDuration, double acceptableDeviation)
        {
            Logger.Log("Use minimumDuration: " + minimumDuration);
            Logger.Log("Use acceptableDeviation: " + acceptableDeviation);

            DistanceAlgorithm algorithm = new DistanceAlgorithm();

            algorithm.MinimumDuration = minimumDuration;
            algorithm.AccetableDeviations = acceptableDeviation;

            _testrun.ExtractFixationsAndSaccades(algorithm);

            ExportData();
        }

        private void useIViewEvents()
        {
            Logger.Log("Loading RawData.. ");

            String rawDataPath = LoadController.GetAssociatedRawDataForTestdata(_exp, _participant, _testrun.Filename);

            RawTrackingData rawData = LoadController.LoadRawData(rawDataPath);

            if (rawData != null)
            {
                IViewEventsAlgorithm algorithm = new IViewEventsAlgorithm();

                algorithm.RawData = rawData;

                _testrun.ExtractFixationsAndSaccades(algorithm);

                ExportData();
            }
            else
            {
                Logger.Log("Could not load the raw data for this test.");

                DisplayError("Die Rawdaten zu diesem Test konnten nicht geladen werden.");

                HideSaveIndicator();
            }

            
        }
    }
}
