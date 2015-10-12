using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.Base
{
    /// <summary>
    /// Class which represents a raw gaze sent from the tracking component
    /// </summary>
    public class RawTrackingGaze
    {
        /// <summary>
        /// Gaze data for the left eye
        /// </summary>
        private BaseTrackingData _leftEye;

        /// <summary>
        /// Gaze data for the right eye
        /// </summary>
        private BaseTrackingData _rightEye;

        /// <summary>
        /// Empty constructor used for loading from xml
        /// </summary>
        public RawTrackingGaze()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leftEye">Data about the left eye</param>
        /// <param name="rightEye">Data about the right eye</param>
        public RawTrackingGaze(BaseTrackingData leftEye, BaseTrackingData rightEye)
        {
            _leftEye = leftEye;
            _rightEye = rightEye;
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode gazeNode = xmlDoc.CreateElement("raw-gaze");

            XmlNode leftEye = xmlDoc.CreateElement("left-eye");

            leftEye.AppendChild(_leftEye.ToXML(xmlDoc));

            gazeNode.AppendChild(leftEye);


            //right eye

            XmlNode rightEye = xmlDoc.CreateElement("right-eye");

            rightEye.AppendChild(_rightEye.ToXML(xmlDoc));

            gazeNode.AppendChild(rightEye);

            return gazeNode;
        }

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="gazeNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
        public static RawTrackingGaze LoadFromXML(XmlNode gazeNode)
        {
            if (gazeNode.Name == "raw-gaze")
            {
                RawTrackingGaze gaze = new RawTrackingGaze();

                foreach (XmlNode eyes in gazeNode.ChildNodes)
                {
                    if (eyes.Name == "left-eye")
                    {
                        BaseTrackingData etData = BaseTrackingData.LoadFromXML(eyes.FirstChild);

                        if (etData != null)
                        {
                            gaze._leftEye = etData;
                        }
                    }
                    else if (eyes.Name == "right-eye")
                    {
                        BaseTrackingData etData = BaseTrackingData.LoadFromXML(eyes.FirstChild);

                        if (etData != null)
                        {
                            gaze._rightEye = etData;
                        }
                    }
                }

                return gaze;

            }

            return null;
        }
    }
}
