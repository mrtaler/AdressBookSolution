using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace AdressBookLibrary.Model
{
    [Serializable]
    public class Contact : IEquatable<Contact>
    {
        #region Private field

        private string _firstName;
        private string _secondName;
        private ObservableCollection<PhoneNumber> _phoneNumbers;
        private ObservableCollection<Note> _notes;
        private DateTime _lastChangeTime;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="firstName">First name of contact</param>
        /// <param name="secondName">Second name of contact</param>
        /// <param name="phoneNiumber">Phone number of contact</param>
        /// <param name="notes">Notes for contact</param>
        public Contact(string firstName, string secondName, ICollection<PhoneNumber> phoneNiumber, ICollection<Note> notes)
        {
            _firstName = firstName;
            _secondName = secondName;
            _phoneNumbers = new ObservableCollection<PhoneNumber>(phoneNiumber);
            _notes = new ObservableCollection<Note>(notes);
            _lastChangeTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Contact()
        {
            _firstName = "";
            _secondName = "";
            _phoneNumbers = new ObservableCollection<PhoneNumber>();
            _notes = new ObservableCollection<Note>();
            _lastChangeTime = DateTime.UtcNow;
        }

        public Contact(Contact contact)
        {
            _firstName = contact.FirstName;
            _secondName = contact.SecondName;
            _phoneNumbers = new ObservableCollection<PhoneNumber>(contact.PhoneNumbers);
            _notes = new ObservableCollection<Note>(contact.Notes);
            _lastChangeTime = contact._lastChangeTime;
        }

        public Contact(string firstName, string secondName, DateTime lastChangeTime, ICollection<PhoneNumber> phoneNiumber, ICollection<Note> notes)
        {
            _firstName = firstName;
            _secondName = secondName;
            _phoneNumbers = new ObservableCollection<PhoneNumber>(phoneNiumber);
            _notes = new ObservableCollection<Note>(notes);
            _lastChangeTime = lastChangeTime;
        }

        public Contact(string firstName, string secondName, string lastChangeTime, ICollection<PhoneNumber> phoneNiumber, ICollection<Note> notes)
        {
            _firstName = firstName;
            _secondName = secondName;
            _phoneNumbers = new ObservableCollection<PhoneNumber>(phoneNiumber);
            _notes = new ObservableCollection<Note>(notes);
            _lastChangeTime =Convert.ToDateTime(lastChangeTime);
        }

        /// <summary>
        /// Constructor for serialise
        /// </summary>
        protected Contact(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("Serialization is not possible, because the SerializationInfo is null");
            _firstName = info.GetString("FirstName");
            _secondName = info.GetString("SecondName");
            _phoneNumbers = (ObservableCollection<PhoneNumber>)info.GetValue("PhoneNumbers", typeof(ObservableCollection<PhoneNumber>));
            _notes = (ObservableCollection<Note>)info.GetValue("Notes", typeof(ObservableCollection<Note>));
            _lastChangeTime = info.GetDateTime("LastChangeTime");
        }

        #endregion

        #region Public properties

        /// <summary>
        /// First name to contact
        /// </summary>
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (!_firstName.Equals(value))
                {
                    _firstName = value;
                    _lastChangeTime = DateTime.UtcNow;
                }
            }
        }

        /// <summary>
        /// Second name to contact
        /// </summary>
        public string SecondName
        {
            get
            {
                return _secondName;
            }
            set
            {
                if (!_secondName.Equals(value))
                {
                    _secondName = value;
                    _lastChangeTime = DateTime.UtcNow;
                }
            }
        }

        /// <summary>
        /// Phone numbers to contact
        /// </summary>
        public ObservableCollection<PhoneNumber> PhoneNumbers
        {
            get
            {
                return _phoneNumbers;
            }
            set
            {
                if (!_phoneNumbers.Equals(value))
                {
                    _phoneNumbers = value;
                    _lastChangeTime = DateTime.UtcNow;
                }
            }
        }

        /// <summary>
        /// Notes to contact
        /// </summary>
        public ObservableCollection<Note> Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                if (!_notes.Equals(value))
                {
                    _notes = value;
                    _lastChangeTime = DateTime.UtcNow;
                }
            }
        }

        /// <summary>
        /// Last change time
        /// </summary>
        public DateTime LastChangeTime
        {
            get
            {
                return _lastChangeTime.ToLocalTime();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add phone number
        /// </summary>
        /// <param name="phoneNumber">phone number</param>
        public void AddPhoneNumber(PhoneNumber phoneNumber)
        {
            _phoneNumbers.Add(phoneNumber);
            _lastChangeTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Remove phone number
        /// </summary>
        /// <param name="phoneNumber">phone number</param>
        /// <returns>true if is remove</returns>
        public bool RemovePhoneNumber(PhoneNumber phoneNumber)
        {
            if (_phoneNumbers.Remove(phoneNumber))
            {
                _lastChangeTime = DateTime.UtcNow;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add note for contact
        /// </summary>
        /// <param name="note">note</param>
        public void AddNote(Note note)
        {
            _notes.Add(note);
            _lastChangeTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Remove note for contact
        /// </summary>
        /// <param name="note">note</param>
        /// <returns>True if is remove</returns>
        public bool RemoveNote(Note note)
        {
            if (_notes.Remove(note))
            {
                _lastChangeTime = DateTime.UtcNow;
                return true;
            }
            return false;
        }

        #endregion

        #region Public mhetods

        public bool Equals(Contact contact)
        {
            if ((FirstName.Equals(contact.FirstName)) &
                (SecondName.Equals(contact.SecondName)) &
                (LastChangeTime.Equals(contact.LastChangeTime)))
                return true;
            return false;
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("Desirialization is not possible, because the SerializationInfo is null");
            info.AddValue("FirstName", _firstName);
            info.AddValue("SecondName", _secondName);
            info.AddValue("PhoneNumbers", _phoneNumbers);
            info.AddValue("Notes", _notes);
            info.AddValue("LastChangeTime", _lastChangeTime);
        }

        #endregion//Public mhetods
    }
}
