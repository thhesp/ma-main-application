﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Models.DataModel
{
    class GazeModel : BasicRawData
    {
        
        private String _uniqueId;


        private PositionDataModel _leftEye;
        private PositionDataModel _rightEye;

        private String _timestamp;

        public GazeModel(String timestamp)
        {
             _uniqueId = this.GetUniqueId();
            _timestamp = timestamp;
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

        public String Timestamp
        {
            get { return _timestamp; }
        }

        public PositionDataModel LeftEye
        {
            get { return _leftEye; }
            set { _leftEye = value; }
        }

        public PositionDataModel RightEye
        {
            get { return _rightEye; }
            set { _rightEye = value; }
        }
        #endregion

        #region XMLFunctions

        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode gazeNode = xmlDoc.CreateElement("gaze");

            //server sent timestamp

            XmlAttribute serverSentTimestamp = xmlDoc.CreateAttribute("server-sent-timestamp");

            serverSentTimestamp.Value = this.ServerSentTimestamp;

            gazeNode.Attributes.Append(serverSentTimestamp);

            // server received timestamp

            XmlAttribute serverReceivedTimestamp = xmlDoc.CreateAttribute("server-received-timestamp");

            serverReceivedTimestamp.Value = this.ServerReceivedTimestamp;

            gazeNode.Attributes.Append(serverReceivedTimestamp);

            //client sent timestamp

            XmlAttribute clientSentTimestamp = xmlDoc.CreateAttribute("client-sent-timestamp");

            clientSentTimestamp.Value = this.ClientSentTimestamp;

            gazeNode.Attributes.Append(clientSentTimestamp);


            //client received timestamp

            XmlAttribute clientReceivedTimestamp = xmlDoc.CreateAttribute("client-received-timestamp");

            clientReceivedTimestamp.Value = this.ClientReceivedTimestamp;

            gazeNode.Attributes.Append(clientReceivedTimestamp);



            //left eye

            XmlNode leftEye = xmlDoc.CreateElement("left-eye");

            leftEye.AppendChild(this.LeftEye.ToXML(xmlDoc));

            gazeNode.AppendChild(leftEye);


            //right eye

            XmlNode rightEye = xmlDoc.CreateElement("right-eye");

            rightEye.AppendChild(this.RightEye.ToXML(xmlDoc));

            gazeNode.AppendChild(rightEye);

            return gazeNode;
        }

        #endregion
    }
}
