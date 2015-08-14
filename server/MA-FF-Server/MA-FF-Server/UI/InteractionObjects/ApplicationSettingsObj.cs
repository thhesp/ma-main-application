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

        private Boolean _useMouseTracking;

        private String _wsPort;

        private Boolean _ETConnectLocal;
        private String _ETSentIP;
        private String _ETSentPort;
        private String _ETReceiveIP;
        private String _ETReceivePort;


        public ApplicationSettingsObj(EditApplicationSettings form)
        {
            _form = form;

            _useMouseTracking = Boolean.Parse(Properties.Settings.Default.UseMouseTracking);

            _wsPort = Properties.Settings.Default.WebsocketPort;

            _ETConnectLocal = Boolean.Parse(Properties.Settings.Default.ETConnectLocal);

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
            Properties.Settings.Default.UseMouseTracking = _useMouseTracking.ToString();

            Properties.Settings.Default.WebsocketPort = _wsPort;

            Properties.Settings.Default.ETConnectLocal = _ETConnectLocal.ToString();

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

        public void setUseMouseTracking(Boolean useMouseTracking)
        {
            _useMouseTracking = useMouseTracking;
        }

        public Boolean getUseMouseTracking()
        {
            return _useMouseTracking;
        }

        public void setETConnectLocal(Boolean ETConnectLocal)
        {
            _ETConnectLocal = ETConnectLocal;
        }

        public Boolean getETConnectLocal()
        {
            return _ETConnectLocal;
        }

        public void setWSPort(String wsPort)
        {
            _wsPort = wsPort;
        }

        public String getWSPort()
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

        public void setETSentPort(String sentPort)
        {
            _ETSentPort = sentPort;
        }

        public String getETSentPort()
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

        public void setETReceivePort(String receivePort)
        {
            _ETReceivePort = receivePort;
        }

        public String getETReceivePort()
        {
            return _ETReceivePort;
        }
    }
}
