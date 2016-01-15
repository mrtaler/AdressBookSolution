using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AdressBookWPF.Configuration
{
    public class UserElement : ConfigurationElement
    {
        [ConfigurationProperty("Login", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Login
        {
            get { return (string)(base["Login"]); }
            set { base["Login"] = value; }
        }

        [ConfigurationProperty("Password", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Password
        {
            get { return (string)(base["Password"]); }
            set { base["Password"] = value; }
        }

        [ConfigurationProperty("Roles", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Roles
        {
            get { return (string)(base["Roles"]); }
            set { base["Roles"] = value; }
        }
    }
}
