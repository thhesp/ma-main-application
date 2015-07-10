﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using WebAnalyzer.Models.SettingsModel;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Util;

namespace WebAnalyzer.Controller
{
    public static class LoadController
    {


        public static ExperimentModel LoadExperiment(String path)
        {
            if(System.IO.Directory.Exists(path)){
                XmlDocument doc = new XmlDocument();
                doc.Load(path + "\\" + Properties.Settings.Default.ExperimentFilename);

                ExperimentModel experiment = ExperimentModel.LoadFromXML(doc);

                if (experiment != null)
                {
                    experiment.Particpants = LoadParticipants(path);
                    experiment.Settings = LoadSettings(path);
                }

                return experiment;
            }
            // throw exception?
            return null;
        }

        public static List<ExperimentParticipant> LoadParticipants(String path)
        {
            if (System.IO.Directory.Exists(path))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path + "\\" + Properties.Settings.Default.ParticipantsFilename);

                XmlNode participantsNode = doc.DocumentElement.SelectSingleNode("/participants");

                if (participantsNode == null)
                {
                    return null;
                }

                List<ExperimentParticipant> participants = new List<ExperimentParticipant>();

                foreach (XmlNode participantNode in participantsNode.ChildNodes)
                {
                    ExperimentParticipant participant = ExperimentParticipant.LoadFromXML(participantNode);
                    if (participant != null)
                    {
                        participants.Add(participant);
                    }
                }

                return participants;
            }
            // throw exception?
            return null;

        }

        public static ExperimentSettings LoadSettings(String path)
        {
            if (System.IO.Directory.Exists(path))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path + "\\" + Properties.Settings.Default.SettingsFilename);

                return ExperimentSettings.LoadFromXML(doc);
            }
            // throw exception?
            return null;
        }
    }
}
