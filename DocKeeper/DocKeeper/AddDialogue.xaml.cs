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
	public partial class AddDialogue : ContentPage
	{
        // Global Variables
        private Type ListType;
        private object List;

        public AddDialogue()
        {
            InitializeComponent();
        }

        public AddDialogue (object list, Type listType)
		{
			InitializeComponent();

            // Assign Variables
            List = list;
            ListType = listType;

            // Decide for a title
            if(listType == typeof(Document))
            {
                this.Title = "Add new document";
            }
            else if(listType == typeof(Person))
            {
                this.Title = "Add new person";
            }
        }

        private void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void Create_Button_Clicked(object sender, EventArgs e)
        {
            // Ignore if there is no input
            if (string.IsNullOrWhiteSpace(NameEntryField.Text) || string.IsNullOrEmpty(NameEntryField.Text))
            {
                await DisplayAlert("Error", "Field cannot be empty!", "OK");
                return;
            }

            // Determine which type the list contains and add the according object

            if (ListType == typeof(Document))
            {
                (List as ObservableCollection<Document>).Add(new Document(NameEntryField.Text, 0));
            }
            else if(ListType == typeof(Person))
            {
                (List as ObservableCollection<Person>).Add(new Person(NameEntryField.Text, 0));
            }
            await Navigation.PopModalAsync();
        }
    }
}