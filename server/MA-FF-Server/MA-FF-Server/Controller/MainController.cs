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


namespace WebAnalyzer.Controller
{
    public class MainController
    {
        private ExperimentModel currentExperiment;

        private HTMLUI mainUI;
        private ExperimentWizard experimentWizard;

        public void Start()
        {
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

            currentExperiment = ExperimentModel.CreateExperiment(e.Name);

            if (e.ImportData)
            {
                //import data
                if (e.ImportParticipants)
                    currentExperiment.Participants = LoadController.LoadParticipants(e.ImportExperimentPath);

                if (e.ImportSettings)
                    currentExperiment.Settings = LoadController.LoadSettings(e.ImportExperimentPath);

            }

            ExportController.SaveExperiment(currentExperiment);
            SetExpiermentData(currentExperiment);
            CloseExperimentWizard();
        }

        private void On_LoadExperiment(object source, LoadExperimentEvent e)
        {
            Logger.Log("On load experiment event");

            currentExperiment = LoadController.LoadExperiment(e.Path);

            SetExpiermentData(currentExperiment);
            CloseExperimentWizard();
        }

        private void SetExpiermentData(ExperimentModel experiment)
        {
            mainUI.SetExperimentData(experiment.ExperimentName, experiment.GetParticipantArray(), experiment.GetDomainSettingArray());
            mainUI.ReloadPage();
        }

        private void RefreshMainUI()
        {
            SetExpiermentData(currentExperiment);
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
                    ExperimentParticipant par = currentExperiment.GetParticipantByUID(e.UID);
                    ShowEditParticipantForm(par, false);
                    break;
                case EditParticipantEvent.EDIT_TYPES.Create:
                    ExperimentParticipant newPar = new ExperimentParticipant();
                    ShowEditParticipantForm(newPar, true);
                    break;
                case EditParticipantEvent.EDIT_TYPES.Copy:
                    ExperimentParticipant orig = currentExperiment.GetParticipantByUID(e.UID);
                    ExperimentParticipant copy = ExperimentParticipant.Copy(orig);
                    currentExperiment.Participants.Add(copy);
                    ExportController.SaveExperimentParticipants(currentExperiment);
                    RefreshMainUI();
                    break;
                case EditParticipantEvent.EDIT_TYPES.Delete:
                    ExperimentParticipant toDeletePar = currentExperiment.GetParticipantByUID(e.UID);
                    currentExperiment.Participants.Remove(toDeletePar);
                    ExportController.SaveExperimentParticipants(currentExperiment);
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
                    DomainSettings setting = currentExperiment.GetDomainSettingByUid(e.UID);
                    ShowEditDomainSettingForm(setting, false);
                    break;
                case EditDomainSettingEvent.EDIT_TYPES.Create:
                    DomainSettings newSetting = new DomainSettings();
                    ShowEditDomainSettingForm(newSetting, true);
                    break;
                case EditDomainSettingEvent.EDIT_TYPES.Copy:
                    DomainSettings origSetting = currentExperiment.GetDomainSettingByUid(e.UID);
                    DomainSettings copy = DomainSettings.Copy(origSetting);
                    currentExperiment.Settings.Domains.Add(copy);
                    ExportController.SaveExperimentSettings(currentExperiment);
                    RefreshMainUI();
                    break;
                case EditDomainSettingEvent.EDIT_TYPES.Delete:
                    DomainSettings toDeleteSetting = currentExperiment.GetDomainSettingByUid(e.UID);
                    currentExperiment.Settings.Domains.Remove(toDeleteSetting);
                    ExportController.SaveExperimentSettings(currentExperiment);
                    RefreshMainUI();
                    break;
            }
        }

        private void On_CreateParticipant(object source, CreateParticipantEvent e)
        {
            Logger.Log("create participant?");
            currentExperiment.Participants.Add(e.Participant);

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
                    ExportController.SaveExperimentParticipants(currentExperiment);
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
                    ExportController.SaveExperimentSettings(currentExperiment);
                    //refresh participants
                    RefreshMainUI();
                }
            });
        }

        
        private void On_CreateDomainSetting(object source, CreateDomainSettingEvent e)
        {
            Logger.Log("create domain setting?");
            currentExperiment.Settings.Domains.Add(e.Domain);

        }


        private void On_TriggerSave(object source, TriggerSaveEvent e)
        {
            switch (e.Type)
            {
                case TriggerSaveEvent.SAVE_TYPES.ALL:
                    ExportController.SaveExperiment(currentExperiment);
                    break;
                case TriggerSaveEvent.SAVE_TYPES.PARTICIPANTS:
                    ExportController.SaveExperimentParticipants(currentExperiment);
                    break;
                case TriggerSaveEvent.SAVE_TYPES.SETTINGS:
                    ExportController.SaveExperimentSettings(currentExperiment);
                    break;
            }
        }
    }
}
