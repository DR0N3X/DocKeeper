using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataStructure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DocKeeper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckListPage : ContentPage
    {
        #region Properties
        private ObservableCollection<Person> People { get; set; }
        private ObservableCollection<CheckList> _CheckLists = new ObservableCollection<CheckList>();
        private ObservableCollection<CheckList> CheckLists
        {
            get
            {
                return _CheckLists;
            }
            set
            {
                _CheckLists = value;

                if (People.Count == 0 || People == null) return;
                //else
                foreach (CheckList checkList in CheckLists)
                {
                    checkList.People = People;
                }
            }
        }
        private bool LookingAtCheckListContent = false;
        private CheckList LastViewedCheckList = null;
        private string SaveFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "checklists.xml";
        #endregion

        #region Constructors
        public CheckListPage()
        {
            InitializeComponent();
        }

        public CheckListPage(ObservableCollection<Person> people)
        {
            InitializeComponent();
            People = people;

            MyListView.ItemsSource = _CheckLists;
            MyListTitle.Text = "Checklists";

            //Populate this
            CheckLists.Add(new CheckList("Passport and Identity Card renewal", People));
            CheckLists[0].Documents.Add(People[0].Documents[0]);
        }
        #endregion

        #region Methods

        #region Variable Retrieval
        /// <summary>
        /// Returns the parent of a specified document in the People list
        /// </summary>
        /// <param name="searchIdentifier">Document you are searching the parent for</param>
        /// <returns>Identifier of parent, if document doesn't exist returns -1</returns>
        public int GetDocumentParentIdentifier(int searchIdentifier)
        {
            int currentIdentifier;
            foreach (Person person in People)
            {
                currentIdentifier = person.Identifier;
                foreach (Document document in person.Documents)
                {
                    if (document.Identifier == searchIdentifier) return currentIdentifier;
                }
            }
            currentIdentifier = -1;
            return currentIdentifier;
        }

        /// <summary>
        /// Checks if a Person or Document with the identifier exists
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns>True for exists; False for doesn't exist</returns>
        public bool IdentifierExists(int identifier)
        {
            foreach (Person person in People)
            {
                if (identifier == person.Identifier) return true;
                foreach (Document document in person.Documents)
                {
                    if (identifier == document.Identifier) return true;
                }
            }
            return false;
        }

        public Document GetDocument(int identifier)
        {
            foreach (Person person in People)
            {
                foreach (Document document in person.Documents)
                {
                    return document;
                }
            }
            return null;
        }

        public Person GetPerson(int identifier)
        {
            foreach (Person person in People)
            {
                if (person.Identifier == identifier) return person;
            }
            return null;
        }

        #region Serialization
        public void SafeLoad()
        {
            try
            {
                CheckLists = Load(SaveFilePath);
            }
            catch (Exception) { }
        }

        private ObservableCollection<CheckList> Load(string filePath)
        {
            // Open the file
            StreamReader reader = new StreamReader(filePath);

            // Make a deserializer
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<CheckList>));

            //Deserialize
            ObservableCollection<CheckList> checkLists = (ObservableCollection<CheckList>)xmlSerializer.Deserialize(reader.BaseStream);

            //close the stream
            reader.Close();
            reader.Dispose();

            //Return the results
            return checkLists;
        }

        private void Save(string filePath, ObservableCollection<CheckList> checkLists)
        {
            // Open the file
            StreamWriter writer = new StreamWriter(filePath);

            // Serialize the file
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<CheckList>));

            xmlSerializer.Serialize(writer.BaseStream, checkLists);

            // write the stream to file and close it
            writer.Flush();
            writer.Close();

            return;
        }

        public void SafeSave()
        {
            try
            {
                Save(SaveFilePath, CheckLists);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #endregion

        #region Serializer
        public void LoadSafe()
        {

        }


        #endregion

        #region EventHandlers

        public async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string result = string.Empty;
            if (MyListView.SelectedItem.GetType() == typeof(Document))
            {
                result = await DisplayActionSheet("Are you sure you want to remove this document from the list?", "", "", new string[] { "Yes", "No" });

                if (result == "Yes")
                {
                    LastViewedCheckList.Documents.Remove(e.Item as Document);
                }
                return;


                // Deselect item
                (sender as ListView).SelectedItem = null;
            }

             result = await DisplayActionSheet("What would you like to do ?", "Cancel", string.Empty, new string[] { "View", "Edit", "Delete" });

            if(result == "View")
            {
                LastViewedCheckList = (e.Item as CheckList);
                LookingAtCheckListContent = true;
                MyListView.ItemsSource = (e.Item as CheckList).Documents;
                MyListTitle.Text = (e.Item as CheckList).Name;
            }
            else if(result == "Edit")
            {
                await Navigation.PushModalAsync(new EditCheckListDialogue((e.Item as CheckList), CheckLists, People));
            }
            else if(result == "Delete")
            {
                CheckLists.Remove(e.Item as CheckList);
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        public void AddButton_Clicked(object sender, EventArgs e)
        {
            // Prevent the Dialogue from opening twice
            if (Navigation.ModalStack.Count > 0) return;

            if (LookingAtCheckListContent)
            {
                Navigation.PushModalAsync(new AddDocumentItemDialogue(LastViewedCheckList, People));
            }
            else
            {
                Navigation.PushModalAsync(new AddCheckListDialogue(CheckLists, People));
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if(LookingAtCheckListContent)
            {
                MyListView.ItemsSource = CheckLists;
                MyListTitle.Text = "Checklists";
                LookingAtCheckListContent = false;
            }
            else
            {
                return false;
            }
            return true; // stay in the app
        }


        #endregion
    
        #endregion
    }
}