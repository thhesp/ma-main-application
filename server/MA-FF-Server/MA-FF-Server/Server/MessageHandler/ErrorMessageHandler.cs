using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.MessageModel;
using WebAnalyzer.Controller;

namespace WebAnalyzer.Server.MessageHandler
{
    class ErrorMessageHandler : IObserver<Object>
    {
        readonly TestController _controller;
        readonly WebsocketConnection _connection;

        public ErrorMessageHandler(TestController controller, WebsocketConnection connection)
        {
            _controller = controller;
            _connection = connection;
        }

        public void OnCompleted()
        {
            Console.WriteLine("Error Message: Completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("Error message : " + error.Message);
        }

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
