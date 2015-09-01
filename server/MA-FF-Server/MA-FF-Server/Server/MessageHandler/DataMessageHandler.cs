using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;
using WebAnalyzer.Controller;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Models.MessageModel;

namespace WebAnalyzer.Server.MessageHandler
{
    class DataMessageHandler : IObserver<Object>
    {

        readonly WebsocketConnection _connection;
        readonly TestController _controller;

        public DataMessageHandler(TestController controller, WebsocketConnection connection)
        {
            _controller = controller;
            _connection = connection;
        }

        public void OnCompleted()
        {
            Console.WriteLine("Data Message: Completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("Datamessage error: " + error.Message);
        }

        public void OnNext(Object omsgIn)
        {
            InDataMessage msgIn = (InDataMessage)omsgIn;

            //Logger.Log("Data Message uniqueid: " + msgIn.UniqueID + " with Type: " + msgIn.Type);

            if (msgIn.UniqueID != null)
            {
                GazeModel gaze = ExtractGazeData(msgIn);

                if (gaze != null)
                {
                    if (msgIn.URL != null)
                    {
                        _controller.AssignGazeToWebpage(gaze, msgIn.URL);
                    }
                }
            }
        }

        private GazeModel ExtractGazeData(InDataMessage msgIn)
        {

            String uniqueId = msgIn.UniqueID;

            GazeModel gazeModel = _controller.GetGazeModel(uniqueId);

            if (gazeModel != null)
            {

                gazeModel.ServerReceivedTimestamp = msgIn.ServerReceived;

                gazeModel.DataRequestedTimestamp = msgIn.RequestTimestamp;

                gazeModel.ServerSentTimestamp = msgIn.ServerSent;

                gazeModel.ClientSentTimestamp = msgIn.ClientSent;

                gazeModel.ClientReceivedTimestamp = msgIn.ClientReceived;

                gazeModel.LeftEye.Element = msgIn.LeftElement;
                gazeModel.RightEye.Element = msgIn.RightElement;
            }
            else
            {
                Logger.Log("Could not get corresponding GazeModel");
            }


            return gazeModel;
        }
    }
}
