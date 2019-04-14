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
	public partial class AddCheckListDialogue : ContentPage
	{
        // Global Variables
        private ObservableCollection<CheckList> CheckLists;
        private ObservableCollection<Person> People;

        public AddCheckListDialogue()
        {
            InitializeComponent();
        }

        public AddCheckListDialogue(ObservableCollection<CheckList> checkLists, ObservableCollection<Person> people)
        {
            InitializeComponent();
            People = people;
            CheckLists = checkLists;
        }

        private void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void Create_Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntryField.Text) || string.IsNullOrEmpty(NameEntryField.Text))
            {
                await DisplayAlert("Error", "Field cannot be empty!", "OK");
                return;
            }

            CheckLists.Add(new CheckList(NameEntryField.Text, People));

            await Navigation.PopModalAsync();
        }
    }
}