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
    /// Interaction object for creating/ editing a participant
    /// </summary>
    public class ParticipantControl : BaseInteractionObject
    {

        /// <summary>
        /// Eventhandler for creating a participant
        /// </summary>
        public event CreateParticipantEventHandler CreateParticipant;

        /// <summary>
        /// Reference to the edit participant form
        /// </summary>
        private EditParticipantForm _form;

        /// <summary>
        /// Reference to the participant
        /// </summary>
        private ExperimentParticipant _participant;

        /// <summary>
        /// Flag if it is a new participant or not
        /// </summary>
        private Boolean _create;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">Reference to the edit participant form</param>
        /// <param name="par">Reference to the participant</param>
        /// <param name="create">Flag if its a new participant or not</param>
        public ParticipantControl(EditParticipantForm form, ExperimentParticipant par, Boolean create)
        {
            _form = form;
            _participant = par;
            _create = create;
        }

        /// <summary>
        /// Returns if it is a new participant which is being created
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public Boolean creatingNewParticipant()
        {
            return _create;
        }

        /// <summary>
        /// Used for saving the participant
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void saveParticipant()
        {
            if (_create)
            {
                CreateParticipant(this, new CreateParticipantEvent(_participant));
            }

            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.OK;
            });
        }

        /// <summary>
        /// Used for canceling the edit/creation process
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void cancel()
        {
            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.Abort;
            });
        }

        /// <summary>
        /// Used for setting the identifier
        /// </summary>
        /// <param name="identifier">New identifier</param>
        /// <remarks>Called from javascript</remarks>
        public void setIdentifier(String identifier)
        {
            _participant.Identifier = identifier;
        }

        /// <summary>
        /// Returns the identifier of the participant
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String getIdentifier()
        {
            return _participant.Identifier;
        }

        /// <summary>
        /// Used for setting the birthyear
        /// </summary>
        /// <param name="birthyear">New birthyear</param>
        /// <remarks>Called from javascript</remarks>
        public void setBirthyear(int birthyear)
        {
            _participant.BirthYear = birthyear;
        }

        /// <summary>
        /// Returns the birthyear of the participant
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public int getBirthyear()
        {
            return _participant.BirthYear;
        }

        /// <summary>
        /// Used for updating the education of the participant
        /// </summary>
        /// <param name="education">New education</param>
        /// <remarks>Called from javascript</remarks>
        public void setEducation(String education)
        {
            _participant.Education = education;
        }

        /// <summary>
        /// Returns the education of the participant
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String getEducation()
        {
            return _participant.Education;
        }

        /// <summary>
        /// Returns the sex of the participant
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String getSex()
        {
            return _participant.Sex.ToString().ToLower();
        }

        /// <summary>
        /// Used for updating the sex of the participant
        /// </summary>
        /// <param name="sex">New sex of the participant</param>
        /// <remarks>Called from javascript</remarks>
        public void setSex(String sex)
        {
            switch (sex)
            {
                case "male":
                    _participant.Sex = ExperimentParticipant.SEX_TYPES.Male;
                    break;
                case "female":
                    _participant.Sex = ExperimentParticipant.SEX_TYPES.Female;
                    break;
                case "undecided":
                    _participant.Sex = ExperimentParticipant.SEX_TYPES.Undecided;
                    break;
            }
        }

        /// <summary>
        /// Returns array of extra data keys
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String[] getExtraDataKeys()
        {
            return _participant.ExtraData.Keys.ToArray();
        }

        /// <summary>
        /// Returns array of extra data values
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String[] getExtraDataValues()
        {
            return _participant.ExtraData.Values.ToArray();
        }

        /// <summary>
        /// Used for updating the extra data
        /// </summary>
        /// <param name="key">The extra data key</param>
        /// <param name="value">the extra data value</param>
        public void addExtraData(String key, String value)
        {
            if(key != null & value != null)
                _participant.AddExtraData(key, value);
        }
    }
}
