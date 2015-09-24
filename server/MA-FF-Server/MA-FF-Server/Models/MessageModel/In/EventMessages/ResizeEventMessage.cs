using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel.In.EventMessages
{
    public class ResizeEventMessage : EventMessage
    {
        private int _windowWidth = 0;
        private int _windowHeight = 0;

        public int WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }

        public int WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }
    }
}
