using System;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Text;

namespace DataStructure
{
    public class CheckList
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlIgnore]
        public ObservableCollection<Document> Documents
        {
            set
            {
                _Documents = value;
            }
            get
            {
                return _Documents;
            }
        }
        [XmlIgnore]
        private ObservableCollection<Document> _Documents = new ObservableCollection<Document>();
        [XmlIgnore]
        public ObservableCollection<Person> People
        {
            get
            {
                return _People;
            }
            set
            {
                _People = value;
            }
        }
        private ObservableCollection<Person> _People = null;
        private ObservableCollection<int> WaitingListIdentifiers = new ObservableCollection<int>();
        public ObservableCollection<int> Identifiers
        {
            get
            {
                ObservableCollection<int> tempList = new ObservableCollection<int>();
                foreach (Document doc in Documents)
                {
                    tempList.Add(doc.Identifier);
                }
                return tempList;
            }
            set
            {
                if (People == null)
                {
                    foreach (int item in value)
                    {
                        WaitingListIdentifiers.Add(item);
                        return;
                    }
                }

                foreach(int item in value)
                {
                    Documents.Add(GetDocument(item));
                }
            }
        }

        /// <summary>
        /// Do not use this
        /// </summary>
        public CheckList()
        {
            Name = string.Empty;
            Documents = new ObservableCollection<Document>();

        }

        public CheckList(string name, ObservableCollection<Person> people)
        {
            Name = name;
            People = people;
        }

        /// <summary>
        /// Get a document from the People collection
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        private Document GetDocument(int identifier)
        {
            foreach (Person person in People)
            {
                foreach(Document document in person.Documents)
                {
                    if (document.Identifier == identifier) return document;
                }
            }
            return null;
        }
    }
}
