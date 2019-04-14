using System;
using DataStructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DocKeeper
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DocumentPage : ContentPage
	{
        Document _Document;
        Document Document
        {
            get
            {
                return _Document;
            }
            set
            {
                // replace variable
                _Document = value;

                // set variables in the UI
                AttributeListView.ItemsSource = _Document.Attributes;
                DocumentNameLabel.Text = Document.Name;
            }
        }
        string Name = string.Empty;

		public DocumentPage (Document document)
		{
			InitializeComponent();
            Document = document;
		}
	}
}