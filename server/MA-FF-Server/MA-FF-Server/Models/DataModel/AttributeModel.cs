using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.DataModel
{
    /// <summary>
    /// Model for representing HTML element attributes
    /// </summary>
    public class AttributeModel
    {
        /// <summary>
        /// Attribute name
        /// </summary>
        private String _name;

        /// <summary>
        /// attribute value
        /// </summary>
        private String _value;

        /// <summary>
        /// Constructor for loading from xml
        /// </summary>
        public AttributeModel()
        {

        }

        /// <summary>
        /// Constructor with name and value
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">attribute value</param>
        public AttributeModel(String name, String value)
        {
            _name = name;
            _value = value;
        }

        /// <summary>
        /// Getter/ Setter for the name
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value;  }
        }

        /// <summary>
        /// Getter/ Setter for the value
        /// </summary>
        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode attributeNode = xmlDoc.CreateElement("attribute");

            XmlAttribute name = xmlDoc.CreateAttribute("name");

            name.Value = this.Name;

            attributeNode.Attributes.Append(name);

            XmlAttribute value = xmlDoc.CreateAttribute("value");

            value.Value = this.Value;

            attributeNode.Attributes.Append(value);

            return attributeNode;
        }

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="attrNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
        public static AttributeModel LoadFromXML(XmlNode attrNode)
        {
            if (attrNode.Name == "attribute")
            {
                AttributeModel attrModel = new AttributeModel();

                foreach (XmlAttribute attr in attrNode.Attributes)
                {
                    switch (attr.Name)
                    {
                        case "name":
                            attrModel.Name = attr.Value;
                            break;
                        case "value":
                            attrModel.Value = attr.Value;
                            break;
                    }
                }

                return attrModel;
            }

            Logger.Log("Wrong node type given");

            return null;
        }
    }
}
