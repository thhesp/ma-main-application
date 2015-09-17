﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.Base
{
    public class RawTrackingGaze
    {
        private EyeTrackingData _leftEye;
        private EyeTrackingData _rightEye;

        public RawTrackingGaze()
        {
        }

        public RawTrackingGaze(EyeTrackingData leftEye, EyeTrackingData rightEye)
        {
            _leftEye = leftEye;
            _rightEye = rightEye;
        }

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

        public static RawTrackingGaze LoadFromXML(XmlNode gazeNode)
        {
            if (gazeNode.Name == "raw-gaze")
            {
                RawTrackingGaze gaze = new RawTrackingGaze();

                foreach (XmlNode eyes in gazeNode.ChildNodes)
                {
                    if (eyes.Name == "left-eye")
                    {
                        EyeTrackingData etData = EyeTrackingData.LoadFromXML(eyes.FirstChild);

                        if (etData != null)
                        {
                            gaze._leftEye = etData;
                        }
                    }
                    else if (eyes.Name == "right-eye")
                    {
                        EyeTrackingData etData = EyeTrackingData.LoadFromXML(eyes.FirstChild);

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
