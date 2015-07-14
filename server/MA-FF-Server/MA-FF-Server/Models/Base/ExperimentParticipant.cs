using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;

using System.Xml;

namespace WebAnalyzer.Models.Base
{
    public class ExperimentParticipant
    {

        public enum SEX_TYPES { Male = 0, Female = 1, Undecided = 2 };

        private String _identifier;

        /* demographic data */
        private SEX_TYPES _sex;
        private int _birthYear;
        private String _education;

        /* extra data */
        private Dictionary<String, String> _variableData = new Dictionary<String, String>();

        /* experiment data */
        private List<WebpageModel> _visitedPages = new List<WebpageModel>();

        /* other data */
        private DateTime _createdAt;

        public ExperimentParticipant()
        {
            _createdAt = DateTime.Now;
        }

        public String Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public SEX_TYPES Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }

        public int BirthYear
        {
            get { return _birthYear; }
            set { _birthYear = value; }
        }

        public String Education
        {
            get { return _education; }
            set { _education = value; }
        }

        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }

        public Dictionary<String, String> ExtraData
        {
            get { return _variableData; }
            set { _variableData = value; }
        }

        public void AddExtraData(String key, String value)
        {
            if (!_variableData.ContainsKey(key))
            {
                _variableData.Add(key, value);
            }
            else
            {
                _variableData[key] = value;
            }
                
        }

        public Dictionary<String, String> GetExtraData()
        {
            return _variableData;
        }

        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode participant = xmlDoc.CreateElement("participant");

            /* Identifier */

            XmlAttribute identifier = xmlDoc.CreateAttribute("identifier");

            identifier.Value = this.Identifier;

            participant.Attributes.Append(identifier);

            /* Created At */

            XmlAttribute createdAt = xmlDoc.CreateAttribute("created-at");

            createdAt.Value = this.CreatedAt.ToString();

            participant.Attributes.Append(createdAt);

            /* Sex */

            XmlAttribute sex = xmlDoc.CreateAttribute("sex");

            sex.Value = this.Sex.ToString();

            participant.Attributes.Append(sex);

            /* birthyear */

            XmlAttribute birthyear = xmlDoc.CreateAttribute("birthyear");

            birthyear.Value = this.BirthYear.ToString();

            participant.Attributes.Append(birthyear);

            /* Education */

            XmlAttribute education = xmlDoc.CreateAttribute("education");

            education.Value = this.Education;

            participant.Attributes.Append(education);

            /* Variable Data */

            XmlNode extraAttributes = xmlDoc.CreateElement("extra-attributes");

            foreach (String key in _variableData.Keys)
            {
                XmlNode attribute = xmlDoc.CreateElement(key);

                attribute.InnerText = _variableData[key];

                extraAttributes.AppendChild(attribute);
            }

            participant.AppendChild(extraAttributes);

            return participant;
        }

        public static ExperimentParticipant LoadFromXML(XmlNode particpantNode)
        {
            ExperimentParticipant participant = new ExperimentParticipant();

            foreach (XmlAttribute attr in particpantNode.Attributes)
            {
                switch (attr.Name)
                {
                    case "identifier":
                        participant.Identifier = attr.Value;
                        break;
                    case "created-at":
                        participant.CreatedAt = DateTime.Parse(attr.Value);
                        break;
                    case "sex":
                        //participant.Sex = attr.Value;
                        break;
                    case "birthyear":
                        participant.BirthYear = int.Parse(attr.Value);
                        break;
                    case "education":
                        participant.Education = attr.Value;
                        break;
                }
            }

            foreach (XmlNode attributes in particpantNode.ChildNodes)
            {
                foreach (XmlNode attribute in attributes.ChildNodes)
                {
                    participant.AddExtraData(attribute.Name, attribute.InnerText);
                }
            }

            return participant;
        }


        public static ExperimentParticipant Copy(ExperimentParticipant orig)
        {
            ExperimentParticipant copy = new ExperimentParticipant();

            copy.Identifier = "Kopie - " + orig.Identifier;
            copy.BirthYear = orig.BirthYear;
            copy.Education = orig.Education;
            copy.Sex = orig.Sex;

            copy.ExtraData = orig.ExtraData;

            return copy;
        }
    }
}
