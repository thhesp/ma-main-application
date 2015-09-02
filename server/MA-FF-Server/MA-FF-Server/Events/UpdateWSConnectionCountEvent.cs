using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    public delegate void UpdateWSConnectionCountEventHandler(object sender, UpdateWSConnectionCountEvent e);

    public class UpdateWSConnectionCountEvent : EventArgs
    {
        private int _count;

        public UpdateWSConnectionCountEvent(int count)
        {
            _count = count;
        }

        public int Count
        {
            get { return _count; }
        }
    }
}
