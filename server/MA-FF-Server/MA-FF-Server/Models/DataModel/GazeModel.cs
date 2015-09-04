﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Models.Base;

using WebAnalyzer.Util;

namespace WebAnalyzer.Models.DataModel
{
    public class GazeModel : BasicRawData
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

        public GazeModel()
        {

        }
        
        private String GetUniqueId()
        {
            return Guid.NewGuid().ToString();
        }

        public PositionDataModel GetEyeData(String eye)
        {

            //@Todo: extract values to constants
            if (eye == "left")
            {
                return _leftEye;
            }
            else if (eye == "right")
            {
                return _rightEye;
            }

            return null;
        }

        #region GetterSetterFunctions

        public String UniqueId
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }

        public String Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
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

            //timestamp

            XmlAttribute timestamp = xmlDoc.CreateAttribute("timestamp");

            timestamp.Value = this.Timestamp;

            gazeNode.Attributes.Append(timestamp);

            // data requested timestamp

            XmlAttribute dataRequestedTimestamp = xmlDoc.CreateAttribute("data-requested-timestamp");

            dataRequestedTimestamp.Value = this.DataRequestedTimestamp;

            gazeNode.Attributes.Append(dataRequestedTimestamp);


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

        public static GazeModel LoadFromXML(XmlNode gazeNode)
        {
            if (gazeNode.Name == "gaze")
            {
                GazeModel gaze = new GazeModel();

                foreach (XmlAttribute attr in gazeNode.Attributes)
                {
                    switch (attr.Name)
                    {
                        case "timestamp":
                            gaze.Timestamp = attr.Value;
                            break;
                        case "data-requested-timestamp":
                            gaze.Timestamp = attr.Value;
                            break;
                        case "server-sent-timestamp":
                            gaze.ServerSentTimestamp = attr.Value;
                            break;
                        case "server-received-timestamp":
                            gaze.ServerReceivedTimestamp = attr.Value;
                            break;
                        case "client-sent-timestamp":
                            gaze.ClientSentTimestamp = attr.Value;
                            break;
                        case "client-received-timestamp":
                            gaze.ClientReceivedTimestamp = attr.Value;
                            break;
                    }
                }

                foreach (XmlNode eyes in gazeNode.ChildNodes)
                {
                    if (eyes.Name == "left-eye")
                    {
                        PositionDataModel posModel = PositionDataModel.LoadFromXML(eyes.FirstChild);

                        if (posModel != null)
                        {
                            gaze.LeftEye = posModel;
                        }
                    }
                    else if (eyes.Name == "right-eye")
                    {
                        PositionDataModel posModel = PositionDataModel.LoadFromXML(eyes.FirstChild);

                        if (posModel != null)
                        {
                            gaze.RightEye = posModel;
                        }
                    }
                }

                return gaze;
            }

            Logger.Log("Wrong node type given");

            return null;
        }

        public XmlNode GenerateStatisticsXML(XmlDocument xmlDoc)
        {
            XmlNode gazeNode = xmlDoc.CreateElement("gaze");

            // create & insert statistics?


            XmlAttribute durRequestTillSent = xmlDoc.CreateAttribute("duration-from-request-till-sent");

            durRequestTillSent.Value = this.DurationFromRequestTillSending().ToString();

            gazeNode.Attributes.Append(durRequestTillSent);


            XmlAttribute durServerSentReceived = xmlDoc.CreateAttribute("duration-from-server-sent-to-received");

            durServerSentReceived.Value = this.DurationFromServerSentToReceived().ToString();

            gazeNode.Attributes.Append(durServerSentReceived);

            XmlAttribute durClientReceivedSent = xmlDoc.CreateAttribute("duration-from-client-received-to-sent");

            durClientReceivedSent.Value = this.DurationFromClientReceivedToClientSent().ToString();

            gazeNode.Attributes.Append(durClientReceivedSent);

            return gazeNode;
        }

        #endregion
    }
}
