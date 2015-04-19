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
            Logger.Log("Data Message: X: " + msgIn.x + " Y: " + msgIn.y);
            Logger.Log("Data Message attributes: " + msgIn.data);

            if(msgIn.x != null && msgIn.y != null)
            {

                PositionDataModel posModel = extractPositionData(msgIn);
                
                String url = msgIn.url;

                if (url != null)
                {
                    ExperimentController.getInstance().AddPositionData(url, posModel);
                }
            }
        }

        private PositionDataModel extractPositionData(dynamic msgIn)
        {
            int x = msgIn.x;
            int y = msgIn.y;

            String timestamp = Timestamp.GetMillisecondsUnixTimestamp();

            PositionDataModel posModel = new PositionDataModel(x, y, timestamp);

            String serverTimestamp = msgIn.serversent;

            if (serverTimestamp != null)
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

            // get element data 

            posModel.Element = extractElementData(msgIn);

            return posModel;
        }

        private DOMElementModel extractElementData(dynamic msgIn)
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
