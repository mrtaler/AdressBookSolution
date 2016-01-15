using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdressBookLibrary.Model;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Linq;
using System.IO;

namespace AdressBookLibrary.Data
{
    public class XmlRepository : IRepository<Contact>
    {
        #region Private member

        /// <summary>
        /// Path for filename
        /// </summary>
        private string _fileName = "";

        /// <summary>
        /// Path for xsd schema
        /// </summary>
        private string _xsdSchema;

        /// <summary>
        /// Path from dtd schema
        /// </summary>
        private string _dtdSchema;

        /// <summary>
        /// Name in repository
        /// </summary>
        private string _name;

        /// <summary>
        /// kepper for xml-data
        /// </summary>
        private XmlDocument _document;

        #endregion//Private members

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filename">file path</param>
        /// <param name="xsdPath">xsd schema path</param>
        /// <param name="dtdPath">dtd schema path</param>
        public XmlRepository(string filename, string xsdPath, string dtdPath)
        {
            _fileName = filename;
            _xsdSchema = xsdPath;
            _dtdSchema = dtdPath;
        }

        #endregion

        #region Properties

        /// <summary>
        /// get All contact
        /// </summary>
        public ICollection<Contact> All
        {
            get
            {
                XmlNodeList nodeList;
                nodeList = _document.SelectNodes("AdressBook/Contact");
                List<Contact> records = new List<Contact>();
                foreach (XmlNode node in nodeList)
                {
                    records.Add(GetRecordFromNode(node));
                }
                return records;
            }
        }

        /// <summary>
        /// Name for repository
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }

        /// <summary>
        /// Type of repository
        /// </summary>
        public RepositoryType Type { get { return RepositoryType.XML; } }

        /// <summary>
        /// path for DTD schema
        /// </summary>
        public string DTDSchema { get { return _dtdSchema; } }

        /// <summary>
        /// path for XSD schema
        /// </summary>
        public string XSDSchema { get { return _xsdSchema; } }

        /// <summary>
        /// path for File
        /// </summary>
        public string FileName { get { return _fileName; } }

        #endregion //Properties

        #region Private mhetods

        /// <summary>
        /// transform node in record
        /// </summary>
        /// <param name="node">node</param>
        /// <returns>record</returns>
        private Contact GetRecordFromNode(XmlNode node)
        {
            string firstName = node.SelectSingleNode("FirstName").InnerText;
            string secondName = node.SelectSingleNode("SecondName").InnerText;
            DateTime lastChangeTime = Convert.ToDateTime(node.SelectSingleNode("LastChangeTime").InnerText);

            XmlNodeList phoneNumbersNodelList = node.SelectNodes("PhoneNumbers/PhoneNumber");
            List<PhoneNumber> phoneNumbersList = new List<PhoneNumber>();
            foreach (XmlNode item in phoneNumbersNodelList)
            {
                string number = item.SelectSingleNode("Number").InnerText;
                string numberName = item.SelectSingleNode("NumberName").InnerText;
                phoneNumbersList.Add(new PhoneNumber(number, numberName));
            }

            XmlNodeList notesNodelList = node.SelectNodes("Notes/Note");
            List<Note> notesList = new List<Note>();
            foreach (XmlNode item in notesNodelList)
            {
                string noteText = item.SelectSingleNode("NoteText").InnerText;
                List<NoteTag> noteTagsList = new List<NoteTag>();
                XmlNodeList noteTagsNodeList = item.SelectNodes("NoteTags/NoteTag");
                foreach (XmlNode tag in noteTagsNodeList)
                {
                    noteTagsList.Add(new NoteTag(tag.InnerText));
                }
                notesList.Add(new Note(noteText, noteTagsList));
            }
            return new Contact(firstName, secondName, lastChangeTime, phoneNumbersList, notesList);
        }

        /// <summary>
        /// transform record to node
        /// </summary>
        /// <param name="contact">record</param>
        /// <returns>node</returns>
        private XmlNode GetNodeFromRecord(Contact contact)
        {

            XmlElement contactNode = _document.CreateElement("Contact");

            XmlElement firstName = _document.CreateElement("FirstName");
            firstName.InnerText = contact.FirstName;
            contactNode.AppendChild(firstName);

            XmlElement secondName = _document.CreateElement("SecondName");
            secondName.InnerText = contact.SecondName;
            contactNode.AppendChild(secondName);

            XmlElement lastChangeTime = _document.CreateElement("LastChangeTime");
            lastChangeTime.InnerText = XmlConvert.ToString(contact.LastChangeTime, XmlDateTimeSerializationMode.Local);
            contactNode.AppendChild(lastChangeTime);

            XmlElement phoneNumbers = _document.CreateElement("PhoneNumbers");
            List<XmlElement> phoneNumbersList = new List<XmlElement>();
            foreach (PhoneNumber item in contact.PhoneNumbers)
            {
                XmlElement phoneNumber = _document.CreateElement("PhoneNumber");
                XmlElement number = _document.CreateElement("Number");
                number.InnerText = item.Number;
                phoneNumber.AppendChild(number);
                XmlElement numberName = _document.CreateElement("NumberName");
                numberName.InnerText = item.NumberName;
                phoneNumber.AppendChild(numberName);
                phoneNumbers.AppendChild(phoneNumber);
            }
            contactNode.AppendChild(phoneNumbers);

            XmlElement notes = _document.CreateElement("Notes");
            List<XmlElement> NotesList = new List<XmlElement>();
            foreach (Note item in contact.Notes)
            {
                XmlElement note = _document.CreateElement("Note");
                XmlElement noteText = _document.CreateElement("NoteText");
                noteText.InnerText = item.NoteText;
                note.AppendChild(noteText);
                XmlElement notaTags = _document.CreateElement("NoteTags");
                foreach (NoteTag tag in item.NoteTags)
                {
                    XmlElement noteTag = _document.CreateElement("NoteTag");
                    noteTag.InnerText = tag.Tag;
                    notaTags.AppendChild(noteTag);
                }
                note.AppendChild(notaTags);
                notes.AppendChild(note);
            }
            contactNode.AppendChild(notes);

            return contactNode;
        }

        #endregion //private mhetods

        #region Public mhetods

        /// <summary>
        /// Add record
        /// </summary>
        public void Add(Contact contact)
        {
            XmlNode root = _document.SelectSingleNode("AdressBook");
            root.AppendChild(GetNodeFromRecord(contact));
        }

        /// <summary>
        /// Update Repository
        /// </summary>
        public void Update()
        {
            _document.Load(_fileName);
        }

        /// <summary>
        /// Remove record
        /// </summary>
        public bool Remove(Contact contact)
        {
            XmlNode root = _document.SelectSingleNode("AdressBook");
            XmlNode child = _document.SelectSingleNode(String.Format("AdressBook/Contact[FirstName='{0}' and SecondName='{1}']", contact.FirstName, contact.SecondName));
            root.RemoveChild(child);
            return true;
        }

        /// <summary>
        /// Save change
        /// </summary>
        /// <returns>true if save</returns>
        public bool Save()
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                using (XmlWriter write = XmlWriter.Create(_fileName, settings))
                {
                    _document.Save(write);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// load and validation xml file
        /// </summary>
        /// <param name="shema"></param>
        public void Open(bool shema)
        {
            //Whether we check everything files exist
            if (File.Exists(_fileName))
            {
                //XSD schema
                if (shema == true)
                {
                    if (File.Exists(_xsdSchema))
                    {
                        _document = new XmlDocument();
                        try
                        {
                            _document.Load(_fileName);
                            _document.Schemas.Add(null, _xsdSchema);
                            _document.Validate((o, e) =>
                            {
                                throw new XmlException(e.Message);
                            });
                        }
                        catch (Exception ex)
                        {
                            throw new XmlException(ex.Message);
                        }
                    }
                    else
                        throw new FileLoadException("It wasn't succeeded to load the file({0}) necessary for XSD of validation, specify other file or a validation method.", _xsdSchema);
                }
                //DTD schema
                else
                {
                    if (File.Exists(_dtdSchema))
                    {
                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.ProhibitDtd = false;
                        settings.ValidationType = ValidationType.DTD;
                        settings.NameTable = new NameTable();
                        XmlParserContext context = new XmlParserContext(
                            settings.NameTable,
                            new XmlNamespaceManager(settings.NameTable),
                            "root-element-name", "",
                            _dtdSchema, "", "", "en",
                            XmlSpace.Default);

                        using (XmlReader reader = XmlReader.Create(_fileName, settings, context))
                        {
                            try
                            {
                                while (reader.Read()) { }
                            }
                            catch (XmlException e)
                            {
                                throw new XmlException(e.Message);
                            }
                            _document = new XmlDocument();
                            _document.Load(_fileName);
                        }
                    }
                    else
                        throw new FileLoadException("It wasn't succeeded to load the file({0}) necessary for DTD of validation, specify other file or a validation method.", _dtdSchema);
                }
            }
            else throw new FileNotFoundException(String.Format("File {0} does not exist", _fileName));
        }

        #endregion//Public mhetod
    }
}