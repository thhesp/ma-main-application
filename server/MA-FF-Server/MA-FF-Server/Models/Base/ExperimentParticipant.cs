using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;

using System.Xml;

namespace WebAnalyzer.Models.Base
{
    /// <summary>
    /// Class for the experiment participant
    /// </summary>
    public class ExperimentParticipant : UIDBase
    {
        /// <summary>
        /// Enumeration of sex types which are usable
        /// </summary>
        public enum SEX_TYPES { Male = 0, Female = 1, Undecided = 2 };

        /// <summary>
        /// Identifier of the participant
        /// </summary>
        private String _identifier;

        /* demographic data */
        /// <summary>
        /// Sex of the participant
        /// </summary>
        private SEX_TYPES _sex;
        /// <summary>
        /// Birthyear of the participant
        /// </summary>
        private int _birthYear;
        /// <summary>
        /// Education of the participant
        /// </summary>
        private String _education;

        /* extra data */
        /// <summary>
        /// Extra data about the participant
        /// </summary>
        private Dictionary<String, String> _variableData = new Dictionary<String, String>();

        /* experiment data */
        /// <summary>
        /// Experiment Data of the participant
        /// </summary>
        /// <remarks>
        /// Not sure where or IF it is used...
        /// </remarks>
        private List<WebpageModel> _visitedPages = new List<WebpageModel>();

        /* other data */
        /// <summary>
        /// Date when the participant was created
        /// </summary>
        private DateTime _createdAt;


        /// <summary>
        /// Constructor which sets the created at time
        /// </summary>
        public ExperimentParticipant() : base()
        {
            _createdAt = DateTime.Now;
        }

        /// <summary>
        /// Getter/ Setter for participant identifier
        /// </summary>
        public String Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        /// <summary>
        /// Getter/ Setter for the sex of the participant
        /// </summary>
        public SEX_TYPES Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }

        /// <summary>
        /// Getter/ Setter for the birthyear of the participant
        /// </summary>
        public int BirthYear
        {
            get { return _birthYear; }
            set { _birthYear = value; }
        }

        /// <summary>
        /// Getter/ Setter for the education of the participant
        /// </summary>
        public String Education
        {
            get { return _education; }
            set { _education = value; }
        }

        /// <summary>
        /// Getter/ Setter for creation date of the participant
        /// </summary>
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }

        /// <summary>
        /// Getter/ Setter for the extra data about the participant
        /// </summary>
        public Dictionary<String, String> ExtraData
        {
            get { return _variableData; }
            set { _variableData = value; }
        }

        /// <summary>
        /// Method for adding extra data
        /// </summary>
        /// <param name="key">the description of the extra data</param>
        /// <param name="value">the value of the extra data</param>
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

        /// <summary>
        /// Returns the extra data about the participant
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, String> GetExtraData()
        {
            return _variableData;
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="participantNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
        public static ExperimentParticipant LoadFromXML(XmlNode participantNode)
        {
            ExperimentParticipant participant = new ExperimentParticipant();

            foreach (XmlAttribute attr in participantNode.Attributes)
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
                        participant.Sex = (SEX_TYPES) Enum.Parse(typeof(SEX_TYPES), attr.Value);
                        break;
                    case "birthyear":
                        participant.BirthYear = int.Parse(attr.Value);
                        break;
                    case "education":
                        participant.Education = attr.Value;
                        break;
                }
            }

            foreach (XmlNode attributes in participantNode.ChildNodes)
            {
                foreach (XmlNode attribute in attributes.ChildNodes)
                {
                    participant.AddExtraData(attribute.Name, attribute.InnerText);
                }
            }

            return participant;
        }

        /// <summary>
        /// Creates a copy of the participant
        /// </summary>
        /// <param name="orig">Original participant to copy</param>
        /// <returns>Copy of the given participant</returns>
        public static ExperimentParticipant Copy(ExperimentParticipant orig)
        {
            ExperimentParticipant copy = new ExperimentParticipant();

            copy.Identifier = "Kopie - " + orig.Identifier;
            copy.BirthYear = orig.BirthYear;
            copy.Education = (String) orig.Education.Clone();
            copy.Sex = orig.Sex;

            foreach (KeyValuePair<String, String> entry in orig.ExtraData)
            {
                copy.ExtraData.Add((String)entry.Key.Clone(), (String)entry.Value.Clone());
            }

            return copy;
        }
    }
}
