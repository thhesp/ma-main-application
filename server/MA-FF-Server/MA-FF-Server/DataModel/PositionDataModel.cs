using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebAnalyzer.DataModel
{
    class PositionDataModel
    {
        private int _x;
        private int _y;
        private String _timestamp;

        private String _tag;
        private String _id;
        private String _title;

        private List<String> _classes = new List<String>();
        private List<AttributeModel> _attributes = new List<AttributeModel>();


        private PositionDataModel _nextPosition;

        public PositionDataModel(int x, int y, String timestamp)
        {
            _x = x;
            _y = y;
            _timestamp = timestamp;
        }

        #region GetterSetterFunctions

        public int X
        {
            get { return _x; }
            set { _x = value;  }
        }


        public int Y
        {
            get { return _y; }
            set { _y = value;  }
        }

        public String Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }


        public String Tag
        {
            get { return _tag; }
            set { _tag = value;  }
        }


        public String ID
        {
            get { return _id; }
            set { _id = value;  }
        }

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public PositionDataModel NextPosition
        {
            get { return _nextPosition; }
            set { _nextPosition = value; }
        }

        public void AddClass(String className)
        {
            _classes.Add(className);
        }

        public void AddAttribute(String name, String value)
        {
            AttributeModel attrModel = new AttributeModel(name, value);

            _attributes.Add(attrModel);
        }

        
        #endregion

        #region XMLFunctions

        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode positionNode = xmlDoc.CreateElement("position");

            //x

            XmlAttribute xPosition = xmlDoc.CreateAttribute("x");

            xPosition.Value = this.X.ToString();

            positionNode.Attributes.Append(xPosition);

            //y

            XmlAttribute yPosition = xmlDoc.CreateAttribute("y");

            yPosition.Value = this.Y.ToString();

            positionNode.Attributes.Append(yPosition);

            //timestamp

            XmlAttribute timestamp = xmlDoc.CreateAttribute("timestamp");

            timestamp.Value = this.Timestamp;

            positionNode.Attributes.Append(timestamp);

            //tag

            XmlAttribute tag = xmlDoc.CreateAttribute("tag");

            tag.Value = this.Tag;

            positionNode.Attributes.Append(tag);

            //id

            XmlAttribute id = xmlDoc.CreateAttribute("id");

            id.Value = this.ID;

            positionNode.Attributes.Append(id);

            //title


            XmlAttribute title = xmlDoc.CreateAttribute("title");

            title.Value = this.Title;

            positionNode.Attributes.Append(title);

            //classes

            XmlNode classesNode = xmlDoc.CreateElement("classes");

            foreach (String className in _classes)
            {
                XmlNode classNode = xmlDoc.CreateElement("class");
                classNode.InnerText = className;
                classesNode.AppendChild(classNode);

            }

            positionNode.AppendChild(classesNode);

            //attributes

            XmlNode attributesNode = xmlDoc.CreateElement("attributes");

            foreach (AttributeModel attribute in _attributes)
            {
                attributesNode.AppendChild(attribute.ToXML(xmlDoc));
            }

            positionNode.AppendChild(attributesNode);

            return positionNode;
        }

        #endregion
    }
}
