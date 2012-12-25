using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business
{
    public class Employee:IUser
    {
        private string _firstName;
        private string _lastName;
        private Branch _branch;

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
