using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Models.SettingsModel;

namespace WebAnalyzer.Models.Base
{
    public class ExperimentModel
    {

        private String _experimentName;
        private DateTime _createdAt;

        private List<ExperimentParticipant> _participants = new List<ExperimentParticipant>();

        private ExperimentSettings _settings;

        public ExperimentModel(String experimentName)
        {
            _experimentName = experimentName;
        }

        public ExperimentModel()
        {

        }

        public String ExperimentName
        {
            get { return _experimentName; }
            set { _experimentName = value;  }
        }

        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }

        public List<ExperimentParticipant> Particpants
        {
            get { return _participants; }
            set { _participants = value; }
        }

        public ExperimentSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        public String GetBaseExperimentLocation()
        {
            return Properties.Settings.Default.Datalocation + _experimentName + "\\";
        }

        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode experiment = xmlDoc.CreateElement("experiment");

            /* name */

            XmlAttribute name = xmlDoc.CreateAttribute("name");

            name.Value = this.ExperimentName;

            experiment.Attributes.Append(name);

            /* Created At */

            XmlAttribute createdAt = xmlDoc.CreateAttribute("created-at");

            createdAt.Value = this.CreatedAt.ToString();

            experiment.Attributes.Append(createdAt);



            return experiment;
        }

        public static ExperimentModel LoadFromXML(XmlDocument doc)
        {
            ExperimentModel experiment = new ExperimentModel();

            XmlNode expNode = doc.DocumentElement.SelectSingleNode("experiment");

            foreach(XmlAttribute attr in expNode.Attributes){
                switch (attr.Name)
                {
                    case "name":
                        experiment.ExperimentName = attr.Value;
                        break;
                    case "created-at":
                        experiment.CreatedAt = DateTime.Parse(attr.Value);
                        break;
                }
            }

            return experiment;
        }
    }
}
