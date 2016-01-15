using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;


using AdressBookLibrary.Model;
using AdressBookLibrary.Commands;

namespace AdressBookWPF.ViewModel
{
    class NoteViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Private members

        private Note _note;
        private string _errormessage;

        #endregion //Private members

        #region Constructors

        public NoteViewModel(Note note)
        {
            _note = new Note(note.NoteText, new ObservableCollection<NoteTag>(note.NoteTags));
            AddNoteTag = new Command(OnAddNoteTag);
            RemoveNoteTag = new Command(OnRemoveNoteTag);
        }

        #endregion //Constructors

        #region Properties

        public NoteTag SelectNoteTag { get; set; }

        public string NoteText
        {
            get
            {
                return _note.NoteText;
            }
            set
            {
                _note.NoteText = value;
                OnPropertyChanged("NoteText");
            }
        }

        public ObservableCollection<NoteTag> NoteTags
        {
            get
            {
                return _note.NoteTags;
            }
            set
            {
                _note.NoteTags = value;
                OnPropertyChanged("NoteTags");
            }
        }

        public Note Note
        {
            get
            {
                return _note;
            }
            set
            {
                _note = value;
            }
        }

        #endregion //Properties

        #region Command

        public Command AddNoteTag { get; set; }
        public Command RemoveNoteTag { get; set; }

        #endregion //Command

        #region Private methods

        private void OnAddNoteTag(object param)
        {
            _note.NoteTags.Add(new NoteTag(""));
            OnPropertyChanged("Tag");
            OnPropertyChanged("NoteTags");
        }

        private void OnRemoveNoteTag(object param)
        {
            _note.NoteTags.Remove(SelectNoteTag);
            OnPropertyChanged("Tag");
            OnPropertyChanged("NoteTags");
        }

        #endregion //Private methods

        #region Validation

        public bool IsValid()
        {
            return IsValidNoteTags() & IsValidNoteText();
        }

        private bool IsValidNoteText()
        {
            if (_note.NoteText == "")
            {
                _errormessage = "Text can not be empty.";
                return false;
            }
            return true;
        }

        private bool IsValidNoteTags()
        {
            if (_note.NoteTags.Count <= 0)
            {
                _errormessage = "Note must have one or more tag.";
                return false;
            }
            foreach (NoteTag item in _note.NoteTags)
            {
                if (ReferenceEquals(item, null))
                {
                    _errormessage = "Note tags is null.";
                    return false;
                }
                if (item.Tag == "")
                {
                    _errormessage = "Tag can not be empty.";
                    return false;
                }
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
                _errormessage = null;
                switch (property)
                {
                    case "NoteTags":
                        IsValidNoteTags();
                        break;
                    case "NoteText":
                        IsValidNoteText();
                        break;
                }
                return _errormessage;
            }
        }

        #endregion //Validation
    }
}
