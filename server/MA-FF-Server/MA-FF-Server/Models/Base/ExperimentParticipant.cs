using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.Base
{
    public class ExperimentParticipant
    {

        public static enum SEX_TYPES { Male, Female, Undecided };

        private String _identifier;

        /* demographic data */
        private SEX_TYPES _sex;
        private int _birthYear;
        private String _education;

        /* extra data */
        private Dictionary<String, String> _variableData = new Dictionary<String, String>();

        /* experiment data */
        private List<WebpageModel> _visitedPages = new List<WebpageModel>();

        public String Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public SEX_TYPES Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }

        public int BirthYear
        {
            get { return _birthYear; }
            set { _birthYear = value; }
        }

        public String Education
        {
            get { return _education; }
            set { _education = value; }
        }

        public void AddExtraData(String key, String value)
        {
            _variableData.Add(key, value);
        }

        public Dictionary<String, String> GetExtraData()
        {
            return _variableData;
        }



    }
}
