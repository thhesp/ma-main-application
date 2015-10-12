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
    /// <summary>
    /// MessageHandler for Activation and Deactivation Messages
    /// </summary>
    class ActivationMessageHandler : IObserver<Object>
    {
        /// <summary>
        /// Eventhandler for the AddWebpage Event
        /// </summary>
        public event AddWebpageEventHandler AddWebpage;

        /// <summary>
        /// Readonly reference to the websocket connection
        /// </summary>
        readonly WebsocketConnection _connection;

        /// <summary>
        /// Constructor with the websocket connection
        /// </summary>
        /// <param name="connection">The connection which received the message</param>
        public ActivationMessageHandler(WebsocketConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Method which gets called once the processing of the message is completed.
        /// </summary>
        public void OnCompleted()
        {
            Console.WriteLine("Activationmessage: Completed");
        }

        /// <summary>
        /// Method which gets called if an error occurs while processing the message.
        /// </summary>
        /// <param name="error">Errorexception which occured</param>
        public void OnError(Exception error)
        {
            Console.WriteLine("Activaionnmessage error: " + error.Message);
        }

        /// <summary>
        /// Method used for processing the message
        /// </summary>
        /// <param name="omsgIn">The Message which was received</param>
        public void OnNext(Object omsgIn)
        {
            ActivationMessage msgIn = (ActivationMessage)omsgIn;

            Logger.Log("ActivationMessage received: " + msgIn.Type);

            if (msgIn.Type == ActivationMessage.ACTIVATION_MESSAGE_TYPE.ACTIVATE)
            {
                AddWebpage(this, new AddWebpageEvent(msgIn.URL, msgIn.WindowWidth, msgIn.WindowHeight, _connection.UID));
                _connection.Active = true;
            }
            else if (msgIn.Type == ActivationMessage.ACTIVATION_MESSAGE_TYPE.DEACTIVATE)
            {
                _connection.Active = false;
            }

        }
    }
}
