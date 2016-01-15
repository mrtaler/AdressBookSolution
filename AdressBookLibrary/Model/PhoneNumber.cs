using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace AdressBookLibrary.Model
{
    [Serializable]
    public class PhoneNumber
    {
        #region Private member

        /// <summary>
        /// number
        /// </summary>
        private string _number;

        /// <summary>
        /// name number
        /// </summary>
        private string _numberName;

        #endregion //private member

        #region Properties

        /// <summary>
        /// phone number
        /// </summary>
        public string Number
        {
            get
            {
                return _number;

            }
            set
            {
                if (_number!=value)
                {
                    _number = value;
                }
            }
        }

        /// <summary>
        /// name phone number
        /// </summary>
        public string NumberName
        {
            get
            {
                return _numberName;

            }
            set
            {
                if (_numberName!=value)
                {
                    _numberName = value;
                }
            }
        }

        #endregion //Properties

        #region Constructors

        public PhoneNumber(string number, string numberName)
        {
            _number = number;
            _numberName = numberName;
        }

        public PhoneNumber()
            : this("", "") { }

        public PhoneNumber(PhoneNumber phoneNumber)
            : this(phoneNumber._number, phoneNumber._numberName) { }

        /// <summary>
        /// Constructor for serialise object
        /// </summary>
        protected PhoneNumber(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("Serialization is not possible, because the SerializationInfo is null");
            _number = info.GetString("Number");
            _numberName = info.GetString("NumberName");
        }

        #endregion //Constructors

        #region Public methods

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("Desirialization is not possible, because the SerializationInfo is null");
            info.AddValue("Number", _number);
            info.AddValue("NumberName", _numberName);
        }

        public override bool Equals(object obj)
        {
            if (!ReferenceEquals(obj, null))
            {
                if (obj is PhoneNumber)
                {
                    PhoneNumber phoneNumber = obj as PhoneNumber;
                    if (ReferenceEquals(this, phoneNumber))
                        return true;
                    if(this.GetHashCode()==phoneNumber.GetHashCode())
                        return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            int getHashCode = Number.GetHashCode()*17+Number.GetHashCode();
            return getHashCode.GetHashCode();
        }

        #endregion //Base ovveride mhetods
    }
}
