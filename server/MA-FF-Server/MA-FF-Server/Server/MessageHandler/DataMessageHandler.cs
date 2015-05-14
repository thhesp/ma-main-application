using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;
using WebAnalyzer.Experiment;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Server.MessageHandler
{
    class DataMessageHandler : IObserver<Object>
    {
        readonly ConnectionManager _connectionManager;
        readonly WebsocketConnection _connection;

        public DataMessageHandler(ConnectionManager connectionManager, WebsocketConnection connection)
        {
            _connectionManager = connectionManager;
            _connection = connection;
        }

        public void OnCompleted()
        {
            Console.WriteLine("Data Message: Completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("Data message : " + error.Message);
        }

        public void OnNext(Object omsgIn)
        {
            dynamic msgIn = omsgIn;

            // extract data from json object and handle it
            Logger.Log("Data Message uniqueid: " + msgIn.uniqueid);

            if (msgIn.uniqueid != null)
            {

                if (CheckForError(msgIn))
                {
                    String uniqueId = msgIn.uniqueid;
                    ExperimentController.getInstance().DisposeOfGazeData(uniqueId);
                }

                GazeModel gaze = ExtractGazeData(msgIn);

                if (gaze != null)
                {
                    String url = msgIn.url;

                    if (url != null)
                    {
                        ExperimentController.getInstance().AssignGazeToWebpage(gaze, url);
                    }
                }
            }
        }

        private GazeModel ExtractGazeData(dynamic msgIn)
        {
            String serverReceivedTimestamp = Timestamp.GetMillisecondsUnixTimestamp();

            String uniqueId = msgIn.uniqueid;

            GazeModel gazeModel = ExperimentController.getInstance().GetGazeModel(uniqueId);

            if (gazeModel != null)
            {

                gazeModel.ServerReceivedTimestamp = serverReceivedTimestamp;

                String requestedTimestamp = msgIn.datarequest;

                if (requestedTimestamp != null)
                {
                    gazeModel.DataRequestedTimestamp = requestedTimestamp;
                }

                String serverTimestamp = msgIn.serversent;

                if (serverTimestamp != null)
                {
                    gazeModel.ServerSentTimestamp = serverTimestamp;
                }

                String clientSentTimestamp = msgIn.clientsent;

                if (clientSentTimestamp != null)
                {
                    gazeModel.ClientSentTimestamp = clientSentTimestamp;
                }


                String clientReceivedTimestamp = msgIn.clientreceived;

                if (clientReceivedTimestamp != null)
                {
                    gazeModel.ClientReceivedTimestamp = clientReceivedTimestamp;
                }

                gazeModel.LeftEye = ExtractPositionData(gazeModel.LeftEye, msgIn.left);
                gazeModel.RightEye = ExtractPositionData(gazeModel.RightEye, msgIn.right);

            }else{
                Logger.Log("Could not get corresponding GazeModel");
            }


            return gazeModel;
        }

        private Boolean CheckForError(dynamic msgIn)
        {
            if ( (msgIn.left.command != null && msgIn.left.command == "error" ) && 
                ( msgIn.right.command != null && msgIn.right.command == "error" ))
            {
                return true;
            }

            return false;
        }

        private PositionDataModel ExtractPositionData(PositionDataModel posModel, dynamic msgIn)
        {

            Logger.Log("MsgIn: "+ msgIn);

            Logger.Log("Data Message: X: " + msgIn.x + " Y: " + msgIn.y);

            if (posModel != null)
            {
                // get element data 

                posModel.Element = ExtractElementData(msgIn);
            }

            return posModel;
        }

        private DOMElementModel ExtractElementData(dynamic msgIn)
        {
            DOMElementModel elementModel = new DOMElementModel();

            String id = msgIn.id;

            if (id != null)
            {
                elementModel.ID = id;
            }

            String tag = msgIn.tag;

            if (tag != null)
            {
                elementModel.Tag = tag;
            }

            String title = msgIn.title;

            if (title != null)
            {
                elementModel.Title = title;
            }

            dynamic element = msgIn.element;

            if (element != null)
            {
                int left = element.left;

                if (left != null)
                {
                    elementModel.Left = left;
                }

                int top = element.top;

                if (top != null)
                {
                    elementModel.Top = top;
                }

                int width = element.width;

                if (width != null)
                {
                    elementModel.Width = width;
                }

                int height = element.height;

                if (height != null)
                {
                    elementModel.Height = height;
                }

                int outerWidth = element.outerWidth;

                if (outerWidth != null)
                {
                    elementModel.OuterWidth = outerWidth;
                }

                int outerHeight = element.outerHeight;

                if (outerHeight != null)
                {
                    elementModel.OuterHeight = outerHeight;
                }
            }

            if (msgIn.classes != null)
            {
                foreach (String className in msgIn.classes)
                {
                    elementModel.AddClass(className);
                }
            }

            if (msgIn.attributes != null)
            {
                foreach (dynamic attr in msgIn.attributes)
                {
                    String name = attr.name;
                    String value = attr.value;

                    if (name != null && value != null)
                    {
                        elementModel.AddAttribute(name, value);
                    }
                }
            }

            return elementModel;
        }
    }
}
