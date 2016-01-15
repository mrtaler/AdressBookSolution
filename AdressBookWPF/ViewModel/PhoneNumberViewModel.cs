using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.RegularExpressions;

using AdressBookLibrary.Model;

namespace AdressBookWPF.ViewModel
{
    class PhoneNumberViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Private member

        private PhoneNumber _phoneNumber;

        private string _errorMessage;

        #endregion //Private member

        #region Constructors

        public PhoneNumberViewModel(PhoneNumber phoneNumber)
        {
            _phoneNumber = phoneNumber;
        }

        #endregion //Constructors

        #region Properties

        public string Number
        {
            get
            {
                return _phoneNumber.Number;
            }
            set
            {
                _phoneNumber.Number = value;
            }
        }

        public string NumberName
        {
            get
            {
                return _phoneNumber.NumberName;
            }
            set
            {
                _phoneNumber.NumberName = value;
            }
        }

        public PhoneNumber PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                }
            }
        }

        #endregion //Properties

        #region Validation

        /// <summary>
        /// Validation PhoneNumber object
        /// </summary>
        /// <returns>return true if phonenumber objec is valid</returns>
        public bool IsValid()
        {
            return IsValidNumber()&IsValidNumberName();
        }

        /// <summary>
        /// Validation number from PhoneNumber object
        /// </summary>
        /// <returns>return true if number is valid</returns>
        private bool IsValidNumber()
        {
            if (_phoneNumber.Number.Length <=1)
            {
                _errorMessage = "The field must contain at least two characters.";
                return false;
            }
            var reg = new Regex("^\\+?\\d+$");
            if (!reg.Match(_phoneNumber.Number).Success)
            {
                _errorMessage = "Number must be in the format: +999 or 999.";
            }
            return true;
        }

        /// <summary>
        /// Validation number name from PhoneNumber object
        /// </summary>
        /// <returns>return true if number is valid</returns>
        private bool IsValidNumberName()
        {
            if (_phoneNumber.NumberName.Length <=1)
            {
                _errorMessage = "The field must contain at least two characters.";
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
                _errorMessage = null;
                switch (property)
                {
                    case "Number":
                        IsValidNumber();
                        break;
                    case "NumberName":
                        IsValidNumberName();
                        break;
                }
                return _errorMessage;
            }
        }

        #endregion //Validation
    }
}
