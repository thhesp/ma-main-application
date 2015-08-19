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

using WebAnalyzer.Experiment;
using WebAnalyzer.Test.Communication;
using WebAnalyzer.EyeTracking;
using WebAnalyzer.Server;

namespace WebAnalyzer.Controller
{
    public class MainController
    {
        private ExperimentModel _currentExperiment;

        private TestController _testController;

        private HTMLUI mainUI;
        private ExperimentWizard experimentWizard;
        private ExperimentParticipant _currentParticipant;

        public void Start()
        {
            _testController = new TestController();

            ShowMainUI();
        }

        private void ShowMainUI()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            mainUI = new HTMLUI();
            mainUI.Shown += new System.EventHandler(this.MainUIFinishedLoading);

            mainUI.EditParticipant += On_EditParticpant;
            mainUI.EditDomainSetting += On_EditDomainSetting;
            mainUI.Testrun += On_TestrunEvent;
            mainUI.EditApplicationSetting += On_EditApplicationSettings;

            Application.Run(mainUI);
        }

        private void MainUIFinishedLoading(object sender, EventArgs e)
        {
            ShowExperimentWizard();
        }

       
        private void ShowExperimentWizard()
        {
            Logger.Log("Show experiment wizard?");
            experimentWizard = new ExperimentWizard();
            experimentWizard.CreateExperiment += On_CreateExperiment;
            experimentWizard.LoadExperiment += On_LoadExperiment;

            if (experimentWizard.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                experimentWizard.Dispose();
            }
        }

        private void On_CreateExperiment(object source, CreateExperimentEvent e)
        {
            Logger.Log("On create experiment event");

            _currentExperiment = ExperimentModel.CreateExperiment(e.Name);

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
        }

        private void On_LoadExperiment(object source, LoadExperimentEvent e)
        {
            Logger.Log("On load experiment event");

            _currentExperiment = LoadController.LoadExperiment(e.Path);

            SetExpiermentData(_currentExperiment);
            SetConnectionStati();
            CloseExperimentWizard();
        }

        private void SetConnectionStati()
        {
            mainUI.SetWSConnectionStatus(_testController.WSStatus);
            mainUI.SetTrackingConnectionStatus(_testController.TrackingStatus);
            mainUI.ReloadPage();
        }

        private void SetExpiermentData(ExperimentModel experiment)
        {
            mainUI.SetExperimentData(experiment);
            mainUI.ReloadPage();
        }

        private void RefreshMainUI()
        {
            SetExpiermentData(_currentExperiment);
        }

        private void CloseExperimentWizard()
        {
            experimentWizard.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                experimentWizard.DialogResult = DialogResult.OK;
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
            mainUI.BeginInvoke((Action)delegate
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
            mainUI.BeginInvoke((Action)delegate
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

        private void On_TestrunEvent(object source, TestrunEvent e)
        {
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
            mainUI.BeginInvoke((Action)delegate
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
            if (_testController.DataCollected
                && _currentExperiment != null 
                && _currentParticipant != null)
            {
                ExportController.SaveExperimentTestRun(_currentExperiment, _currentParticipant, _testController.Test);
            }
        }

        private void On_EditApplicationSettings(object source, EditApplicationSettingsEvent e)
        {
            mainUI.BeginInvoke((Action)delegate
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

    }
}
