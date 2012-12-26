using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Business
{
    public interface IUser
    {
        string Name { get; }
        string Username { get; set; }
        string Password { get; set; }
        string PasswordSalt { get; set; }
    }
}
