using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.Base
{
    public class UIDBase
    {

        private String _uid;

        public UIDBase()
        {
            _uid = Guid.NewGuid().ToString();
        }

        public String UID
        {
            get { return _uid; }
            set { _uid = value; }
        }
    }
}
