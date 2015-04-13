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

                String timestamp = Timestamp.GetUnixTimestamp();

                PositionDataModel posModel = new PositionDataModel(x,y,timestamp);

                if (msgIn.id)
                {
                    posModel.ID = msgIn.id;
                }
                
                if(msgIn.tag)
                {
                    posModel.Tag = msgIn.tag;
                }
                
                if(msgIn.title)
                {
                    posModel.Title = msgIn.title;
                }

                if (msgIn.classes)
                {
                    foreach (String className in msgIn.classes)
                    {
                        posModel.AddClass(className);
                    }
                }

                if(msgIn.attributes)
                {
                    foreach(dynamic attr in msgIn.attributes)
                    {
                        String name = attr.name;
                        String value = attr.value;

                        posModel.AddAttribute(name, value);
                    }
                }

                ExperimentController.getInstance().AddPositionData("stackoverflow.com", posModel);
            }
        }
    }
}
