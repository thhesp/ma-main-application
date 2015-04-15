using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;
using WebAnalyzer.Experiment;
using WebAnalyzer.DataModel;

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

                String timestamp = Timestamp.GetMillisecondsUnixTimestamp();

                PositionDataModel posModel = new PositionDataModel(x,y,timestamp);

                String id = msgIn.id;

                if (id != null)
                {
                    posModel.ID = id;
                }

                String tag = msgIn.tag;
                
                if(tag != null)
                {
                    posModel.Tag = tag;
                }

                String title = msgIn.title;

                if(title != null)
                {
                    posModel.Title = title;
                }

                foreach (String className in msgIn.classes)
                {
                    posModel.AddClass(className);
                }

                foreach (dynamic attr in msgIn.attributes)
                {
                    String name = attr.name;
                    String value = attr.value;

                    if (name != null && value != null)
                    {
                        posModel.AddAttribute(name, value);
                    }
               }

                String serverTimestamp = msgIn.serversent;

                if(serverTimestamp != null)
                {
                    posModel.ServerSentTimestamp = serverTimestamp;
                }

                String clientSentTimestamp = msgIn.clientsent;

                if (clientSentTimestamp != null)
                {
                    posModel.ClientSentTimestamp = clientSentTimestamp;
                }


                String clientReceivedTimestamp = msgIn.clientreceived;

                if (clientReceivedTimestamp != null)
                {
                    posModel.ClientReceivedTimestamp = clientReceivedTimestamp;
                }
                
                String url = msgIn.url;

                if (url != null)
                {
                    ExperimentController.getInstance().AddPositionData(url, posModel);
                }
            }
        }
    }
}
