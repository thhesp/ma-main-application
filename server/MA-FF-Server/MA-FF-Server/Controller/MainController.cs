﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebAnalyzer.UI;
using WebAnalyzer.ApplicationSettings;
using WebAnalyzer.Util;
using WebAnalyzer.Events;

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
            mainUI.SetExperimentData(experiment.ExperimentName);
            mainUI.ReloadPage();
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
            if (e.CreateNew)
            {
                // create new participant
                ExperimentParticipant par = new ExperimentParticipant();
                ShowEditParticipantForm(par, true);

            }
            else
            {
                //load participant
                ExperimentParticipant par = new ExperimentParticipant();
                ShowEditParticipantForm(par, false);
            }
        }

        private void On_CreateParticipant(object source, CreateParticipantEvent e)
        {
            Logger.Log("create participant?");
            currentExperiment.Participants.Add(e.Participant);

        }

        private void ShowEditParticipantForm(ExperimentParticipant particpant, Boolean createNew)
        {
            Logger.Log("Show edit participant?");
            EditParticipantForm editParticpant = new EditParticipantForm(particpant, createNew);

            editParticpant.CreateParticipant += On_CreateParticipant;

            if (editParticpant.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                
                ExportController.SaveExperimentParticipants(currentExperiment);
                Logger.Log("Save edit participant");
            }
        }
    }
}
