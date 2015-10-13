using System;
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
    /// <summary>
    /// Interaction object for analysing and exporting testdata
    /// </summary>
    public class AnalysisExportControl : BaseInteractionObject
    {

        /// <summary>
        /// Reference to the main ui of the application
        /// </summary>
        private HTMLUI _form;

        /// <summary>
        /// Reference to the currently loaded experiment
        /// </summary>
        private ExperimentModel _exp;

        /// <summary>
        /// Reference to the selected participant
        /// </summary>
        private ExperimentParticipant _participant;

        /// <summary>
        /// Folderpath to save the data to
        /// </summary>
        private String _folderPath;

        /// <summary>
        /// Reference to the selected testrun
        /// </summary>
        private TestModel _testrun;

        /// <summary>
        /// Filename to use as a base when saving
        /// </summary>
        private String _filename;

        /// <summary>
        /// Selected Exportformat
        /// </summary>
        private ExportController.EXPORT_FORMATS _exportFormat;

        /// <summary>
        /// Selected algorithm type
        /// </summary>
        /// <remarks>Only used when analysing data</remarks>
        private Algorithm.ALGORITHM_TYPES _algorithmType;

        /// <summary>
        /// Shall data about each gaze be added
        /// </summary>
        /// <remarks>ONly used when analysing data</remarks>
        private Boolean _containGazeData = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">Reference to the main UI</param>
        public AnalysisExportControl(HTMLUI form)
        {
            _form = form;
        }

        /// <summary>
        /// Getter / Setter for the currently loaded experiment
        /// </summary>
        public ExperimentModel Experiment
        {
            get { return _exp; }
            set { _exp = value; }
        }

        /// <summary>
        /// Method to set the filename
        /// </summary>
        /// <param name="filename">The new filename</param>
        /// <remarks>Called from Javascript</remarks>
        public void setFilename(String filename)
        {
            _filename = filename;
        }

        /// <summary>
        /// Sets the export format
        /// </summary>
        /// <param name="format">The new format</param>
        /// <remarks>Called from javascript</remarks>
        public void setExportFormat(String format)
        {
            _exportFormat = (ExportController.EXPORT_FORMATS)Enum.Parse(typeof(ExportController.EXPORT_FORMATS), format, true);

            if (_exportFormat == ExportController.EXPORT_FORMATS.CSV)
            {
                DisplayError("Das Fomat CSV wird derzeit leider noch nicht unterstützt.");
            }
        }

        /// <summary>
        /// Used for updating the algorithm type
        /// </summary>
        /// <param name="algorithmType">new algorithm type</param>
        /// <remarks>Called from javascript</remarks>
        public void setAlgorithmType(String algorithmType)
        {
            switch (algorithmType)
            {
                case "distance":
                    _algorithmType = Algorithm.ALGORITHM_TYPES.DISTANCE;
                    break;
                case "iview":
                    _algorithmType = Algorithm.ALGORITHM_TYPES.IVIEW_EVENTS;
                    break;
            }
        }

        /// <summary>
        /// Sets if gaze data shall be included for each gaze
        /// </summary>
        /// <param name="containGazeData"></param>
        /// <remarks>Called from javascript</remarks>
        public void setContainGazeData(Boolean containGazeData)
        {
            _containGazeData = containGazeData;
        }

        /// <summary>
        /// Used for starting the export
        /// </summary>
        /// <remarks>Called from javascript</remarks>
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

        /// <summary>
        /// Used for starting the analysis
        /// </summary>
        /// <remarks>Called from javascript</remarks>
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

        /// <summary>
        /// Extracts the data which is needed for the algorithm from the HTML.
        /// </summary>
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

        /// <summary>
        /// Used for selecting a participant
        /// </summary>
        /// <remarks>Called from javascript</remarks>
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

        /// <summary>
        /// Used for selecting a testrun
        /// </summary>
        /// <remarks>Called from javascript</remarks>
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

        /// <summary>
        /// Used for selecting a folder
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void selectFolder()
        {
            _folderPath = @SelectFolderDialog();


            this.EvaluteJavaScript("setFolderPath('" + _folderPath + "');");
        }

        /// <summary>
        /// Creates a select folder dialog.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Callback when a participant was selected
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void On_SelectParticipant(object source, SelectParticipantForTestEvent e)
        {
            Logger.Log("Selected Participant " + e.UID);

            _participant = _exp.GetParticipantByUID(e.UID);

            this.EvaluteJavaScript("setParticipant('" + _participant.Identifier + "');");
        }

        /// <summary>
        /// Callback when a testrun was selected
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Shows the saving indicator
        /// </summary>
        public void ShowSaveIndicator()
        {
            Logger.Log("show save indicator");
            EvaluteJavaScript("showSaveIndicator();");

        }

        /// <summary>
        /// Hides the saving indicator
        /// </summary>
        public void HideSaveIndicator()
        {
            Logger.Log("hide save indicator");
            EvaluteJavaScript("hideSaveIndicator();");
        }

        /// <summary>
        /// Exports the analysed data
        /// </summary>
        /// <param name="algorithm">For the analysis used algorithm</param>
        private void ExportData(Algorithm algorithm)
        {
            Boolean errorWhileExporting = false;

            try
            {
                ExportController.SaveExperimentFixations(_testrun, _participant, algorithm, _folderPath, _filename, _exportFormat, _containGazeData);
                ExportController.SaveExperimentAOI(_exp, _testrun, _participant, algorithm, _folderPath, _filename, _exportFormat, _containGazeData);
                ExportController.SaveExperimentSaccades(_testrun, _participant, algorithm, _folderPath, _filename, _exportFormat, _containGazeData);
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

        /// <summary>
        /// Method for the distance algorithm
        /// </summary>
        /// <param name="minimumDuration">The minimumduration</param>
        /// <param name="acceptableDeviation">The acceptable deviation</param>
        /// <remarks>Called from javascript</remarks>
        public void useDistanceAlgorithm(double minimumDuration, double acceptableDeviation)
        {
            Logger.Log("Use minimumDuration: " + minimumDuration);
            Logger.Log("Use acceptableDeviation: " + acceptableDeviation);

            DistanceAlgorithm algorithm = new DistanceAlgorithm();

            algorithm.MinimumDuration = minimumDuration;
            algorithm.AccetableDeviations = acceptableDeviation;

            try
            {
                _testrun.ExtractFixationsAndSaccades(algorithm);

                ExportData(algorithm);
            }
            catch (Exception e)
            {
                Logger.Log("Exception while using the algorithm.");
                Logger.Log("Abort analysis");

                HideSaveIndicator();
                DisplayError("Bei der Analyse der Daten ist ein Problem aufgetreten.");
            }
        }

        /// <summary>
        /// Use the iview events
        /// </summary>
        private void useIViewEvents()
        {
            Logger.Log("Loading RawData.. ");

            String rawDataPath = LoadController.GetAssociatedRawDataForTestdata(_exp, _participant, _testrun.Filename);

            RawTrackingData rawData = LoadController.LoadRawData(rawDataPath);

            if (rawData != null)
            {
                IViewEventsAlgorithm algorithm = new IViewEventsAlgorithm();

                algorithm.RawData = rawData;

                try
                {
                    _testrun.ExtractFixationsAndSaccades(algorithm);

                    ExportData(algorithm);
                }
                catch (Exception e)
                {
                    Logger.Log("Exception while using the algorithm.");
                    Logger.Log("Abort analysis");

                    HideSaveIndicator();
                    DisplayError("Bei der Analyse der Daten ist ein Problem aufgetreten.");
                }
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
