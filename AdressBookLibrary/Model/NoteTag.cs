 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace AdressBookLibrary.Model
{
    [Serializable]
    public class NoteTag
    {

        #region Constructors

        public NoteTag(string tag) {
            Tag = tag;
        }

        /// <summary>
        /// Constructor for serialise object
        /// </summary>
        protected NoteTag(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("Serialization is not possible, because the SerializationInfo is null");
            Tag = info.GetString("NoteTag");
        }

        #endregion //Constructors

        public string Tag { get; set; }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("Desirialization is not possible, because the SerializationInfo is null");
            info.AddValue("NoteTag", Tag);
        }
    }
}
