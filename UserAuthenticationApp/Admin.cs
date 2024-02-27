using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationApp
{
    public class Admin : User
    {
        public List<User> Users { get; set; } = new List<User>();
    }
}
