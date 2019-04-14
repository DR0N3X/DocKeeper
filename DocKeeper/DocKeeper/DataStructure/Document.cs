using System;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Text;

namespace DataStructure
{
    public class Document
    {
        #region Properties
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public int Identifier { get; set; }
        public ObservableCollection<ObjectAttribute> Attributes { get; set; }
        #endregion

        #region Constructors
        public Document()
        {
            this.Name = string.Empty;
            this.Attributes = new ObservableCollection<ObjectAttribute>();
        }

        public Document(string name, int identifier)
        {
            this.Name = name;
            this.Attributes = new ObservableCollection<ObjectAttribute>();
            this.Identifier = identifier;
        }

        public Document(string name, ObservableCollection<ObjectAttribute> attributes)
        {
            this.Name = name;
            this.Attributes = attributes;
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
