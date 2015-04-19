using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Models.DataModel
{
    class PositionDataModel : BasicData
    {
        private int _x;
        private int _y;

        private DOMElementModel _element;

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


        public DOMElementModel Element
        {
            get { return _element; }
            set { _element = value; }
        }

        public PositionDataModel NextPosition
        {
            get { return _nextPosition; }
            set { _nextPosition = value;  }
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

            
            //element data

            positionNode.AppendChild(_element.ToXML(xmlDoc));

            return positionNode;
        }

        #endregion
    }
}
