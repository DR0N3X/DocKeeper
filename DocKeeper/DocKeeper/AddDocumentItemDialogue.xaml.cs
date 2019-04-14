using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataStructure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DocKeeper
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddDocumentItemDialogue : ContentPage
	{
        private ObservableCollection<Person> People { get; set; }
        private ObservableCollection<object> SelectionTree { get; set; }
        private CheckList CheckList { get; set; }
        private bool IsLookingAtDocuments = false;
        private Person LastViewedPerson;

        public AddDocumentItemDialogue()
        {
            InitializeComponent();
        }

        public AddDocumentItemDialogue(CheckList checkList ,ObservableCollection<Person> people)
        {
            InitializeComponent();
            People = people;
            ListViewTitle.Text = "People";
            MyListView.ItemsSource = People;
            CheckList = checkList;
        }

        #region Event Handlers

        public async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item.GetType() == typeof(Person))
            {
                LastViewedPerson = (e.Item as Person);
                IsLookingAtDocuments = true;
                MyListView.ItemsSource = (e.Item as Person).Documents;
                ListViewTitle.Text = "Documents of " + (e.Item as Person).Name;
            }
            else
            {
                // Only add the document if it isn't in the list already
                if(CheckList.Documents.Contains(e.Item as Document))
                {
                    await DisplayAlert("Duplicate item", "That Item is already in the checklist!", "OK");
                    return;
                }
                CheckList.Documents.Add((e.Item as Document));
                await Navigation.PopModalAsync();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (IsLookingAtDocuments)
            {
                MyListView.ItemsSource = People;
                ListViewTitle.Text = "People";
                IsLookingAtDocuments = false;
            }
            else
            {
                Navigation.PopModalAsync();
            }
            return true; // stay in the app
        }
        #endregion
    }
}