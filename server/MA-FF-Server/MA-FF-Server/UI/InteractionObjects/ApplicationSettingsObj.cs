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
    /// <summary>
    /// Interaction object for editing the application settings
    /// </summary>
    public class ApplicationSettingsObj : BaseInteractionObject
    {
        /// <summary>
        /// Reference to the edit application window
        /// </summary>
        private EditApplicationSettings _form;

        /// <summary>
        /// Current tracking model type
        /// </summary>
        private String _trackingModelType;

        /// <summary>
        /// current websocket port
        /// </summary>
        private int _wsPort;

        /// <summary>
        /// Flag if the connection to the eyetracker shall be local
        /// </summary>
        private Boolean _ETConnectLocal;

        /// <summary>
        /// Eyetracker sent ip
        /// </summary>
        private String _ETSentIP;

        /// <summary>
        /// Eyetracker sent port
        /// </summary>
        private int _ETSentPort;

        /// <summary>
        /// Eyetracker receive IP
        /// </summary>
        private String _ETReceiveIP;

        /// <summary>
        /// Eyetracker receive port
        /// </summary>
        private int _ETReceivePort;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">Reference to the edit application settings form</param>
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

        /// <summary>
        /// Used for saving the settings
        /// </summary>
        /// <remarks> Called from javascript</remarks>
        public void saveSettings()
        {
            SaveData();

            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.OK;
            });
        }

        /// <summary>
        /// Really saves the data from the object variables to the application settings
        /// </summary>
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

        /// <summary>
        /// Cancels the editing process
        /// </summary>
        /// <remarks> Called from javascript</remarks>
        public void cancel()
        {
            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.Abort;
            });
        }

        /// <summary>
        /// Used for updating the tracking model type
        /// </summary>
        /// <param name="trackingModelType">The new tracking model type</param>
        /// <remarks> Called from javascript</remarks>
        public void setTrackingModelType(String trackingModelType)
        {
            _trackingModelType = trackingModelType;
        }

        /// <summary>
        /// Returns the current tracking model type
        /// </summary>
        /// <returns></returns>
        /// <remarks> Called from javascript</remarks>
        public String getTrackingModelType()
        {
            return _trackingModelType;
        }

        /// <summary>
        /// Used for updating the et connect local
        /// </summary>
        /// <param name="ETConnectLocal">New Value</param>
        /// <remarks> Called from javascript</remarks>
        public void setETConnectLocal(Boolean ETConnectLocal)
        {
            _ETConnectLocal = ETConnectLocal;
        }

        /// <summary>
        /// Returns the et connect local
        /// </summary>
        /// <returns></returns>
        /// <remarks> Called from javascript</remarks>
        public Boolean getETConnectLocal()
        {
            return _ETConnectLocal;
        }

        /// <summary>
        /// Used for updating the websocket port
        /// </summary>
        /// <param name="wsPort">The new port</param>
        /// <remarks> Called from javascript</remarks>
        public void setWSPort(int wsPort)
        {
            _wsPort = wsPort;
        }

        /// <summary>
        /// Returns the websocket port
        /// </summary>
        /// <returns></returns>
        /// <remarks> Called from javascript</remarks>
        public int getWSPort()
        {
            return _wsPort;
        }

        /// <summary>
        /// Used for updating the eyetracking sent ip
        /// </summary>
        /// <param name="sentIP">new sent ip</param>
        /// <remarks> Called from javascript</remarks>
        public void setETSentIP(String sentIP)
        {
            _ETSentIP = sentIP;
        }

        /// <summary>
        /// Returns the eyetracking sent ip
        /// </summary>
        /// <returns></returns>
        /// <remarks> Called from javascript</remarks>
        public String getETSentIP()
        {
            return _ETSentIP;
        }

        /// <summary>
        /// Used for updating the eyetracking sent port
        /// </summary>
        /// <param name="sentPort">new sent port</param>
        /// <remarks> Called from javascript</remarks>
        public void setETSentPort(int sentPort)
        {
            _ETSentPort = sentPort;
        }

        /// <summary>
        /// Returns the eyetracking sent port
        /// </summary>
        /// <returns></returns>
        /// <remarks> Called from javascript</remarks>
        public int getETSentPort()
        {
            return _ETSentPort;
        }

        /// <summary>
        /// Used for updating the eyetracking receive ip
        /// </summary>
        /// <param name="receiveIP">new receive ip</param>
        /// <remarks> Called from javascript</remarks>
        public void setETReceiveIP(String receiveIP)
        {
            _ETReceiveIP = receiveIP;
        }

        /// <summary>
        /// Returns the eyetracking receive ip
        /// </summary>
        /// <returns></returns>
        /// <remarks> Called from javascript</remarks>
        public String getETReceiveIP()
        {
            return _ETReceiveIP;
        }

        /// <summary>
        /// Used for updating the eyetracking receive port
        /// </summary>
        /// <param name="receivePort">new receive port</param>
        /// <remarks> Called from javascript</remarks>
        public void setETReceivePort(int receivePort)
        {
            _ETReceivePort = receivePort;
        }

        /// <summary>
        /// Returns the eyetracking receive port
        /// </summary>
        /// <returns></returns>
        /// <remarks> Called from javascript</remarks>
        public int getETReceivePort()
        {
            return _ETReceivePort;
        }
    }
}
