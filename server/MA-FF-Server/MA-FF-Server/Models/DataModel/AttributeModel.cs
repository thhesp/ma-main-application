using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.DataModel
{
    public class AttributeModel
    {
        private String _name;
        private String _value;

        public AttributeModel()
        {

        }

        public AttributeModel(String name, String value)
        {
            _name = name;
            _value = value;
        }

        #region GetterSetterFunctions

        public String Name
        {
            get { return _name; }
            set { _name = value;  }
        }


        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }


        #endregion

        #region XMLFunctions

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

        #endregion
    }
}
