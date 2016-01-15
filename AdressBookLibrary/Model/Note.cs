using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace AdressBookLibrary.Model
{
    [Serializable]
    public class Note
    {
        #region Private members
        
        /// <summary>
        /// Text of note
        /// </summary>
        private string _noteText;

        /// <summary>
        /// tags for note
        /// </summary>
        private ObservableCollection<NoteTag> _noteTags;

        #endregion //Private members

        #region Constructors

        public Note(string noteText, ICollection<NoteTag> noteTags)
        {
            _noteText = noteText;
            _noteTags = new ObservableCollection<NoteTag>(noteTags);
        }

        public Note()
        {
            _noteText = "";
            _noteTags = new ObservableCollection<NoteTag>();
            _noteTags.Add(new NoteTag(""));
        }

        /// <summary>
        /// Constructor for serialise object
        /// </summary>
        protected Note(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("Serialization is not possible, because the SerializationInfo is null");
            _noteText = info.GetString("NoteText");
            _noteTags = (ObservableCollection<NoteTag>)info.GetValue("NoteTags", typeof(ObservableCollection<NoteTag>));
        }

        #endregion//Constructors

        #region Properties

        /// <summary>
        /// Text for note
        /// </summary>
        public string NoteText
        {
            get
            {
                return _noteText;
            }
            set
            {
                if (NoteText != value)
                {
                    _noteText = value;
                }
            }
        }

        /// <summary>
        /// collection note tags
        /// </summary>
        public ObservableCollection<NoteTag> NoteTags
        {
            get
            {
                return _noteTags;
            }
            set
            {
                if (NoteTags != value)
                {
                    _noteTags = value;
                }
            }
        }

        #endregion //Properties

        #region Public mhetods

        /// <summary>
        /// Adding tag for note
        /// </summary>
        /// <param name="tag">tag</param>
        public void AddTag(NoteTag tag)
        {
            _noteTags.Add(tag);
        }

        /// <summary>
        /// Remove tag for note
        /// </summary>
        /// <param name="tag">tag</param>
        public void RemoveTag(NoteTag tag)
        {
            _noteTags.Remove(tag);
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("Desirialization is not possible, because the SerializationInfo is null");
            info.AddValue("NoteText", _noteText);
            info.AddValue("NoteTags", _noteTags);
        }

        public override bool Equals(object obj)
        {
            if (obj is Note)
            {
                Note note = obj as Note;
                if ((ReferenceEquals(note, this))||(note.GetHashCode() == this.GetHashCode()))
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = new int();
            foreach (NoteTag item in NoteTags)
            {
                hashCode += item.Tag.GetHashCode();
            }
            hashCode *= NoteText.GetHashCode();
            return hashCode;
        }

        #endregion //Public mhetods
    }
}
