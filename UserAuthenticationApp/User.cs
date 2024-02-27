using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationApp
{
    public class User
    {

        DataHandler handler = new DataHandler();

        public string Username { get; set; }
        public string PasswordHash { get; set; }

        // Metoda pro zkontrolování hesla
        public bool CheckPassword(string password)
        {
            return handler.GetPasswordHash(password) == PasswordHash;
        }
    }
}
