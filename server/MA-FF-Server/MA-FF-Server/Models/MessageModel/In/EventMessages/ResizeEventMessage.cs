using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel.In.EventMessages
{
    /// <summary>
    /// Model for the resize event message
    /// </summary>
    public class ResizeEventMessage : EventMessage
    {
        /// <summary>
        /// window width
        /// </summary>
        private int _windowWidth = 0;

        /// <summary>
        /// window height
        /// </summary>
        private int _windowHeight = 0;


        /// <summary>
        /// Getter/ Setter for window width
        /// </summary>
        public int WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }

        /// <summary>
        /// Getter/ Setter for window height
        /// </summary>
        public int WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }
    }
}
