using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;

using WebAnalyzer.Events;
using WebAnalyzer.Models.MessageModel;

namespace WebAnalyzer.Server.MessageHandler
{
    class ActivationMessageHandler : IObserver<Object>
    {
        readonly WebsocketConnection _connection;

        public ActivationMessageHandler(WebsocketConnection connection)
        {
            _connection = connection;
        }

        public void OnCompleted()
        {
            Console.WriteLine("Activationmessage: Completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("Activaionnmessage error: " + error.Message);
        }

        public void OnNext(Object omsgIn)
        {
            ActivationMessage msgIn = (ActivationMessage)omsgIn;

            Logger.Log("ActivationMessage received: " + msgIn.Type);

            if (msgIn.Type == ActivationMessage.ACTIVATION_MESSAGE_TYPE.ACTIVATE)
            {
                _connection.Active = true;
            }
            else if (msgIn.Type == ActivationMessage.ACTIVATION_MESSAGE_TYPE.DEACTIVATE)
            {
                _connection.Active = false;
            }

        }
    }
}
