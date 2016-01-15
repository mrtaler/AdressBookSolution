using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBookLibrary.Model
{
    public class Repository : IRepository<Contact>
    {
        #region Private members

        /// <summary>
        /// repository
        /// </summary>
        private IRepository<Contact> _repository;

        #endregion //Private members

        #region Constructors

        public Repository(IRepository<Contact> repository)
        {
            _repository = repository;
        }

        #endregion//Constructors

        #region Properties

        /// <summary>
        /// Records of repository
        /// </summary>
        public ICollection<Contact> All
        {
            get
            {
                return new List<Contact>(_repository.All);
            }
        }

        /// <summary>
        /// Name of repositry
        /// </summary>
        public string Name
        {
            get
            {
                return _repository.Name;
            }
            set
            {
                _repository.Name = value;
            }
        }

        /// <summary>
        /// Tupe of repository
        /// </summary>
        public RepositoryType Type
        {
            get
            {
                return _repository.Type;
            }
        }

        #endregion //Properties

        #region Public mhetods

        /// <summary>
        /// Add recors for repository
        /// </summary>
        public void Add(Contact value)
        {
            _repository.Add(value);
        }

        /// <summary>
        /// Remove record from repository
        /// </summary>
        public bool Remove(Contact value)
        {
            return _repository.Remove(value);
        }

        /// <summary>
        /// Save changes in repository
        /// </summary>
        public bool Save()
        {
            return _repository.Save();
        }

        /// <summary>
        /// Update repository
        /// </summary>
        public void Update()
        {
            _repository.Update();
        }

        #endregion
    }
}
