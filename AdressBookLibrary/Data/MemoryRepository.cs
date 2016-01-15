using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdressBookLibrary.Model;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace AdressBookLibrary.Data
{
    [Serializable]
    public class MemoryRepository : IRepository<Contact>
    {
        #region Private members
        
        /// <summary>
        /// list contacts
        /// </summary>
        List<Contact> _contacts = new List<Contact>();

        /// <summary>
        /// list contact for work
        /// </summary>
        List<Contact> _contactCopy = new List<Contact>();

        #endregion //private members

        #region Constructors

        public MemoryRepository()
        {
            List<PhoneNumber> numbers = new List<PhoneNumber>();
            numbers.Add(new PhoneNumber("+375293906112", "Mobile"));
            numbers.Add(new PhoneNumber("+1231231321", "Home"));

            List<Note> notes = new List<Note>();
            List<NoteTag> tag = new List<NoteTag>();
            tag.Add(new NoteTag("one tag"));
            tag.Add(new NoteTag("two tag"));
            tag.Add(new NoteTag("three tag"));
            notes.Add(new Note("textdhd trololo", tag));
            notes.Add(new Note("text for note", tag));

            Add(new Contact("Alesya", "Astapenko", numbers, notes));
            notes = new List<Note>();
            tag = new List<NoteTag>();
            tag.Add(new NoteTag("one 1111 tag"));
            tag.Add(new NoteTag("two 1111tag"));
            tag.Add(new NoteTag("three 111tag"));
            notes.Add(new Note("textdhd 11trololo", tag));
            notes.Add(new Note("text for 111note", tag));

            Add(new Contact("Sergey", "Linkevich", numbers, notes));
            notes = new List<Note>();
            tag = new List<NoteTag>();
            tag.Add(new NoteTag("one 1111 tag"));
            tag.Add(new NoteTag("two 1111tag"));
            tag.Add(new NoteTag("three 111tag"));
            notes.Add(new Note("textdhd 11trololo", tag));
            notes.Add(new Note("text for 111note", tag));

            Add(new Contact("Ivan", "Osmalovskii", numbers, notes));
            notes = new List<Note>();
            tag = new List<NoteTag>();
            tag.Add(new NoteTag("one 1111 tag"));
            tag.Add(new NoteTag("two 1111tag"));
            tag.Add(new NoteTag("three 111tag"));
            notes.Add(new Note("textdhd 11trololo", tag));
            notes.Add(new Note("text for 111note", tag));

            Add(new Contact("Evgenii", "Motorin", numbers, notes));
            notes = new List<Note>();
            tag = new List<NoteTag>();
            tag.Add(new NoteTag("one 1111 tag"));
            tag.Add(new NoteTag("two 1111tag"));
            tag.Add(new NoteTag("three 111tag"));
            notes.Add(new Note("textdhd 11trololo", tag));
            notes.Add(new Note("text for 111note", tag));

            Add(new Contact("Stepan", "Lesnov", numbers, notes));
            notes = new List<Note>();
            tag = new List<NoteTag>();
            tag.Add(new NoteTag("one 1111 tag"));
            tag.Add(new NoteTag("two 1111tag"));
            tag.Add(new NoteTag("three 111tag"));
            notes.Add(new Note("textdhd 11trololo", tag));
            notes.Add(new Note("text for 111note", tag));

            Add(new Contact("Ekaterina", "Kazora", numbers, notes));
            notes = new List<Note>();
            tag = new List<NoteTag>();
            tag.Add(new NoteTag("one 1111 tag"));
            tag.Add(new NoteTag("two 1111tag"));
            tag.Add(new NoteTag("three 111tag"));
            notes.Add(new Note("textdhd 11trololo", tag));
            notes.Add(new Note("text for 111note", tag));

            Add(new Contact("Alina", "Petrova", numbers, notes));
            notes = new List<Note>();
            tag = new List<NoteTag>();
            tag.Add(new NoteTag("one 1111 tag"));
            tag.Add(new NoteTag("two 1111tag"));
            tag.Add(new NoteTag("three 111tag"));
            notes.Add(new Note("textdhd 11trololo", tag));
            notes.Add(new Note("text for 111note", tag));

            Add(new Contact("Petr", "Evstratev", numbers, notes));

            Save();
        }

        /// <summary>
        /// Constructor for serialise
        /// </summary>
        protected MemoryRepository(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("Serialization is not possible, because the SerializationInfo is null");
            _contacts = (List<Contact>)info.GetValue("Contacts", typeof(List<Contact>));
            Name = info.GetString("Name");
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// Return collection contact objects
        /// </summary>
        public ICollection<Contact> All
        {
            get
            {
                return _contactCopy;
            }
        }

        /// <summary>
        /// Name of repository
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// type of repository
        /// </summary>
        public RepositoryType Type { get { return RepositoryType.Memory; } }

        #endregion //Properties

        #region Public mhetods

        /// <summary>
        /// Add record
        /// </summary>
        public void Add(Contact value)
        {
            _contactCopy.Add(value);
        }

        /// <summary>
        /// Remove record
        /// </summary>
        public bool Remove(Contact value)
        {
            if (_contactCopy.Remove(value))
                return true;
            return false;
        }

        /// <summary>
        /// Save changes
        /// </summary>
        public bool Save()
        {
            _contacts = new List<Contact>();
            try
            {
                foreach (Contact item in _contactCopy)
                {
                    _contacts.Add(new Contact(item));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Update date from repository
        /// </summary>
        public void Update()
        {
            _contactCopy = new List<Contact>();
            foreach (Contact item in _contacts)
            {
                _contactCopy.Add(new Contact(item));
            }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("Desirialization is not possible, because the SerializationInfo is null");
            info.AddValue("Contacts", _contacts);
            info.AddValue("Name",Name);
        }

        #endregion //Public mhetods
    }
}
