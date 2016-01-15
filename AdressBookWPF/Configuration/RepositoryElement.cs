using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AdressBookWPF.Configuration
{
    public class RepositoryElement : ConfigurationElement
    {
        [ConfigurationProperty("TypeRepository", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string TypeRepository
        {
            get { return (string)(base["TypeRepository"]); }
            set { base["TypeRepository"] = value; }
        }

        [ConfigurationProperty("Path", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Path
        {
            get { return (string)(base["Path"]); }
            set { base["Path"] = value; }
        }

        [ConfigurationProperty("Name", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Name
        {
            get { return (string)(base["Name"]); }
            set { base["Name"] = value; }
        }

        [ConfigurationProperty("DTD", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string DTD
        {
            get { return (string)(base["DTD"]); }
            set { base["DTD"] = value; }
        }

        [ConfigurationProperty("XSD", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string XSD
        {
            get { return (string)(base["XSD"]); }
            set { base["XSD"] = value; }
        }
    }
}
