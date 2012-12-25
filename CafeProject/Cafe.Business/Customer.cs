using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business
{
    public class Customer : IUser 
    {
        private string _firstName;
        private string _lastName;
        private string _fullName;
        private string _emailAddress;
        private string _address;
        private string _mobileNumber;
        private string _telNumber;

        public string Name
        {
            get { return _firstName + " " + _lastName; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }


        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string PasswordSalt
        {
            get;
            set;
        }
    }
}
