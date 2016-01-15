using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AdressBookWPF.Configuration
{
    public class Repositories : ConfigurationSection
    {
        [ConfigurationProperty("Repositories")]
        public RepositoryCollection RepositoriesItems
        {
            get { return ((RepositoryCollection)(base["Repositories"])); }
        }
    }
}
