using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.Base;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class ExperimentObject : BaseInteractionObject
    {

        public enum CONNECTION_STATUS { connected = 0, disconnected = 1, warning = 2};

        private ExperimentModel _exp;

        private CONNECTION_STATUS _wsConnection = CONNECTION_STATUS.disconnected;

        private CONNECTION_STATUS _trackingConnection = CONNECTION_STATUS.disconnected;

        private int _connectionCount = 0;


        public ExperimentObject(){
        }

        public ExperimentModel Experiment
        {
            get { return _exp; }
            set { _exp = value; }
        }

        public String getName(){
            if(_exp != null)
                return _exp.ExperimentName;

            return null;
        }

        public String[] participantArray()
        {
            if (_exp != null)
                return _exp.GetParticipantArray();

            return null;
        }

        public String[] participantUIDs()
        {
            if (_exp != null)
                return _exp.GetParticipantUIDs();

            return null;
        }

        public String[] domainSettingsArray()
        {
            if (_exp != null)
                return _exp.GetDomainSettingArray();

            return null;
        }

        public String[] domainSettingUIDs()
        {
            if (_exp != null)
                return _exp.GetDomainSettingUIDs();

            return null;
        }

        public String getWSConnectionStatus()
        {
            return _wsConnection.ToString();
        }


        public String getTrackingConnectionStatus()
        {
            return _trackingConnection.ToString();
        }

        public void SetWSConnectionStatus(CONNECTION_STATUS status)
        {
            _wsConnection = status;
        }

        public void SetTrackingConnectionStatus(CONNECTION_STATUS status)
        {
            _trackingConnection = status;
        }

        public void SetWSConnectionCount(int count)
        {
            _connectionCount = count;
        }

        public int getConnectionCount()
        {
            return _connectionCount;
        }

        public void RefreshData()
        {
            EvaluteJavaScript("initialize();");
        }
    }
}
