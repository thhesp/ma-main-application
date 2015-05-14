using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.Base
{
    class BasicRawData
    {
        protected String _dataRequestedTimestamp;

        protected String _serverSentTimestamp;
        protected String _serverReceivedTimestamp;

        protected String _clientSentTimestamp;
        protected String _clientReceivedTimestamp;


        public BasicRawData()
        {

        }

        #region GetterSetterFunctions

        public String DataRequestedTimestamp
        {
            get { return _dataRequestedTimestamp; }
            set { _dataRequestedTimestamp = value; }
        }

        public String ServerSentTimestamp
        {
            get { return _serverSentTimestamp; }
            set { _serverSentTimestamp = value; }
        }

        public String ServerReceivedTimestamp
        {
            get { return _serverReceivedTimestamp; }
            set { _serverReceivedTimestamp = value; }
        }

        public String ClientSentTimestamp
        {
            get { return _clientSentTimestamp; }
            set { _clientSentTimestamp = value; }
        }

        public String ClientReceivedTimestamp
        {
            get { return _clientReceivedTimestamp; }
            set { _clientReceivedTimestamp = value; }
        }

        #endregion

    }
}
