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
using System.Windows.Shapes;

using AdressBookWPF.ViewModel;
using AdressBookWPF.Properties;
using AdressBookLibrary.Model;

namespace AdressBookWPF
{
    /// <summary>
    /// Interaction logic for ListContact.xaml
    /// </summary>
    public partial class ListContact : Window
    {
        public ListContact()
        {
            InitializeComponent();
            ListContactViewModel vm = new ListContactViewModel();
            vm.View = this;
            DataContext = vm;
            if (Settings.Default.Roles == Roles.Admin)
            {
                addBtn.Visibility = Visibility.Visible;
                delBtn.Visibility = Visibility.Visible;
            }
        }
    }
}
