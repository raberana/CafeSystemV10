using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Cafe.Business;

namespace Cafe.Helpers
{
    public static class MainHelper
    {
        public static new string[] EncrytPassword(string newPassword)
        {   
            ICryptoService cryptoService = new PasswordPBKDF2();
            string encryptedPassword = cryptoService.Compute(newPassword);
            string passwordSalt = cryptoService.Salt;
            string[] result = {encryptedPassword, passwordSalt};
            return result;
        }

        public static bool ValidatePassword(IUser user, string password)
        {
            ICryptoService cryptoService = new PasswordPBKDF2();
            string hashed = cryptoService.Compute(password, user.PasswordSalt);
            return hashed == password;
        }

    }
}
