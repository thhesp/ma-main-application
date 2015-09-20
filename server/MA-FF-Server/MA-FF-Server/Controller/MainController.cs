using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebAnalyzer.UI;
using WebAnalyzer.ApplicationSettings;
using WebAnalyzer.Util;
using WebAnalyzer.Events;


using WebAnalyzer.Models.SettingsModel;
using WebAnalyzer.Models.Base;
using WebAnalyzer.UI.InteractionObjects;

using WebAnalyzer.Test.Communication;
using WebAnalyzer.EyeTracking;
using WebAnalyzer.Server;

namespace WebAnalyzer.Controller
{
    public class MainController
    {
        private ExperimentModel _currentExperiment;

        private TestController _testController;

        private HTMLUI _mainUI;
        private ExperimentWizard _experimentWizard;
        private ExperimentParticipant _currentParticipant;


        private TestrunControl _testrunControlUI;

        public void Start()
        {
            ShowMainUI();
        }

        private void ShowMainUI()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _mainUI = new HTMLUI();
            _mainUI.Shown += new System.EventHandler(this.MainUIFinishedLoading);

            _mainUI.EditParticipant += On_EditParticpant;
            _mainUI.EditDomainSetting += On_EditDomainSetting;
            _mainUI.Testrun += On_TestrunEvent;
            _mainUI.EditApplicationSetting += On_EditApplicationSettings;
            _mainUI.TriggerSave += On_TriggerSave;

            Application.Run(_mainUI);
        }

        private void MainUIFinishedLoading(object sender, EventArgs e)
        {
            InitializeTestController();
            ShowExperimentWizard();
        }

        private void InitializeTestController()
        {
            _testController = new TestController();
            _testController.SaveTestrun += On_SaveTestrun;
            _testController.UpdateWSConnectionCount += On_UpdateConnectionCount;
        }

       
        private void ShowExperimentWizard()
        {
            Logger.Log("Show experiment wizard?");
            _experimentWizard = new ExperimentWizard();
            _experimentWizard.CreateExperiment += On_CreateExperiment;
            _experimentWizard.LoadExperiment += On_LoadExperiment;

            System.Windows.Forms.DialogResult result = _experimentWizard.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _experimentWizard.Dispose();
            }
            else if (result == System.Windows.Forms.DialogResult.Cancel || result == System.Windows.Forms.DialogResult.Abort)
            {
                _mainUI.Close();
            }
            else
            {
                Logger.Log("Experiment Wizard existed with result:  " + result.ToString());
            }
        }

        private void On_CreateExperiment(ExperimentWizardObj source, CreateExperimentEvent e)
        {
            Logger.Log("On create experiment event");

            _currentExperiment = ExperimentModel.CreateExperiment(e.Name);

            if (LoadController.ValidateIfExperimentDoesNotExist(_currentExperiment))
            {
                if (e.ImportData)
                {
                    //import data
                    if (e.ImportParticipants)
                        _currentExperiment.Participants = LoadController.LoadParticipants(e.ImportExperimentPath);

                    if (e.ImportSettings)
                        _currentExperiment.Settings = LoadController.LoadSettings(e.ImportExperimentPath);

                }

                ExportController.SaveExperiment(_currentExperiment);
                SetExpiermentData(_currentExperiment);
                CloseExperimentWizard();
                Logger.getInstance().ExperimentName = _currentExperiment.ExperimentName;
            }
            else
            {
                source.DisplayError("Es existiert bereits ein Experiment mit diesem Namen!");
                Logger.Log("Experiment does already exist!");
            }
        }

        private void On_LoadExperiment(ExperimentWizardObj source, LoadExperimentEvent e)
        {
            Logger.Log("On load experiment event");

            if (LoadController.ValidateExperimentFolder(e.Path))
            {
                ExperimentModel loadedExperiment = LoadController.LoadExperiment(e.Path);

                if (loadedExperiment != null)
                {
                    _currentExperiment = loadedExperiment;
                    SetExpiermentData(_currentExperiment);
                    CloseExperimentWizard();
                    Logger.getInstance().ExperimentName = _currentExperiment.ExperimentName;

                    Console.WriteLine("Setting: " + Properties.Settings.Default.WSMessageDelay);
                }
                else
                {
                    //do sth?
                    source.DisplayError("Das Experiment konnte nicht geladen werden.");
                    Logger.Log("Error loading the experiment!");
                }
            }
            else
            {
                //do sth?
                source.DisplayError("Der ausgewählte Ordner enthält keine Experimentdaten!");
                Logger.Log("Experiment does not exist!");
            }


        }

        private void SetConnectionStati()
        {
            _mainUI.SetWSConnectionStatus(_testController.WSStatus);
            _mainUI.SetTrackingConnectionStatus(_testController.TrackingStatus);
        }

        private void SetExpiermentData(ExperimentModel experiment)
        {
            _mainUI.SetExperimentData(experiment);
            SetConnectionStati();
            _mainUI.ReloadPage();
        }

        private void RefreshMainUI()
        {
            SetExpiermentData(_currentExperiment);
        }

        private void CloseExperimentWizard()
        {
            _experimentWizard.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _experimentWizard.DialogResult = DialogResult.OK;
            });
        }

        private void On_EditParticpant(object source, EditParticipantEvent e)
        {
            Logger.Log("edit participant?");
            switch (e.Type)
            {
                case EditParticipantEvent.EDIT_TYPES.Edit:
                    ExperimentParticipant par = _currentExperiment.GetParticipantByUID(e.UID);
                    ShowEditParticipantForm(par, false);
                    break;
                case EditParticipantEvent.EDIT_TYPES.Create:
                    ExperimentParticipant newPar = new ExperimentParticipant();
                    ShowEditParticipantForm(newPar, true);
                    break;
                case EditParticipantEvent.EDIT_TYPES.Copy:
                    ExperimentParticipant orig = _currentExperiment.GetParticipantByUID(e.UID);
                    ExperimentParticipant copy = ExperimentParticipant.Copy(orig);
                    _currentExperiment.Participants.Add(copy);
                    ExportController.SaveExperimentParticipants(_currentExperiment);
                    RefreshMainUI();
                    break;
                case EditParticipantEvent.EDIT_TYPES.Delete:
                    ExperimentParticipant toDeletePar = _currentExperiment.GetParticipantByUID(e.UID);
                    _currentExperiment.Participants.Remove(toDeletePar);
                    ExportController.SaveExperimentParticipants(_currentExperiment);
                    RefreshMainUI();
                    break;
            }
        }

        private void On_EditDomainSetting(object source, EditDomainSettingEvent e)
        {
            Logger.Log("edit domain setting?");
            switch (e.Type)
            {
               case EditDomainSettingEvent.EDIT_TYPES.Edit:
                    DomainSettings setting = _currentExperiment.GetDomainSettingByUid(e.UID);
                    ShowEditDomainSettingForm(setting, false);
                    break;
                case EditDomainSettingEvent.EDIT_TYPES.Create:
                    DomainSettings newSetting = new DomainSettings();
                    ShowEditDomainSettingForm(newSetting, true);
                    break;
                case EditDomainSettingEvent.EDIT_TYPES.Copy:
                    DomainSettings origSetting = _currentExperiment.GetDomainSettingByUid(e.UID);
                    DomainSettings copy = DomainSettings.Copy(origSetting);
                    _currentExperiment.Settings.Domains.Add(copy);
                    ExportController.SaveExperimentSettings(_currentExperiment);
                    RefreshMainUI();
                    break;
                case EditDomainSettingEvent.EDIT_TYPES.Delete:
                    DomainSettings toDeleteSetting = _currentExperiment.GetDomainSettingByUid(e.UID);
                    _currentExperiment.Settings.Domains.Remove(toDeleteSetting);
                    ExportController.SaveExperimentSettings(_currentExperiment);
                    RefreshMainUI();
                    break;
            }
        }

        private void On_CreateParticipant(object source, CreateParticipantEvent e)
        {
            Logger.Log("create participant?");
            _currentExperiment.Participants.Add(e.Participant);

        }

        private void ShowEditParticipantForm(ExperimentParticipant particpant, Boolean createNew)
        {
            _mainUI.BeginInvoke((Action)delegate
            {
                Logger.Log("Show edit participant?");
                EditParticipantForm editParticpant = new EditParticipantForm(particpant, createNew);

                editParticpant.CreateParticipant += On_CreateParticipant;

                if (editParticpant.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ExportController.SaveExperimentParticipants(_currentExperiment);
                    Logger.Log("Save edit participant");
                    //refresh participants
                    RefreshMainUI();
                }
            });
        }

        private void ShowEditDomainSettingForm(DomainSettings setting, Boolean createNew)
        {
            _mainUI.BeginInvoke((Action)delegate
            {
                Logger.Log("Show edit domain setting?");
                EditDomainSettingForm editSetting = new EditDomainSettingForm(setting, createNew);
                editSetting.CreateDomainSetting += On_CreateDomainSetting;
                editSetting.TriggerSave += On_TriggerSave;

                if (editSetting.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Logger.Log("Save edit domain setting");
                    ExportController.SaveExperimentSettings(_currentExperiment);
                    //refresh participants
                    RefreshMainUI();
                }
            });
        }

        
        private void On_CreateDomainSetting(object source, CreateDomainSettingEvent e)
        {
            Logger.Log("create domain setting?");
            _currentExperiment.Settings.Domains.Add(e.Domain);

        }


        private void On_TriggerSave(object source, TriggerSaveEvent e)
        {
            switch (e.Type)
            {
                case TriggerSaveEvent.SAVE_TYPES.ALL:
                    ExportController.SaveExperiment(_currentExperiment);
                    break;
                case TriggerSaveEvent.SAVE_TYPES.PARTICIPANTS:
                    ExportController.SaveExperimentParticipants(_currentExperiment);
                    break;
                case TriggerSaveEvent.SAVE_TYPES.SETTINGS:
                    ExportController.SaveExperimentSettings(_currentExperiment);
                    break;
            }
        }

        private void On_TestrunEvent(BaseInteractionObject source, TestrunEvent e)
        {
            if(source is TestrunControl){
                _testrunControlUI = (TestrunControl) source;
            }
                

            switch (e.Type)
            {
                case TestrunEvent.EVENT_TYPE.Create:
                    CreateTestrun();
                    break;
                case TestrunEvent.EVENT_TYPE.Start:
                    StartTest();
                    break;
                case TestrunEvent.EVENT_TYPE.Stop:
                    StopTest();
                    break;
            }
        }

        private void CreateTestrun()
        {
            _mainUI.BeginInvoke((Action)delegate
            {
                Logger.Log("Show edit application setting");
                TestrunForm testrun = new TestrunForm(_currentExperiment);
                testrun.Testrun += On_TestrunEvent;
                testrun.SelectParticipant += On_SelectParticipantForTest;

                if (testrun.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                   
                }
            });
        }

        private void StartTest()
        {

            _testController.StartTest();
        }

        private void StopTest()
        {
            Logger.Log("Stop Test");
            _testController.StopTest();
            _testrunControlUI.ShowSaveIndicator();
        }

        private void On_EditApplicationSettings(object source, EditApplicationSettingsEvent e)
        {
            _mainUI.BeginInvoke((Action)delegate
            {
                Logger.Log("Show edit application setting");
                EditApplicationSettings editSetting = new EditApplicationSettings();

                if (editSetting.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Logger.Log("Saving application settings");
                    _testController.RefreshServices();
                    RefreshMainUI();
                    //refresh other stuff?
                }
            });
        }

        private void On_SelectParticipantForTest(object source, SelectParticipantForTestEvent e)
        {
            Logger.Log("Select Participant....");
            _currentParticipant = _currentExperiment.GetParticipantByUID(e.UID);
        }

        private void On_SaveTestrun(object source, SaveTestrunEvent e)
        {
            if (!_testController.DataCollected)
            {
                Logger.Log("No Data collected!");
                if (_testrunControlUI != null)
                {
                    _testrunControlUI.DisplayError("Es wurden keine Daten erfasst und somit konnten keine Daten gespeichert werden.");
                }
            }else if (_currentExperiment != null
                    && _currentParticipant != null)
            {
                Logger.Log("Saving data...");
                ExportController.SaveExperimentTestRun(_currentExperiment, _currentParticipant, _testController.Test);
                ExportController.SaveExperimentRawData(_currentExperiment, _currentParticipant, _testController.Test, _testController.RawData);
            }

            if (_testrunControlUI != null)
            {
                _testrunControlUI.HideSaveIndicator();
            }
        }

        /// <summary>
        /// Callback for Update WS Connection Count Event
        /// </summary>
        /// <param name="sender">Object from which the event gets triggered.</param>
        /// <param name="e">Data about the number of connections</param>
        private void On_UpdateConnectionCount(object sender, UpdateWSConnectionCountEvent e)
        {
            if (_mainUI != null)
            {
                _mainUI.SetWSConnectionCount(e.Count);
                _mainUI.RefreshData();
            }
        }
    }
}
