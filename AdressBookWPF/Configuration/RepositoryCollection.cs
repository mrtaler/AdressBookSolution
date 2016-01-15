using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AdressBookWPF.Configuration
{
    [ConfigurationCollection(typeof(RepositoryElement))]
    public class RepositoryCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RepositoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RepositoryElement)(element)).Path;
        }

        public RepositoryElement this[int idx]
        {
            get { return (RepositoryElement)BaseGet(idx); }
        }
    }
}
