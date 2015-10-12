using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.MessageModel;
using WebAnalyzer.Controller;

namespace WebAnalyzer.Server.MessageHandler
{
    /// <summary>
    /// MessageHandler for Error messages
    /// </summary>
    class ErrorMessageHandler : IObserver<Object>
    {
        /// <summary>
        /// Readonly reference to the testcontroller used for sending data
        /// </summary>
        readonly TestController _controller;

        /// <summary>
        /// Readonly reference to the websocket connection
        /// </summary>
        readonly WebsocketConnection _connection;

        /// <summary>
        /// Constructor with the websocket connection and the test controller
        /// </summary>
        /// <param name="controller">The testcontroller</param>
        /// <param name="connection">The connection which received the message</param>
        public ErrorMessageHandler(TestController controller, WebsocketConnection connection)
        {
            _controller = controller;
            _connection = connection;
        }

        /// <summary>
        /// Method which gets called once the processing of the message is completed.
        /// </summary>
        public void OnCompleted()
        {
            Console.WriteLine("Error Message: Completed");
        }

        /// <summary>
        /// Method which gets called if an error occurs while processing the message.
        /// </summary>
        /// <param name="error">Errorexception which occured</param>
        public void OnError(Exception error)
        {
            Console.WriteLine("Error message : " + error.Message);
        }

        /// <summary>
        /// Method used for processing the message
        /// </summary>
        /// <param name="omsgIn">The Message which was received</param>
        public void OnNext(Object omsgIn)
        {
            ErrorMessage msgIn = (ErrorMessage) omsgIn;

            if (msgIn.UniqueID != null)
            {
                _controller.DisposeOfGazeData(msgIn.UniqueID);
            }
        }
    }
}
