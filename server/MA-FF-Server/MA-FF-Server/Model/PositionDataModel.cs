using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Model
{
    class PositionDataModel
    {
        private int _x;
        private int _y;
        private String _timestamp;

        private String _tag;
        private String _id;
        private String _title;

        private List<String> _classes = new List<String>();
        private List<AttributeModel> _attributes = new List<AttributeModel>();

        public PositionDataModel(int x, int y, String timestamp)
        {
            _x = x;
            _y = y;
            _timestamp = timestamp;
        }

        public int X
        {
            get { return _x; }
            set { _x = value;  }
        }


        public int Y
        {
            get { return _y; }
            set { _y = value;  }
        }

        public String Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }


        public String Tag
        {
            get { return _tag; }
            set { _tag = value;  }
        }


        public String ID
        {
            get { return _id; }
            set { _id = value;  }
        }

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public void AddClass(String className)
        {
            _classes.Add(className);
        }

        public void AddAttribute(String name, String value)
        {
            AttributeModel attrModel = new AttributeModel(name, value);

            _attributes.Add(attrModel);
        }
    }
}
