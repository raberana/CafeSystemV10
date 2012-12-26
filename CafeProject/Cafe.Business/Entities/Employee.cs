using Cafe.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business
{
    public class Employee : IUser
    {
        private string _firstName;
        private string _lastName;
        private Branch _branch;
        private EmployeeRole _role;
        private string _employeeId;

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

        public virtual Branch Branch
        {
            get { return _branch; }
            set { _branch = value; }
        }

        public virtual EmployeeRole Role
        {
            get { return _role; }
            set { _role = value; }
        }

        public string EmployeeId
        {
            get { return _employeeId; }
            set { _employeeId = value; }
        }

        public virtual string Username
        {
            get;
            set;
        }

        public virtual string Password
        {
            get;
            set;
        }

        public virtual string PasswordSalt
        {
            get;
            set;
        }


    }
}
