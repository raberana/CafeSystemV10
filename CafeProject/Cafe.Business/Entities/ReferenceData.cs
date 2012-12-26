using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business.Entities
{
    public class ReferenceData
    {
        private string _name;
        private string _code;
        private string _value1;
        private string _value2;
        private string _value3;
        private string _value4;
        private string _value5;
        private string _value6;
        private DateTime _startDate;
        private DateTime _endDate;

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public virtual string Value1
        {
            get { return _value1; }
            set { _value1 = value; }
        }

        public virtual string Value2
        {
            get { return _value2; }
            set { _value2 = value; }
        }
        public virtual string Value3
        {
            get { return _value3; }
            set { _value3 = value; }
        }

        public virtual string Value4
        {
            get { return _value4; }
            set { _value4 = value; }
        }

        public virtual string Value5
        {
            get { return _value5; }
            set { _value5 = value; }
        }

        public virtual string Value6
        {
            get { return _value6; }
            set { _value6 = value; }
        }

        public virtual DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public virtual DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }
    }
}
