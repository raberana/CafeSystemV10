using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business
{
    public class Branch
    {
        private string _name;
        private string _address;
        private string _code;

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public virtual string Code
        {
            get { return _code; }
            set { _code = value; }
        }
    }
}
