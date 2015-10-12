using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using WebAnalyzer.Util;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Events;

namespace WebAnalyzer.UI.InteractionObjects
{
    /// <summary>
    /// Class for selecting a participant
    /// </summary>
    public class SelectParticipantControl : BaseInteractionObject
    {

        /// <summary>
        /// Eventhandler for selecting the participant
        /// </summary>
        public event SelectParticipantForTestEventHandler SelectParticipant;

        /// <summary>
        /// Reference to the select participant form
        /// </summary>
        private SelectParticipantForm _form;

        /// <summary>
        /// Reference to the currently loaded experiment
        /// </summary>
        private ExperimentModel _exp;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">The select participant form</param>
        public SelectParticipantControl(SelectParticipantForm form)
        {
            _form = form;
        }

        /// <summary>
        /// Getter / Setter for the currently loaded experiment
        /// </summary>
        public ExperimentModel Experiment
        {
            get { return _exp; }
            set { _exp = value; }
        }

        /// <summary>
        /// Method used for selecting the participant
        /// </summary>
        /// <param name="uid">UID of the participant</param>
        /// <remarks>
        /// Called from javascript
        /// </remarks>
        public void selectParticipant(String uid)
        {
            SelectParticipant(this, new SelectParticipantForTestEvent(uid));

            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.OK;
            });
        }

        /// <summary>
        /// Returns an array of participant identifiers
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Called from javascript
        /// </remarks>
        public String[] participantArray()
        {
            if (_exp != null)
                return _exp.GetParticipantArray();

            return null;
        }


        /// <summary>
        /// Returns an array of participant uids
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Called from javascript
        /// </remarks>
        public String[] participantUIDs()
        {
            if (_exp != null)
                return _exp.GetParticipantUIDs();

            return null;
        }
    }
}
