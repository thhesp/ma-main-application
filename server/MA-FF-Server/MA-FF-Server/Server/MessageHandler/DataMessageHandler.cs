using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;
using WebAnalyzer.Experiment;

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
            Logger.Log("Data Message: X: " + msgIn.x + " Y: " + msgIn.y);
            Logger.Log("Data Message attributes: " + msgIn.data);

            if(msgIn.x != null && msgIn.y != null)
            {
                int x = msgIn.x;
                int y = msgIn.y;
                ExperimentController.getInstance().AddPositionData("stackoverflow.com", x, y);
            }
        }
    }
}
