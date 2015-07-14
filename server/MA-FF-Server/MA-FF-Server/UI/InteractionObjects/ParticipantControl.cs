﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WebAnalyzer.Events;
using WebAnalyzer.Models.Base;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class ParticipantControl : BaseInteractionObject
    {

        public event CreateParticipantEventHandler CreateParticipant;

        private EditParticipantForm _form;

        private ExperimentParticipant _participant;

        private Boolean _create;

        public ParticipantControl(EditParticipantForm form, ExperimentParticipant par, Boolean create)
        {
            _form = form;
            _participant = par;
            _create = create;
        }

        public Boolean creatingNewParticipant()
        {
            return _create;
        }

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

        public void cancel()
        {
            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.Abort;
            });
        }

        public void setIdentifier(String identifier)
        {
            _participant.Identifier = identifier;
        }

        public String getIdentifier()
        {
            return _participant.Identifier;
        }

        public void setBirthyear(int birthyear)
        {
            _participant.BirthYear = birthyear;
        }

        public int getBirthyear()
        {
            return _participant.BirthYear;
        }

        public void setEducation(String education)
        {
            _participant.Education = education;
        }

        public String getEducation()
        {
            return _participant.Education;
        }

        public String getSex()
        {
            switch (_participant.Sex)
            {
                case ExperimentParticipant.SEX_TYPES.Male:
                    return "male";
                case ExperimentParticipant.SEX_TYPES.Female:
                    return "female";
                case ExperimentParticipant.SEX_TYPES.Undecided:
                    return "undecided";
            }

            return "";
        }

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

        public String[] getExtraDataKeys()
        {
            return _participant.ExtraData.Keys.ToArray();
        }

        public String[] getExtraDataValues()
        {
            return _participant.ExtraData.Values.ToArray();
        }

        public void addExtraData(String key, String value)
        {
            if(key != null & value != null)
                _participant.AddExtraData(key, value);
        }
    }
}
