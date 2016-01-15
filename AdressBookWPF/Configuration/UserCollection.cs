using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AdressBookWPF.Configuration
{
    [ConfigurationCollection(typeof(UserElement))]
    public class UserCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UserElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UserElement)(element)).Login;
        }

        public UserElement this[int idx]
        {
            get { return (UserElement)BaseGet(idx); }
        }
    }
}
