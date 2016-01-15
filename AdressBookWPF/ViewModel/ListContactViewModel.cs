using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using Microsoft.Win32;

using AdressBookLibrary.Model;
using AdressBookLibrary.Commands;
using AdressBookLibrary.Data;
using AdressBookWPF.Properties;
using AdressBookWPF.Configuration;

namespace AdressBookWPF.ViewModel
{
    class ListContactViewModel : ViewModelBase
    {

        #region Private members

        private IRepository<Contact> _repository;
        private List<IRepository<Contact>> _memoryRepositories = new List<IRepository<Contact>>();

        #endregion//Private members

        #region Constructors

        public ListContactViewModel()
        {
            Logger.InitLogger();
            _memoryRepositories.Add(new MemoryRepository() { Name = "My memory" });
            Logger.Log.DebugFormat("User {0} load memory repository", Settings.Default.Login);
            AddRecord = new Command(OvAddContact);
            DeleteRecord = new Command(OnDeleteContact);
            SingOut = new Command(OnSingOut);
            ShowRecord = new Command(OnShowRecord);
            OpenRepositoryManager = new Command(OnOpenRepositoryManager);
        }

        #endregion

        #region Properties

        /// <summary>
        /// selected record
        /// </summary>
        public Contact SelectedContact { get; set; }

        /// <summary>
        /// selected repository
        /// </summary>
        public IRepository<Contact> Repository
        {
            get
            {
                return _repository;
            }
            set
            {
                if (_repository != value)
                {
                    _repository = value;
                    OnPropertyChanged("All");
                }
            }
        }

        /// <summary>
        /// error validation
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// list of records
        /// </summary>
        public ObservableCollection<Contact> All
        {
            get
            {
                if (!ReferenceEquals(Repository, null))
                    return new ObservableCollection<Contact>(Repository.All);
                return null;
            }
        }

        #endregion //Properties

        #region Commands

        public Command AddRecord { get; set; }
        public Command DeleteRecord { get; set; }
        public Command SingOut { get; set; }
        public Command ShowRecord { get; set; }
        public Command OpenRepositoryManager { get; set; }

        #endregion

        #region Private Mhetods

        /// <summary>
        /// Add record
        /// </summary>
        private void OvAddContact(object param)
        {
            if (!ReferenceEquals(Repository, null))
            {
                Window window = new Information();
                window.DataContext = new InformationViewModel(null, this) { View = window };
                window.ShowDialog();
            }
        }

        /// <summary>
        /// Remove record
        /// </summary>
        private void OnDeleteContact(object param)
        {
            if (!ReferenceEquals(SelectedContact, null))
            {
                Repository.Remove(SelectedContact);
                Repository.Save();
                Logger.Log.DebugFormat("User {0} delete record {1}", Settings.Default.Login, SelectedContact.GetHashCode());
                SelectedContact = null;
                OnPropertyChanged("All");
            }
        }

        /// <summary>
        /// Sing out
        /// </summary>
        private void OnSingOut(object param)
        {
            Window window = new PasswordWindow();
            window.Show();
            Logger.Log.InfoFormat("User {0} quitted the system", Settings.Default.Login);
            View.Close();
        }

        /// <summary>
        /// Show record
        /// </summary>
        private void OnShowRecord(object param)
        {
            Window window = new Information();
            window.DataContext = new InformationViewModel(SelectedContact, this) { View = window };
            Logger.Log.InfoFormat("User {0} show record {1}", Settings.Default.Login, SelectedContact.GetHashCode());
            window.ShowDialog();
        }

        /// <summary>
        /// Open repository manager
        /// </summary>
        /// <param name="param"></param>
        private void OnOpenRepositoryManager(object param)
        {
            Window window = new RepositoryManager();
            window.DataContext = new RepositoryManagerViewModel(Repository, this, _memoryRepositories) { View = window };
            window.ShowDialog();
            Logger.Log.InfoFormat("User {0} open repository manager", Settings.Default.Login);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// update repository
        /// </summary>
        public void Update()
        {
            Repository.Update();
            OnPropertyChanged("All");
        }

        #endregion
    }
}
