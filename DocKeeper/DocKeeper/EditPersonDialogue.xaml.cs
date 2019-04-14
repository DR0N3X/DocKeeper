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
	public partial class EditPersonDialogue : ContentPage
	{
        private ObservableCollection<Person> People;
        private Person ChosenPerson;

		public EditPersonDialogue ()
		{
			InitializeComponent ();
		}

        public EditPersonDialogue(ObservableCollection<Person> people, Person chosenPerson)
        {
            InitializeComponent();
            People = people;
            ChosenPerson = chosenPerson;

            // Put old name into the entryfield
            NameEntryField.Text = ChosenPerson.Name;
        }

        public void Edit_Button_Clicked(object sender, EventArgs e)
        {
            //Check if the name was left empty
            if (string.IsNullOrWhiteSpace(NameEntryField.Text) || string.IsNullOrEmpty(NameEntryField.Text))
            {
                DisplayAlert("Error", "Field cannot be empty!", "OK");
                return;
            }

            //Evading the bug where the ListView refuses to refresh
            Person newPerson = new Person(NameEntryField.Text, ChosenPerson.Identifier);
            People.Add(newPerson);
            People.Remove(ChosenPerson);

            Navigation.PopModalAsync(true);
        }

        public async void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }
    }
}