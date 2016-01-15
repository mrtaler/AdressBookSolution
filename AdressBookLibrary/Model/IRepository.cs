using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBookLibrary.Model
{
    public interface IRepository<TValue>
    {
        /// <summary>
        /// Add record
        /// </summary>
        /// <param name="value">record</param>
        void Add(TValue value);

        /// <summary>
        /// Remove record
        /// </summary>
        /// <param name="value">record</param>
        /// <returns>true if record to right</returns>
        bool Remove(TValue value);

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns>true if save changes to right</returns>
        bool Save();

        /// <summary>
        /// Cancel changes
        /// </summary>
        void Update();

        /// <summary>
        /// All records
        /// </summary>
        ICollection<TValue> All { get;}

        /// <summary>
        /// Name of repository
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Type of repository
        /// </summary>
        RepositoryType Type { get;}
    }
}
