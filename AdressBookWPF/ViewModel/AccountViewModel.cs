using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

using AdressBookLibrary.Model;
using AdressBookLibrary.Commands;
using AdressBookWPF.Properties;
using AdressBookWPF.Configuration;

namespace AdressBookWPF.ViewModel
{
    class AccountViewModel : ViewModelBase
    {

        private Users list;
        private string _login;

        public Command LogIn { get; set; }

        public string MessageError { get; private set; }
        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged("Login");
                }
            }
        }
        public Language Language { get; set; }

        public AccountViewModel()
        {
            LogIn = new Command(OnLogIn);
            Language = Language.English;
            Logger.InitLogger();
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSectionCollection sectionCollection=config.GetSectionGroup("AdressBookWPFSectionGroup").Sections;
            list = (Users)sectionCollection.Get("UsersSection");
        }

        private void OnLogIn(object param)
        {
            string password = (param as PasswordBox).Password;
            if ((_login != "") & (password != ""))
            {
                foreach (UserElement item in list.UserItems)
                {
                    if ((_login == item.Login) & (item.Password == password))
                    {
                        Settings.Default.Password = item.Password;
                        Settings.Default.Login = item.Login;
                        Settings.Default.Roles = (Roles)Convert.ToByte(item.Roles);
                        Logger.Log.InfoFormat("User {0} logged in", Settings.Default.Login);
                        switch (Language)
                        {
                            case Language.English:
                                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-Us");
                                break;
                            case Language.Russian:
                                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
                                break;
                            case Language.Italiano:
                                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("it-IT");
                                break;
                        }
                        Logger.Log.DebugFormat("User {0} chose culture {1}", Settings.Default.Login, System.Threading.Thread.CurrentThread.CurrentUICulture.DisplayName);
                        Window window = new ListContact();
                        window.Show();
                        View.Close();
                        return;
                    }
                }
                MessageError = "Invalid username or password.";
            }
            else
                MessageError = "Incorrect input information.";
            OnPropertyChanged("MessageError");
        }
    }
}
