using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdressBookLibrary.Model;
using AdressBookLibrary.Data;
using System.Configuration;
using System.Windows;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Microsoft.Win32;

using AdressBookWPF.Properties;
using AdressBookWPF.Configuration;
using AdressBookLibrary.Commands;

namespace AdressBookWPF.ViewModel
{
    class RepositoryManagerViewModel : ViewModelBase
    {
        #region Private member

        /// <summary>
        /// adding file path
        /// </summary>
        private string _filePath;

        /// <summary>
        /// adding xsd path
        /// </summary>
        private string _xsdPath;

        /// <summary>
        /// adding dtd path
        /// </summary>
        private string _dtdPath;

        /// <summary>
        /// Open file dialog for paths
        /// </summary>
        private OpenFileDialog openFileDialog = new OpenFileDialog() { Multiselect = false, Title = "Open file" };

        /// <summary>
        /// List for memory repositories
        /// </summary>
        private List<IRepository<Contact>> _memoryRepositories = new List<IRepository<Contact>>();

        /// <summary>
        /// List for xml repositories
        /// </summary>
        private List<IRepository<Contact>> _xmlRepositories = new List<IRepository<Contact>>();

        /// <summary>
        /// Kepper link is parent view model
        /// </summary>
        private ListContactViewModel _parentViewModel;

        /// <summary>
        /// current type repository
        /// </summary>
        private RepositoryType _currentTypeRepository;

        /// <summary>
        /// edit type repository
        /// </summary>
        private RepositoryType _editTypeRepository;

        #endregion //Private member

        #region Constructors

        public RepositoryManagerViewModel()
        {
            Logger.InitLogger();

            SaveChange = new Command(OnSaveChange);
            RemoveRepository = new Command(OnRemoveRepository);
            CancelChange = new Command(OnCancelChange);
            Serialise = new Command(OnSerialise);
            AddRepository = new Command(OnAddRepository);
            OpenFilePathDialog = new Command(OnOpenFilePathDialog);
            OpenDTDPathDialog = new Command(OnOpenDTDPathDialog);
            OpenXSDPathDialog = new Command(OnOpenXSDPathDialog);

            XSD = true;

            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSectionCollection sectionCollection = config.GetSectionGroup("AdressBookWPFSectionGroup").Sections;
            Repositories listRepository = (Repositories)sectionCollection.Get("RepositorySection");
            foreach (RepositoryElement item in listRepository.RepositoriesItems)
            {
                _xmlRepositories.Add(new XmlRepository(item.Path, item.XSD, item.DTD) { Name = item.Name });
            }
        }

        public RepositoryManagerViewModel(IRepository<Contact> currentRepository, ListContactViewModel parrentViewModel, List<IRepository<Contact>> memoryRepositories)
            : this()
        {
            _parentViewModel = parrentViewModel;
            _memoryRepositories = memoryRepositories;
            if (!ReferenceEquals(currentRepository, null))
            {
                CurrentTypeRepository = currentRepository.Type;
                if (currentRepository.Type == RepositoryType.XML)
                {
                    foreach (IRepository<Contact> item in _xmlRepositories)
                    {
                        if (item.Name == currentRepository.Name)
                        {
                            SelectedCurrentRepository = item;
                            break;
                        }
                    }
                }
                else if (currentRepository.Type == RepositoryType.Memory)
                {
                    foreach (IRepository<Contact> item in _memoryRepositories)
                    {
                        if (item.Name == currentRepository.Name)
                        {
                            SelectedCurrentRepository = item;
                            break;
                        }
                    }
                }
            }
            else
                SelectedCurrentRepository = currentRepository;
        }

        #endregion //Constructors

        #region Commands

        public Command SaveChange { get; set; }
        public Command RemoveRepository { get; set; }
        public Command CancelChange { get; set; }
        public Command Serialise { get; set; }
        public Command AddRepository { get; set; }
        public Command OpenFilePathDialog { get; set; }
        public Command OpenXSDPathDialog { get; set; }
        public Command OpenDTDPathDialog { get; set; }

        #endregion //Commands

        #region Properties

        /// <summary>
        /// if true validation in XSD schema, else in DTD schema
        /// </summary>
        public bool XSD { get; set; }

        /// <summary>
        /// Current type for repositories
        /// </summary>
        public RepositoryType CurrentTypeRepository
        {
            get
            {
                return _currentTypeRepository;
            }
            set
            {
                if (_currentTypeRepository != value)
                {
                    _currentTypeRepository = value;
                    OnPropertyChanged("CurrentRepositories");
                }
            }
        }

        /// <summary>
        /// List to current repositories
        /// </summary>
        public List<IRepository<Contact>> CurrentRepositories
        {
            get
            {
                if (CurrentTypeRepository == RepositoryType.Memory)
                    return _memoryRepositories;
                if (CurrentTypeRepository == RepositoryType.XML)
                    return _xmlRepositories;
                else
                    return null;
            }
        }

        /// <summary>
        /// Selected current repository
        /// </summary>
        public IRepository<Contact> SelectedCurrentRepository { get; set; }

        /// <summary>
        /// Current type for repositories
        /// </summary>
        public RepositoryType EditTypeRepository
        {
            get
            {
                return _editTypeRepository;
            }
            set
            {
                if (_editTypeRepository != value)
                {
                    _editTypeRepository = value;
                    OnPropertyChanged("EditRepositories");
                    _filePath = null;
                    _xsdPath = null;
                    _dtdPath = null;
                    FileName = null;
                    XSDFileName = null;
                    DTDFileName = null;
                    OnPropertyChanged("FileName");
                    OnPropertyChanged("XSDFileName");
                    OnPropertyChanged("DTDFileName");
                }
            }
        }

        /// <summary>
        /// List to current repositories
        /// </summary>
        public List<IRepository<Contact>> EditRepositories
        {
            get
            {
                if (EditTypeRepository == RepositoryType.Memory)
                    return _memoryRepositories;
                if (EditTypeRepository == RepositoryType.XML)
                    return _xmlRepositories;
                else
                    return null;
            }
        }

        /// <summary>
        /// Selected current repository
        /// </summary>
        public IRepository<Contact> SelectedEditRepository { get; set; }

        /// <summary>
        /// Adding file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Adding dtd file name
        /// </summary>
        public string DTDFileName { get; set; }

        /// <summary>
        /// Adding xsd file name
        /// </summary>
        public string XSDFileName { get; set; }

        #endregion//Properties

        #region Private mhetods

        /// <summary>
        /// Save change current repository
        /// </summary>
        private void OnSaveChange(object param)
        {
            if (CurrentTypeRepository == RepositoryType.XML)
            {
                try
                {
                    (SelectedCurrentRepository as XmlRepository).Open(XSD);
                    _parentViewModel.Repository = SelectedCurrentRepository;
                }
                catch (Exception ex)
                {
                    Logger.Log.ErrorFormat("User {0} unable to load the repository{1}, an error {2}", Settings.Default.Login, (SelectedCurrentRepository as XmlRepository).Name, ex.Message);
                    MessageBox.Show("Unable to load the XML repository will be loaded Memory repository", "Adress Book", MessageBoxButton.OK, MessageBoxImage.Error);
                    _parentViewModel.Repository = _memoryRepositories[0];
                }
            }
            else if (CurrentTypeRepository == RepositoryType.Memory)
            {
                _parentViewModel.Repository = SelectedCurrentRepository;
            }
            View.DialogResult = true;

        }

        /// <summary>
        ///Remove repository from list 
        /// </summary>
        private void OnRemoveRepository(object param) { }

        /// <summary>
        /// Cancel change current repository
        /// </summary>
        private void OnCancelChange(object param) { }

        /// <summary>
        /// Serialise select memory repository
        /// </summary>
        private void OnSerialise(object param)
        {
            //serialization
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream fs1 = new FileStream("BinaryFile.bin", FileMode.Open, FileAccess.ReadWrite))
            {
                binaryFormatter.Serialize(fs1, SelectedEditRepository);
            }
        }

        /// <summary>
        /// Add repository from list
        /// </summary>
        private void OnAddRepository(object param)
        {

            //deserialization
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream fs2 = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                MemoryRepository repo = (MemoryRepository)binaryFormatter.Deserialize(fs2);
                _memoryRepositories.Add(repo);
                OnPropertyChanged("EditRepositories");
            }
        }

        /// <summary>
        /// Open file path dialog
        /// </summary>
        /// <param name="param"></param>
        private void OnOpenFilePathDialog(object param)
        {
            if (EditTypeRepository == RepositoryType.Memory)
                openFileDialog.Filter = "Binary|*.bin";
            else if (EditTypeRepository == RepositoryType.XML)
                openFileDialog.Filter = "XML|*.XML";

            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.SafeFileName;
                _filePath = openFileDialog.FileName;
                OnPropertyChanged("FileName");
            }
        }

        /// <summary>
        /// Open XSD path dialog
        /// </summary>
        /// <param name="param"></param>
        private void OnOpenXSDPathDialog(object param)
        {
            openFileDialog.Filter = "XSD|*.xsd";

            if (openFileDialog.ShowDialog() == true)
            {
                XSDFileName = openFileDialog.SafeFileName;
                _xsdPath = openFileDialog.FileName;
                OnPropertyChanged("XSDFileName");
            }
        }

        /// <summary>
        /// Open DTD path dialog
        /// </summary>
        /// <param name="param"></param>
        private void OnOpenDTDPathDialog(object param)
        {
            openFileDialog.Filter = "DTD|*.dtd";

            if (openFileDialog.ShowDialog() == true)
            {
                DTDFileName = openFileDialog.SafeFileName;
                _dtdPath = openFileDialog.FileName;
                OnPropertyChanged("DTDFileName");
            }
        }


        #endregion//Private mhetods
    }
}
