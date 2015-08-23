﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.UI.InteractionObjects;

namespace WebAnalyzer.Events
{
    public delegate void LoadExperimentEventHandler(ExperimentWizardObj sender, LoadExperimentEvent e);

    public class LoadExperimentEvent : EventArgs
    {

        private String _path;

        public LoadExperimentEvent(String path){
            _path = path;
        }

        public String Path
        {   
            get {return _path; }
        }
    }
}
