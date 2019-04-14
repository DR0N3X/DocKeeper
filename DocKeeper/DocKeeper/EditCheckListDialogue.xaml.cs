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
	public partial class EditCheckListDialogue : ContentPage
	{
        private CheckList ChosenCheckList;
        private ObservableCollection<CheckList> CheckLists;
        private ObservableCollection<Person> People;

        /// <summary>
        /// Do not use
        /// </summary>
		public EditCheckListDialogue ()
		{
			InitializeComponent ();
		}

        public EditCheckListDialogue(CheckList checkList, ObservableCollection<CheckList> checkLists, ObservableCollection<Person> people)
        {
            InitializeComponent();
            ChosenCheckList = checkList;
            NameEntryField.Text = checkList.Name;
            CheckLists = checkLists;
            People = people;
        }

        public async void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public async void Edit_Button_Clicked(object sender, EventArgs e)
        {
            //Check if the field was left empty
            if (string.IsNullOrWhiteSpace(NameEntryField.Text) || string.IsNullOrEmpty(NameEntryField.Text))
            {
                await DisplayAlert("Error", "Field cannot be empty!", "OK");
                return;
            }

            //Evading the bug where the ListView refuses to refresh
            CheckList newCheckList = new CheckList(NameEntryField.Text, People);
            newCheckList.Documents = ChosenCheckList.Documents;
            newCheckList.Name = NameEntryField.Text;

            CheckLists.Insert(CheckLists.IndexOf(ChosenCheckList), newCheckList);
            CheckLists.Remove(ChosenCheckList);

            await Navigation.PopModalAsync();
        }
    }
}