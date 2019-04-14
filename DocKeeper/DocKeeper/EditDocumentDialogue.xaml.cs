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
	public partial class EditDocumentNameDialogue : ContentPage
	{
        private ObservableCollection<Document> Documents;
        private Document ChosenDocument;

        /// <summary>
        /// Do not use
        /// </summary>
		public EditDocumentNameDialogue()
		{
			InitializeComponent ();
		}

        public EditDocumentNameDialogue(ObservableCollection<Document> documents, Document chosenDocument)
        {
            InitializeComponent();
            Documents = documents;
            ChosenDocument = chosenDocument;
        }

        public void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        public void Edit_Button_Clicked(object sender, EventArgs e)
        {
            //Check if the field was left empty
            if (string.IsNullOrWhiteSpace(NameEntryField.Text) || string.IsNullOrEmpty(NameEntryField.Text))
            {
                DisplayAlert("Error", "Field cannot be empty!", "OK");
                return;
            }

            Document newDocument = new Document(NameEntryField.Text, ChosenDocument.Identifier);

            Documents.Insert(Documents.IndexOf(ChosenDocument), newDocument);
            Documents.Remove(ChosenDocument);

            Navigation.PopModalAsync();
        }
    }
}