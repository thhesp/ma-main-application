using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebAnalyzer.Models.Base
{
    /// <summary>
    /// Abstract class for the basic data
    /// </summary>
    public abstract class BasicData
    {
        /// <summary>
        /// Timestamp on which data was requested
        /// </summary>
        protected String _dataRequestedTimestamp;

        /// <summary>
        /// Timestamp when data was sent by the server
        /// </summary>
        protected String _serverSentTimestamp;

        /// <summary>
        /// Timestamp when data was received by the server
        /// </summary>
        protected String _serverReceivedTimestamp;

        /// <summary>
        /// Timestamp when data was sent by the client
        /// </summary>
        protected String _clientSentTimestamp;

        /// <summary>
        /// Timestamp when data was received by the client
        /// </summary>
        protected String _clientReceivedTimestamp;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public BasicData()
        {

        }

        /// <summary>
        /// Getter/Setter for the data requested timestamp
        /// </summary>
        public String DataRequestedTimestamp
        {
            get { return _dataRequestedTimestamp; }
            set { _dataRequestedTimestamp = value; }
        }

        /// <summary>
        /// Getter/Setter for the server sent timestamp
        /// </summary>
        public String ServerSentTimestamp
        {
            get { return _serverSentTimestamp; }
            set { _serverSentTimestamp = value; }
        }

        /// <summary>
        /// Getter/Setter for the server received timestamp
        /// </summary>
        public String ServerReceivedTimestamp
        {
            get { return _serverReceivedTimestamp; }
            set { _serverReceivedTimestamp = value; }
        }

        /// <summary>
        /// Getter/Setter for the client sent timestamp
        /// </summary>
        public String ClientSentTimestamp
        {
            get { return _clientSentTimestamp; }
            set { _clientSentTimestamp = value; }
        }

        /// <summary>
        /// Getter/Setter for the client received timestamp
        /// </summary>
        public String ClientReceivedTimestamp
        {
            get { return _clientReceivedTimestamp; }
            set { _clientReceivedTimestamp = value; }
        }

        /// <summary>
        /// Calculates the duration between requesting and sending on the server
        /// </summary>
        /// <returns>Duration</returns>
        public long DurationFromRequestTillSending()
        {
            if (_serverSentTimestamp != null && _dataRequestedTimestamp != null)
            {
                return long.Parse(_serverSentTimestamp) - long.Parse(_dataRequestedTimestamp);
            }

            return 0;
        }

        /// <summary>
        /// Calculates the duration between sending and receiving the answer
        /// </summary>
        /// <returns>Duration</returns>
        public long DurationFromServerSentToReceived()
        {
            if (_serverReceivedTimestamp != null && _serverSentTimestamp != null)
            {
                return long.Parse(_serverReceivedTimestamp) - long.Parse(_serverSentTimestamp);
            }

            return 0;
        }

        /// <summary>
        /// Calculates the duration between receiving and sending the answer on the client
        /// </summary>
        /// <returns>Duration</returns>
        public long DurationFromClientReceivedToClientSent()
        {
            if (_clientReceivedTimestamp != null && _clientSentTimestamp != null)
            {
                return long.Parse(_clientSentTimestamp) - long.Parse(_clientReceivedTimestamp);
            }

            return 0;
        }

    }

}
