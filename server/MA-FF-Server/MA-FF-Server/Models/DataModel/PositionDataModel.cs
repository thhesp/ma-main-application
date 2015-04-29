using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Models.DataModel
{
    class PositionDataModel : BasicRawData
    {

        private String _uniqueId;

        private EyeTrackingData _eyeTrackingData;

        private DOMElementModel _element;

        private PositionDataModel _nextPosition;

        public PositionDataModel()
        {
            _uniqueId = this.GetUniqueId();
        }

        public PositionDataModel(double x, double y) : this()
        {
            _eyeTrackingData = new EyeTrackingData(x, y);
        }

        public PositionDataModel(double x, double y, String receivedTimestamp) : this(x,y)
        {
            _serverReceivedTimestamp = receivedTimestamp;
        }

        private String GetUniqueId()
        {
            return Guid.NewGuid().ToString();
        }

        #region GetterSetterFunctions

        public String UniqueId
        {
            get { return _uniqueId; }
        }

        public EyeTrackingData EyeTrackingData
        {
            get { return _eyeTrackingData; }
            set { _eyeTrackingData = value; }
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

            //eyetracking data

            positionNode.AppendChild(this.EyeTrackingData.ToXML(xmlDoc));


            //element data

            positionNode.AppendChild(this.Element.ToXML(xmlDoc));

            return positionNode;
        }

        #endregion
    }
}
