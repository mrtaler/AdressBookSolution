using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AdressBookWPF.Configuration
{
    public class Users : ConfigurationSection
    {
        [ConfigurationProperty("Users")]
        public UserCollection UserItems
        {
            get { return ((UserCollection)(base["Users"])); }
        }
    }
}
