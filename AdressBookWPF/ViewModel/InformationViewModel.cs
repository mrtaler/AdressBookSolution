using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

using AdressBookLibrary.Model;
using AdressBookLibrary.Commands;
using AdressBookWPF.Properties;

namespace AdressBookWPF.ViewModel
{
    class InformationViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Private members

        private ObservableCollection<NoteViewModel> _listNoteViewModel;
        private ObservableCollection<PhoneNumberViewModel> _listPhoneNumberViewModel;
        
        /// <summary>
        /// for validation
        /// </summary>
        private string _errormessage;

        /// <summary>
        /// is true if need add record
        /// </summary>
        private bool _isNullContact;

        /// <summary>
        /// keep record
        /// </summary>
        private Contact _contact;

        /// <summary>
        /// keep copy record
        /// </summary>
        private Contact _copyContact;

        /// <summary>
        /// keep information about parent ViewModel
        /// </summary>
        private ListContactViewModel _mainVindowModel;

        #endregion //Private members

        #region Constructors

        public InformationViewModel()
        {
            SaveRecord = new Command(OnSaveRecord);
            CancelRecord = new Command(OnCancelRecord);
            AddNote = new Command(OnAddNote);
            RemoveNote = new Command(OnRemoveNote);
            AddPhoneNumber = new Command(OnAddPhoneNumber);
            RemovePhoneNumber = new Command(OnRemovePhoneNumber);
            Logger.InitLogger();
        }

        public InformationViewModel(Contact contact)
            : this()
        {
            //if need record add, need create new record
            if (ReferenceEquals(contact, null))
            {
                _contact = new Contact();
                _isNullContact = true;
            }
            // if need record show
            else
            {
                _contact = contact;
                _isNullContact = false;
            }
            _copyContact = new Contact(_contact);
        }

        public InformationViewModel(Contact contact, ListContactViewModel model)
            : this(contact)
        {
            _mainVindowModel = model;
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// selected note for remove
        /// </summary>
        public NoteViewModel SelectedNote { get; set; }

        /// <summary>
        /// selected phone number for remove
        /// </summary>
        public PhoneNumberViewModel SelectedPhoneNumber { get; set; }

        /// <summary>
        /// Error Message
        /// </summary>
        public String ErrorMessage { get; set; }

        /// <summary>
        /// first name
        /// </summary>
        public string FirstName
        {
            get
            {
                return _copyContact.FirstName;
            }
            set
            {
                if (_copyContact.FirstName != value)
                {
                    _copyContact.FirstName = value;
                    Logger.Log.DebugFormat("User {0} change filename of {1} to {2}",Settings.Default.Login, _copyContact.FirstName,value);
                    OnPropertyChanged("FirstName");
                    OnPropertyChanged("LastChangeTime");
                }
            }
        }

        /// <summary>
        /// swcond name
        /// </summary>
        public string SecondName
        {
            get
            {
                return _copyContact.SecondName;
            }
            set
            {
                if (_copyContact.SecondName != value)
                {
                    _copyContact.SecondName = value;
                    Logger.Log.DebugFormat("User {0} change secondname of {1} to {2}", Settings.Default.Login, _copyContact.SecondName, value);
                    OnPropertyChanged("SecondName");
                    OnPropertyChanged("LastChangeTime");
                }
            }
        }

        /// <summary>
        /// collection for keep view model of phone numbers
        /// </summary>
        public ObservableCollection<PhoneNumberViewModel> PhoneNumbers
        {
            get
            {
                _listPhoneNumberViewModel = new ObservableCollection<PhoneNumberViewModel>();
                foreach (PhoneNumber item in _copyContact.PhoneNumbers)
                    _listPhoneNumberViewModel.Add(new PhoneNumberViewModel(item));
                return _listPhoneNumberViewModel;
            }
            set
            {
                _listPhoneNumberViewModel = value;
                OnPropertyChanged("PhoneNumbers");
                OnPropertyChanged("LastChangeTime");
            }
        }

        /// <summary>
        /// collection for keep view model of notes
        /// </summary>
        public ObservableCollection<NoteViewModel> Notes
        {
            get
            {
                _listNoteViewModel = new ObservableCollection<NoteViewModel>();
                foreach (Note item in _copyContact.Notes)
                    _listNoteViewModel.Add(new NoteViewModel(item));
                return _listNoteViewModel;
            }
            set
            {
                _listNoteViewModel = value;
                OnPropertyChanged("Notes");
                OnPropertyChanged("LastChangeTime");
            }
        }

        public DateTime LastChangeTime
        {
            get
            {
                return _contact.LastChangeTime;
            }
        }

        #endregion //Properties

        #region Commands

        public Command SaveRecord { get; set; }
        public Command CancelRecord { get; set; }
        public Command AddNote { get; set; }
        public Command RemoveNote { get; set; }
        public Command AddPhoneNumber { get; set; }
        public Command RemovePhoneNumber { get; set; }

        #endregion

        #region Private methods

        /// <summary>
        /// Save record
        /// </summary>
        private void OnSaveRecord(object param)
        {
            _copyContact.Notes = new ObservableCollection<Note>();
            foreach (NoteViewModel item in _listNoteViewModel)
                _copyContact.Notes.Add(item.Note);
            if (IsValid())
            {
                try
                {
                    if (_isNullContact)
                    {
                        _mainVindowModel.Repository.Add(new Contact(_copyContact));
                        Logger.Log.DebugFormat("User {0} added record {1}", Settings.Default.Login, _copyContact.GetHashCode());
                    }
                    else
                    {
                        _mainVindowModel.Repository.Remove(_contact);
                        Logger.Log.DebugFormat("User {0} change record on {1} to {2}", Settings.Default.Login, _contact.GetHashCode(), _copyContact.GetHashCode());
                        _mainVindowModel.Repository.Add(_copyContact);

                    }
                    _mainVindowModel.Repository.Save();
                    _mainVindowModel.Update();
                    View.DialogResult = true;
                }
                catch (Exception ex)
                {
                    Logger.Log.ErrorFormat("When you try to save a record, an error occurred {0}", ex.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Cancel changes
        /// </summary>
        private void OnCancelRecord(object param)
        {
            _mainVindowModel.Update();
            Logger.Log.DebugFormat("User {0} cancelled changes",Settings.Default.Login);
            View.DialogResult = false;
        }

        /// <summary>
        /// Add note in record
        /// </summary>
        private void OnAddNote(object param)
        {
            if (!ReferenceEquals(_copyContact, null))
            {
                _copyContact.AddNote(new Note());
                OnPropertyChanged("Notes");
                OnPropertyChanged("LastChangeTime");
                Logger.Log.DebugFormat("User {0} add phoneNumber", Settings.Default.Login);
            }
        }

        /// <summary>
        /// Remove note in record
        /// </summary>
        private void OnRemoveNote(object param)
        {
            if (!ReferenceEquals(SelectedNote, null))
            {
                _copyContact.RemoveNote(SelectedNote.Note);
                OnPropertyChanged("Notes");
                OnPropertyChanged("LastChangeTime");
                Logger.Log.DebugFormat("User {0} remove notes", Settings.Default.Login);
            }
        }

        /// <summary>
        /// Add phone number in record
        /// </summary>
        private void OnAddPhoneNumber(object param)
        {
            if (!ReferenceEquals(_copyContact, null))
            {
                _copyContact.AddPhoneNumber(new PhoneNumber());
                OnPropertyChanged("PhoneNumbers");
                OnPropertyChanged("LastChangeTime");
                Logger.Log.DebugFormat("User {0} remove phoneNumber", Settings.Default.Login);
            }
        }

        /// <summary>
        /// Remave phone number in record
        /// </summary>
        private void OnRemovePhoneNumber(object param)
        {
            if (!ReferenceEquals(SelectedPhoneNumber, null))
            {
                _copyContact.RemovePhoneNumber(SelectedPhoneNumber.PhoneNumber);
                OnPropertyChanged("PhoneNumbers");
                OnPropertyChanged("LastChangeTime");
                Logger.Log.DebugFormat("User {0} remove phoneNumber", Settings.Default.Login);
            }
        }

        #endregion //Private methods

        #region Validation

        private bool IsValid()
        {
            if (_copyContact.PhoneNumbers.Count < 1)
                return false;
            foreach (PhoneNumberViewModel item in _listPhoneNumberViewModel)
                if (!item.IsValid())
                    return false;
            foreach (NoteViewModel item in _listNoteViewModel)
            {
                if (!item.IsValid())
                    return false;
            }
            return IsValidNames(_copyContact.FirstName) & IsValidNames(_copyContact.SecondName);
        }

        private bool IsValidNames(string name)
        {
            if (name == "")
            {
                _errormessage = "This field must apron.";
                return false;
            }
            if (name.Length < 2)
            {
                _errormessage = "This field must have at least 2 characters.";
                return false;
            }
            return true;
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string property]
        {
            get
            {
                string msg = null;
                switch (property)
                {
                    case "FirstName":
                        if (!IsValidNames(_copyContact.FirstName))
                            msg = _errormessage;
                        break;

                    case "SecondName":
                        if (!IsValidNames(_copyContact.SecondName))
                            msg = _errormessage;
                        break;

                    case "PhoneNumbers":
                        if (_copyContact.PhoneNumbers.Count < 1)
                            msg = "Phone Number field must contain at least one entry";
                        ErrorMessage = msg;
                        OnPropertyChanged("ErrorMessage");
                        break;
                }
                return msg;
            }
        }

        #endregion //Validation
    }
}
