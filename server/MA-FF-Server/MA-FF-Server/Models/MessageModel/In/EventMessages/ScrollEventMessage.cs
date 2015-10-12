using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel.In.EventMessages
{
    /// <summary>
    /// Model for the scroll event message
    /// </summary>
    public class ScrollEventMessage : EventMessage
    {

        /// <summary>
        /// Scroll X
        /// </summary>
        private int _scrollX = 0;

        /// <summary>
        /// Scroll Y
        /// </summary>
        private int _scrollY = 0;

        /// <summary>
        /// Getter/ Setter for Scroll X
        /// </summary>
        public int ScrollX
        {
            get { return _scrollX; }
            set { _scrollX = value; }
        }

        /// <summary>
        /// Getter/ Setter for Scroll Y
        /// </summary>
        public int ScrollY
        {
            get { return _scrollY; }
            set { _scrollY = value; }
        }
    }
}
