using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel.In.EventMessages
{
    public class ScrollEventMessage : EventMessage
    {

        private int _scrollX = 0;
        private int _scrollY = 0;

        public int ScrollX
        {
            get { return _scrollX; }
            set { _scrollX = value; }
        }

        public int ScrollY
        {
            get { return _scrollY; }
            set { _scrollY = value; }
        }
    }
}
