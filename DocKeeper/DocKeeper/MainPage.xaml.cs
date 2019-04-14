using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using DataStructure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DocKeeper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        #region Properties
        private Random Rnd = new Random();
        private Person LastViewedPerson;
        private bool IsLookingAtDocuments = false;
        private bool WelcomeMessageShown = false;

        public string SaveFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "data.xml";

        public ObservableCollection<Person> _People;
        public ObservableCollection<Person> People
        {
            get
            {
                if (_People == null)
                {
                    _People = new ObservableCollection<Person>();
                }
                return _People;
            }
            set
            {
                _People = value;
            }
        }
        #endregion

        #region Constructors

        public MainPage()
        {
            // Initialize UI
            InitializeComponent();

            // Initialize Variables
            ListViewTitle.Text = string.Empty;

            // Create Bindings
            MyListView.ItemsSource = People;
            ListViewTitle.Text = "People";

            //Populate with some sample data
            People.Add(new Person("Marvin Müller", 0));
            People[0].Documents.Add(new Document("Identity Card", 1));
            People[0].Documents.Add(new Document("Passport", 2));
            People[0].Documents.Add(new Document("Drivers License", 3));
        }
        #endregion

        #region Methods

        #region Data Handling

        /// <summary>
        /// Returns a random Number above 1_000_000
        /// </summary>
        public int GetUniqueIdentifier()
        {
            int rndNum;
            do
            {
                rndNum = Rnd.Next();
            }
            while (rndNum < 1_000_000 || IdentifierExists(rndNum));
            return rndNum;
        }

        public void ReplaceFaultyIdentifiers()
        {
            //Check for a 0 identifier and replace it with a better one
            foreach (Person person in People)
            {
                foreach (Document doc in person.Documents)
                {
                    person.Identifier = GetUniqueIdentifier();
                    if (doc.Identifier == 0)
                    {
                        doc.Identifier = GetUniqueIdentifier();
                    }
                }
            }
        }

        /// <summary>
        /// Iterates through the entire People List to check for existing identifiers
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public bool IdentifierExists(int identifier)
        {
            foreach (Person person in People)
            {
                if (person.Identifier == identifier) return true; // identifier exists
                foreach (Document document in person.Documents)
                {
                    if (document.Identifier == identifier) return true; // identifier exists
                }
            }
            return false; // identifier doesn't exist
        }

        #endregion

        #region UI-Related
        public void OpenDocument(Document document)
        {
            // Feature deactivated
            //DocumentPage documentPage = new DocumentPage(document);
            //Navigation.PushAsync(documentPage, true);
        }

        public void OpenAttribute(ObjectAttribute attribute)
        {

        }

        /// <summary>
        /// Gets the name contained in a 'Person' or 'Document' object
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string GetName(object item)
        {
            if (item.GetType() == typeof(Person))
            {
                return (item as Person).Name;
            }
            else if (item.GetType() == typeof(Object))
            {
                return (item as Person).Name;
            }
            return string.Empty;
        }
        #endregion

        #region Serialization
        public void SafeLoad()
        {
            try
            {
                People = Load(SaveFilePath);
            }
            catch (Exception){}
        }

        private ObservableCollection<Person> Load(string filePath)
        {
            // Open the file
            StreamReader reader = new StreamReader(filePath);

            // Make a deserializer
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Person>));

            //Deserialize
            People = (ObservableCollection<Person>)xmlSerializer.Deserialize(reader.BaseStream);

            //close the stream
            reader.Close();
            reader.Dispose();

            //Return the results
            return People;
        }

        private void Save(string filePath, ObservableCollection<Person> people)
        {
            // Open the file
            StreamWriter writer = new StreamWriter(filePath);

            // Serialize the file
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Person>));

            xmlSerializer.Serialize(writer.BaseStream, people);

            // write the stream to file and close it
            writer.Flush();
            writer.Close();

            return;
        }

        public void SafeSave()
        {
            try
            {
                Save(SaveFilePath, People);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Event Handlers
        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // if no item was actually selected -> exit
            if (e.Item == null)
                return;

            string[] buttons = new string[] { "View", "Edit", "Delete" };
            if (IsLookingAtDocuments)
            {
                buttons = new string[] { "Edit", "Delete" };
            }

            string result = await DisplayActionSheet("What would you like to do ?", "Cancel", string.Empty, buttons);

            if(result == "View")
            {
                MyListView.ItemsSource = (e.Item as Person).Documents;
                ListViewTitle.Text = "Documents of " + (e.Item as Person).Name;
                LastViewedPerson = (e.Item as Person);
                IsLookingAtDocuments = true;
            }
            else if(result == "Edit")
            {
                if (IsLookingAtDocuments)
                {
                    await Navigation.PushModalAsync(new EditDocumentNameDialogue(LastViewedPerson.Documents, (e.Item as Document)));
                }
                else
                {
                    await Navigation.PushModalAsync(new EditPersonDialogue(People, (e.Item as Person)));
                }
                return;
            }
            else if(result == "Delete")
            {
                string decision = await DisplayActionSheet("Are you sure you want to delete '" + GetName(e.Item) + "'", "Yes", "No", new string[] { });
                if (decision == "Yes")
                {
                    if (e.Item.GetType() == typeof(Person))
                    {
                        People.Remove(e.Item as Person);
                    }
                    else
                    {
                        LastViewedPerson.Documents.Remove(e.Item as Document);
                    }
                }
            }
            //else just deselect and return

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        /// <summary>
        /// Gets fired whenever a back-button is pressed
        /// </summary>
        /// <returns>True = stays in the app; False = exits to android</returns>
        protected override bool OnBackButtonPressed()
        {
            if (IsLookingAtDocuments)
            {
                MyListView.ItemsSource = People;
                ListViewTitle.Text = "People";
                IsLookingAtDocuments = false;
                return true;
            }
            return false;
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            ReplaceFaultyIdentifiers();
            // Prevent it from opening twice
            if (Navigation.ModalStack.Count > 0)
            {
                return;
            }

            if (IsLookingAtDocuments)
            {
                Navigation.PushModalAsync(new AddDialogue(LastViewedPerson.Documents, typeof(Document)));
            }
            else
            {
                Navigation.PushModalAsync(new AddDialogue(People, typeof(Person)));
            }
        }
        #endregion

        #region Debug
        /// <summary>
        /// Populates the 'People'-List with a little bit of data
        /// </summary>
        private void Populate()
        {
            for (int i = 0; i < 10; i++)
            {
                People.Add(new Person("Person " + i, GetUniqueIdentifier()));
                for (int j = 0; j < 20; j++)
                {
                    People[i].Documents.Add(new Document("Document " + j, GetUniqueIdentifier()));
                    for (int k = 0; k < 15; k++)
                    {
                        People[i].Documents[j].Attributes.Add(new ObjectAttribute("Attribute " + k, "Test Content"));
                    }
                }
            }
        }
        #endregion

        #endregion

        private void MyListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            DisplayWelcomeMessage();
        }

        public void DisplayWelcomeMessage()
        {
            if (!WelcomeMessageShown)
            {
                WelcomeMessageShown = true;
                DisplayAlert("Hello world!", "Manage your documents on the left tab, and mark documents, that you need to take with you on your next trip to the authorities on the right tab, so that you can stop worrying about having forgotten something!", "Sure thing!");
            }
        }
    }
}
