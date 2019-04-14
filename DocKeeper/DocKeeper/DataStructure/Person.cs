using System;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Text;

namespace DataStructure
{
    public class Person
    {
        #region Properties
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public int Identifier { get; set; }
        public ObservableCollection<ObjectAttribute> Attributes { get; set; }
        public ObservableCollection<Document> Documents { get; set; }
        #endregion

        #region Constructors
        public Person()
        {
            this.Name = string.Empty;
            this.Attributes = new ObservableCollection<ObjectAttribute>();
            this.Documents = new ObservableCollection<Document>();
            this.Identifier = 0;
        }

        public Person(string name, int identifier)
        {
            this.Name = name;
            this.Attributes = new ObservableCollection<ObjectAttribute>();
            this.Documents = new ObservableCollection<Document>();
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
