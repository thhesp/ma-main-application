using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Models.Base;

using WebAnalyzer.Util;

namespace WebAnalyzer.Models.DataModel
{
    /// <summary>
    /// Representation of a gaze with data about both eyes.
    /// </summary>
    public class GazeModel : BasicData
    {

        /// <summary>
        /// Uniqueid of the gaze object
        /// </summary>
        private String _uniqueId;

        /// <summary>
        /// Data about the left eye
        /// </summary>
        private PositionDataModel _leftEye;

        /// <summary>
        /// Data about the right eye
        /// </summary>
        private PositionDataModel _rightEye;

        /// <summary>
        /// Timestamp of the gaze
        /// </summary>
        private String _timestamp;

        /// <summary>
        /// Constructor which creates the uid
        /// </summary>
        /// <param name="timestamp">Request timestamp</param>
        public GazeModel(String timestamp)
        {
            _uniqueId = this.GetUniqueId();
            _timestamp = timestamp;
        }

        /// <summary>
        /// Constructor for loading from xml
        /// </summary>
        public GazeModel()
        {

        }

        /// <summary>
        /// Creates a uniqueid
        /// </summary>
        /// <returns>Uniqueid</returns>
        private String GetUniqueId()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Returns data for the given eye
        /// </summary>
        /// <param name="eye">String representation of the eye</param>
        /// <returns></returns>
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

        /// <summary>
        /// Getter/ Setter for the uniqueid
        /// </summary>
        public String UniqueId
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }

        /// <summary>
        /// Getter/ Setter for the timestamp
        /// </summary>
        public String Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        /// <summary>
        /// Getter/ Setter for the left eye
        /// </summary>
        public PositionDataModel LeftEye
        {
            get { return _leftEye; }
            set { _leftEye = value; }
        }

        /// <summary>
        /// Getter/ Setter for the right eye
        /// </summary>
        public PositionDataModel RightEye
        {
            get { return _rightEye; }
            set { _rightEye = value; }
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="gazeNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
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
                            gaze.DataRequestedTimestamp = attr.Value;
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

        /// <summary>
        /// Generates a statistics xml node for the gaze
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
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
    }
}
