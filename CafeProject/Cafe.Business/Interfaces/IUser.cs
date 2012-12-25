using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business
{
    public interface IUser
    {
        string Username { get; set; }
        string Password { get; set; }
        string PasswordSalt { get; set; }
    }
}
