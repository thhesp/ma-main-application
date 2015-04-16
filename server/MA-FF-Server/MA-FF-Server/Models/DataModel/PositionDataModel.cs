using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebAnalyzer.Models.DataModel
{
    class PositionDataModel
    {
        private int _x;
        private int _y;
        private String _serverSentTimestamp;
        private String _serverReceivedTimestamp;

        private String _clientSentTimestamp;
        private String _clientReceivedTimestamp;

        private String _tag;
        private String _id;
        private String _title;

        private List<String> _classes = new List<String>();
        private List<AttributeModel> _attributes = new List<AttributeModel>();


        private PositionDataModel _nextPosition;

        public PositionDataModel(int x, int y, String receivedTimestamp)
        {
            _x = x;
            _y = y;
            _serverReceivedTimestamp = receivedTimestamp;
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

        public String ServerSentTimestamp
        {
            get { return _serverSentTimestamp; }
            set { _serverSentTimestamp = value; }
        }

        public String ServerReceivedTimestamp
        {
            get { return _serverReceivedTimestamp; }
            set { _serverReceivedTimestamp = value; }
        }

        public String ClientSentTimestamp
        {
            get { return _clientSentTimestamp; }
            set { _clientSentTimestamp = value; }
        }

        public String ClientReceivedTimestamp
        {
            get { return _clientReceivedTimestamp; }
            set { _clientReceivedTimestamp = value; }
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

            //server sent timestamp

            XmlAttribute serverSentTimestamp = xmlDoc.CreateAttribute("server-sent-timestamp");

            serverSentTimestamp.Value = this.ServerSentTimestamp;

            positionNode.Attributes.Append(serverSentTimestamp);

            // server received timestamp

            XmlAttribute serverReceivedTimestamp = xmlDoc.CreateAttribute("server-received-timestamp");

            serverReceivedTimestamp.Value = this.ServerReceivedTimestamp;

            positionNode.Attributes.Append(serverReceivedTimestamp);

            //client sent timestamp

            XmlAttribute clientSentTimestamp = xmlDoc.CreateAttribute("client-sent-timestamp");

            clientSentTimestamp.Value = this.ClientSentTimestamp;

            positionNode.Attributes.Append(clientSentTimestamp);


            //client received timestamp

            XmlAttribute clientReceivedTimestamp = xmlDoc.CreateAttribute("client-received-timestamp");

            clientReceivedTimestamp.Value = this.ClientReceivedTimestamp;

            positionNode.Attributes.Append(clientReceivedTimestamp);

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
