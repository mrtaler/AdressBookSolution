using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBookLibrary.Model
{
    public class Account
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Roles Roles { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="login">Login of user.</param>
        /// <param name="password">Password of user.</param>
        /// <param name="roles">Admin or User roles.</param>
        public Account(string login, string password, Roles roles)
        {
            Login = login;
            Password = password;
            Roles = roles;
        }

        public Account()
        {
            Login = "";
            Password = "";
            Roles = Roles.User;
        }
    }
}
