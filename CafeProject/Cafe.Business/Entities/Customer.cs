using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business
{
    public class Customer
    {
        private string _firstName;
        private string _lastName;
        private string _emailAddress;
        private string _address;
        private string _mobileNumber;
        private string _telNumber;

        public virtual string Name
        {
            get
            {
                if (!String.IsNullOrEmpty(_lastName.Trim()) && !String.IsNullOrEmpty(_firstName.Trim()))
                    return _firstName + " " + _lastName;
                else
                    return String.Empty;
            }
        }

        public virtual string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public virtual string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public virtual string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        public virtual string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public virtual string MobileNumber
        {
            get { return _mobileNumber; }
            set { _mobileNumber = value; }
        }

        public virtual string TelNumber
        {
            get { return _telNumber; }
            set { _telNumber = value; }
        }

        public virtual int Id { get; set; }
    }
}
