using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.Base
{
    public class BasicData
    {
        protected String _dataRequestedTimestamp;

        protected String _serverSentTimestamp;
        protected String _serverReceivedTimestamp;

        protected String _clientSentTimestamp;
        protected String _clientReceivedTimestamp;


        public BasicData()
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


        
        #region StatisticsFunctions

        public long DurationFromRequestTillSending()
        {
            if (_serverSentTimestamp != null && _dataRequestedTimestamp != null)
            {
                return long.Parse(_serverSentTimestamp) - long.Parse(_dataRequestedTimestamp);
            }

            return 0;
        }

        public long DurationFromServerSentToReceived()
        {
            if (_serverReceivedTimestamp != null && _serverSentTimestamp != null)
            {
                return long.Parse(_serverReceivedTimestamp) - long.Parse(_serverSentTimestamp);
            }

            return 0;
        }

        public long DurationFromClientReceivedToClientSent()
        {
            if (_clientReceivedTimestamp != null && _clientSentTimestamp != null)
            {
                return long.Parse(_clientSentTimestamp) - long.Parse(_clientReceivedTimestamp);
            }

            return 0;
        }

        #endregion

    }

}
