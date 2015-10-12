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
    /// <summary>
    /// MessageHandler for Data messages
    /// </summary>
    class DataMessageHandler : IObserver<Object>
    {
        /// <summary>
        /// Readonly reference to the websocket connection
        /// </summary>
        readonly WebsocketConnection _connection;

        /// <summary>
        /// Readonly reference to the testcontroller used for sending data
        /// </summary>
        readonly TestController _controller;

        /// <summary>
        /// Constructor with the websocket connection and the test controller
        /// </summary>
        /// <param name="controller">The testcontroller</param>
        /// <param name="connection">The connection which received the message</param>
        public DataMessageHandler(TestController controller, WebsocketConnection connection)
        {
            _controller = controller;
            _connection = connection;
        }

        /// <summary>
        /// Method which gets called once the processing of the message is completed.
        /// </summary>
        public void OnCompleted()
        {
            Console.WriteLine("Data Message: Completed");
        }

        /// <summary>
        /// Method which gets called if an error occurs while processing the message.
        /// </summary>
        /// <param name="error">Errorexception which occured</param>
        public void OnError(Exception error)
        {
            Console.WriteLine("Datamessage error: " + error.Message);
        }

        /// <summary>
        /// Method used for processing the message
        /// </summary>
        /// <param name="omsgIn">The Message which was received</param>
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
                        _controller.AssignGazeToWebpage(gaze, msgIn.URL, _connection.UID);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the gaze model for the uniqueid of the data message with the help of the testcontroller
        /// </summary>
        /// <param name="msgIn">The data Message</param>
        /// <returns>The corresponding gaze model</returns>
        private GazeModel ExtractGazeData(InDataMessage msgIn)
        {

            String uniqueId = msgIn.UniqueID;

            GazeModel gazeModel = _controller.GetGazeModel(uniqueId);

            if (gazeModel != null)
            {

                gazeModel.ServerReceivedTimestamp = msgIn.ServerReceived;

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
