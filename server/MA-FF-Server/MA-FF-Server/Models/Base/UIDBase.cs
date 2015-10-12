using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.Base
{
    /// <summary>
    /// Abstract base which creates a uid 
    /// </summary>
    public abstract class UIDBase
    {

        /// <summary>
        /// UID for this object
        /// </summary>
        private String _uid;

        /// <summary>
        /// Constructor which creates the uid for this object
        /// </summary>
        public UIDBase()
        {
            _uid = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Getter/ Setter for the uid
        /// </summary>
        public String UID
        {
            get { return _uid; }
            set { _uid = value; }
        }
    }
}
