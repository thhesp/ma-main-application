﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Models.AnalysisModel;
using WebAnalyzer.Util;
using WebAnalyzer.Models.SettingsModel;

namespace WebAnalyzer.Models.DataModel
{
    public class WebpageModel
    {

        private String _url;
        private String _visitTimestamp;
        private List<GazeModel> _positionData = new List<GazeModel>();

        /* Fixations */
        private List<FixationModel> _leftFixationData = new List<FixationModel>();
        private List<FixationModel> _rightFixationData = new List<FixationModel>();

        public WebpageModel(String url, String visitTimestamp)
        {
            _url = url;
            _visitTimestamp = visitTimestamp;
        }

        #region GetterSetterFunctions

        public String Url
        {
            get { return _url; }
            set { _url = value;}
        }

        public String VisitTimestamp
        {
            get { return _visitTimestamp; }
            set { _visitTimestamp = value; }
        }

        public void AddGazeData(GazeModel data)
        {
            _positionData.Add(data);
        }

        #endregion

        #region XMLFunctions

        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode webpageNode = xmlDoc.CreateElement("webpage");

            XmlAttribute url = xmlDoc.CreateAttribute("url");

            url.Value = this.Url;

            webpageNode.Attributes.Append(url);

            XmlAttribute visited = xmlDoc.CreateAttribute("visited");

            visited.Value = this.VisitTimestamp;

            webpageNode.Attributes.Append(visited);

            foreach (GazeModel data in _positionData)
            {
                webpageNode.AppendChild(data.ToXML(xmlDoc));
            }

            return webpageNode;
        }

        public XmlNode GenerateStatisticsXML(XmlDocument xmlDoc)
        {
            XmlNode webpageNode = xmlDoc.CreateElement("webpage");

            XmlAttribute url = xmlDoc.CreateAttribute("url");

            url.Value = this.Url;

            webpageNode.Attributes.Append(url);

            XmlAttribute visited = xmlDoc.CreateAttribute("visited");

            visited.Value = this.VisitTimestamp;

            // create & insert statistics?

            webpageNode.AppendChild(CreateWebpageStatistics(xmlDoc));

            XmlNode gazesNode = xmlDoc.CreateElement("gazes");

            foreach (GazeModel data in _positionData)
            {
                gazesNode.AppendChild(data.GenerateStatisticsXML(xmlDoc));
            }

            webpageNode.AppendChild(gazesNode);


            return webpageNode;
        }

        private XmlNode CreateWebpageStatistics(XmlDocument xmlDoc)
        {
            // sum of statistics?
            XmlNode statsNode = xmlDoc.CreateElement("webpage-stats");

            XmlNode requestTillSent = xmlDoc.CreateElement("request-till-sent");

            InsertArrayStatistics(xmlDoc, requestTillSent, ArrayOfDurationFromRequestTillSent());

            statsNode.AppendChild(requestTillSent);

            XmlNode serverSentToReceived = xmlDoc.CreateElement("server-sent-to-received");

            InsertArrayStatistics(xmlDoc, serverSentToReceived, ArrayOfDurationFromServerSentToReceived());

            statsNode.AppendChild(serverSentToReceived);

            XmlNode clientReceivedToSent = xmlDoc.CreateElement("client-received-to-sent");

            InsertArrayStatistics(xmlDoc, clientReceivedToSent, ArrayOfDurationFromClientReceivedToClientSent());

            statsNode.AppendChild(clientReceivedToSent);


            return statsNode;
        }

        private void InsertArrayStatistics(XmlDocument xmlDoc, XmlNode node, long[] array)
        {
            XmlAttribute mean = xmlDoc.CreateAttribute("mean");

            mean.Value = Statistics.CalculateMean(array).ToString();

            node.Attributes.Append(mean);

            XmlAttribute median = xmlDoc.CreateAttribute("median");

            median.Value = Statistics.CalculateMedian(array).ToString();

            node.Attributes.Append(median);

            XmlAttribute min = xmlDoc.CreateAttribute("min");

            min.Value = array.Min().ToString();

            node.Attributes.Append(min);


            XmlAttribute max = xmlDoc.CreateAttribute("max");

            max.Value = array.Max().ToString();

            node.Attributes.Append(max);

        }

        public long[] ArrayOfDurationFromRequestTillSent()
        {
            long[] durations = new long[_positionData.Count];
            for (int i = 0; i < _positionData.Count; i++)
            {
                durations[i] = _positionData[i].DurationFromRequestTillSending();
            }

            return durations;
        }

        public long[] ArrayOfDurationFromServerSentToReceived()
        {
            long[] durations = new long[_positionData.Count];
            for (int i = 0; i < _positionData.Count; i++)
            {
                durations[i] = _positionData[i].DurationFromServerSentToReceived();
            }

            return durations;
        }


        public long[] ArrayOfDurationFromClientReceivedToClientSent()
        {
            long[] durations = new long[_positionData.Count];
            for (int i = 0; i < _positionData.Count; i++)
            {
                durations[i] = _positionData[i].DurationFromClientReceivedToClientSent();
            }

            return durations;
        }

        public XmlNode GenerateFixationXML(XmlDocument xmlDoc)
        {
            ExtractFixations();

            XmlNode webpageNode = xmlDoc.CreateElement("webpage");

            XmlAttribute url = xmlDoc.CreateAttribute("url");

            url.Value = this.Url;

            webpageNode.Attributes.Append(url);

            XmlAttribute visited = xmlDoc.CreateAttribute("visited");

            visited.Value = this.VisitTimestamp;

            webpageNode.Attributes.Append(visited);


            // insert fixations

            XmlNode fixationsNode = xmlDoc.CreateElement("fixations");

            XmlNode leftEyeNode = xmlDoc.CreateElement("left-eye");

            XmlAttribute leftFixationsCount = xmlDoc.CreateAttribute("count-of-fixations");

            leftFixationsCount.Value = _leftFixationData.Count.ToString();

            leftEyeNode.Attributes.Append(leftFixationsCount);

            foreach (FixationModel data in _leftFixationData)
            {
                leftEyeNode.AppendChild(data.ToXML(xmlDoc));
            }

            fixationsNode.AppendChild(leftEyeNode);

            XmlNode rightEyeNode = xmlDoc.CreateElement("right-eye");

            XmlAttribute rightFixationsCount = xmlDoc.CreateAttribute("count-of-fixations");

            rightFixationsCount.Value = _rightFixationData.Count.ToString();

            rightEyeNode.Attributes.Append(rightFixationsCount);

            foreach (FixationModel data in _rightFixationData)
            {
                rightEyeNode.AppendChild(data.ToXML(xmlDoc));
            }

            fixationsNode.AppendChild(rightEyeNode);

            webpageNode.AppendChild(fixationsNode);


            return webpageNode;
        }

        public XmlNode GenerateAOIXML(ExperimentSettings settings, XmlDocument xmlDoc)
        {
            ExtractFixations();

            XmlNode webpageNode = xmlDoc.CreateElement("webpage");

            XmlAttribute url = xmlDoc.CreateAttribute("url");

            url.Value = this.Url;

            webpageNode.Attributes.Append(url);

            XmlAttribute visited = xmlDoc.CreateAttribute("visited");

            visited.Value = this.VisitTimestamp;

            webpageNode.Attributes.Append(visited);


            // insert fixations

            XmlNode aoisNode = xmlDoc.CreateElement("aois");

            XmlNode leftEyeNode = xmlDoc.CreateElement("left-eye");

            XmlAttribute leftFixationsCount = xmlDoc.CreateAttribute("count-of-fixations");

            leftFixationsCount.Value = _leftFixationData.Count.ToString();

            leftEyeNode.Attributes.Append(leftFixationsCount);

            foreach (FixationModel data in _leftFixationData)
            {
                XmlNode node = data.ToAOIXML(settings, this, "left", xmlDoc);

                if (node != null)
                {
                    leftEyeNode.AppendChild(node);
                }
            }

            aoisNode.AppendChild(leftEyeNode);

            XmlNode rightEyeNode = xmlDoc.CreateElement("right-eye");

            XmlAttribute rightFixationsCount = xmlDoc.CreateAttribute("count-of-fixations");

            rightFixationsCount.Value = _rightFixationData.Count.ToString();

            rightEyeNode.Attributes.Append(rightFixationsCount);

            foreach (FixationModel data in _rightFixationData)
            {

                XmlNode node = data.ToAOIXML(settings, this, "right", xmlDoc);

                if (node != null)
                {
                    leftEyeNode.AppendChild(node);
                }
            }

            aoisNode.AppendChild(rightEyeNode);

            webpageNode.AppendChild(aoisNode);


            return webpageNode;
        }

        #endregion


        #region FixationFunctions

        private void ExtractFixations(double acceptableDeviations)
        {
            ExtractEyeFixations("left", acceptableDeviations);
            ExtractEyeFixations("right", acceptableDeviations);

        }

        private void ExtractEyeFixations(String eye, double acceptableDeviations)
        {
            long duration = 0;
            List<GazeModel> relatedGazes = new List<GazeModel>();


            // iniate with first gaze data element
            PositionDataModel posModel = _positionData[0].GetEyeData(eye);

            double startX = posModel.EyeTrackingData.X - acceptableDeviations;
            double startY = posModel.EyeTrackingData.Y - acceptableDeviations;

            double endX = posModel.EyeTrackingData.X + acceptableDeviations;
            double endY = posModel.EyeTrackingData.Y + acceptableDeviations;

            String startTimestamp = _positionData[0].DataRequestedTimestamp;

            relatedGazes.Add(_positionData[0]);

            //start loop with second element

            for (int pos = 1; pos < _positionData.Count; pos++)
            {
                // get element with data
                PositionDataModel currentPos = _positionData[pos].GetEyeData(eye);

                if ((startX <= currentPos.EyeTrackingData.X && endX >= currentPos.EyeTrackingData.X) &&
                    (startY <= currentPos.EyeTrackingData.Y && endY >= currentPos.EyeTrackingData.Y)) {
                    duration = long.Parse(_positionData[pos].DataRequestedTimestamp) - long.Parse(startTimestamp);
                    //found related gaze
                    relatedGazes.Add(_positionData[pos]);
                } else {
                    //fixation (if there was one) ends
                    //@Todo: add constant/ setting for duration min time
                    if (duration >= 100) {
                        //create fixation
                        FixationModel fixation = new FixationModel(duration, eye);

                        fixation.From(startX, startY);
                        fixation.To(endX, endY);
                        fixation.RelatedGazes = relatedGazes;

                        if (eye == "left") {
                            _leftFixationData.Add(fixation);
                        }
                        else if (eye == "right") {
                            _rightFixationData.Add(fixation);
                        }

                    } else {
                        //Logger.Log("Fixation to short :(");
                    }


                    //clear related gazes
                    relatedGazes = new List<GazeModel>();

                    // iniate with current gaze element
                    startX = currentPos.EyeTrackingData.X - acceptableDeviations;
                    startY = currentPos.EyeTrackingData.Y - acceptableDeviations;

                    endX = currentPos.EyeTrackingData.X + acceptableDeviations;
                    endY = currentPos.EyeTrackingData.Y + acceptableDeviations;

                    startTimestamp = _positionData[pos].DataRequestedTimestamp;

                    duration = 0;

                    relatedGazes.Add(_positionData[pos]);
                }
            }

            //clear up a possible ending fixation

            if (duration >= 100)
            {
                //Logger.Log("Saving last fixation...");
                //create fixation
                FixationModel fixation = new FixationModel(duration, eye);

                fixation.From(startX, startY);
                fixation.To(endX, endY);
                fixation.RelatedGazes = relatedGazes;

                if (eye == "left")
                {
                    _leftFixationData.Add(fixation);
                }
                else if (eye == "right")
                {
                    _rightFixationData.Add(fixation);
                }

            }
            else
            {
                //Logger.Log("Last fixation to short :(");
            }
        }

        private void ExtractFixations()
        {
            this.ExtractFixations(0);
        }

        #endregion
    }
}
