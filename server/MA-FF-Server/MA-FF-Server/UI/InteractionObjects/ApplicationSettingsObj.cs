using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WebAnalyzer.Events;
using WebAnalyzer.Models.Base;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class ApplicationSettingsObj : BaseInteractionObject
    {
        private EditApplicationSettings _form;

        private String _trackingModelType;

        private int _wsPort;

        private Boolean _ETConnectLocal;
        private String _ETSentIP;
        private int _ETSentPort;
        private String _ETReceiveIP;
        private int _ETReceivePort;


        public ApplicationSettingsObj(EditApplicationSettings form)
        {
            _form = form;

            _trackingModelType = Properties.Settings.Default.TrackingModelType;

            _wsPort = Properties.Settings.Default.WebsocketPort;

            _ETConnectLocal = Properties.Settings.Default.ETConnectLocal;

            _ETSentIP = Properties.Settings.Default.ETSentIP;

            _ETSentPort = Properties.Settings.Default.ETSentPort;

            _ETReceiveIP = Properties.Settings.Default.ETReceiveIP;

            _ETReceivePort = Properties.Settings.Default.ETReceivePort;
        }

        public void saveSettings()
        {
            SaveData();

            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.OK;
            });
        }

        private void SaveData()
        {
            Properties.Settings.Default.TrackingModelType = _trackingModelType;

            Properties.Settings.Default.WebsocketPort = _wsPort;

            Properties.Settings.Default.ETConnectLocal = _ETConnectLocal;

            Properties.Settings.Default.ETSentIP = _ETSentIP;

            Properties.Settings.Default.ETSentPort = _ETSentPort;

            Properties.Settings.Default.ETReceiveIP = _ETReceiveIP;

            Properties.Settings.Default.ETReceivePort = _ETReceivePort;

            Properties.Settings.Default.Save();
        }

        public void cancel()
        {
            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.Abort;
            });
        }

        public void setTrackingModelType(String trackingModelType)
        {
            _trackingModelType = trackingModelType;
        }

        public String getTrackingModelType()
        {
            return _trackingModelType;
        }

        public void setETConnectLocal(Boolean ETConnectLocal)
        {
            _ETConnectLocal = ETConnectLocal;
        }

        public Boolean getETConnectLocal()
        {
            return _ETConnectLocal;
        }

        public void setWSPort(int wsPort)
        {
            _wsPort = wsPort;
        }

        public int getWSPort()
        {
            return _wsPort;
        }

        public void setETSentIP(String sentIP)
        {
            _ETSentIP = sentIP;
        }

        public String getETSentIP()
        {
            return _ETSentIP;
        }

        public void setETSentPort(int sentPort)
        {
            _ETSentPort = sentPort;
        }

        public int getETSentPort()
        {
            return _ETSentPort;
        }

        public void setETReceiveIP(String receiveIP)
        {
            _ETReceiveIP = receiveIP;
        }

        public String getETReceiveIP()
        {
            return _ETReceiveIP;
        }

        public void setETReceivePort(int receivePort)
        {
            _ETReceivePort = receivePort;
        }

        public int getETReceivePort()
        {
            return _ETReceivePort;
        }
    }
}
