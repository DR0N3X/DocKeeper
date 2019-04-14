using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using DataStructure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DocKeeper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedWrapperPage : TabbedPage
    {
        public TabbedWrapperPage()
        {
            InitializeComponent();
            this.Children.Add(new MainPage());
            this.Children.Add(new CheckListPage((this.Children[0] as MainPage).People));
        }

        public void SavePageStates()
        {
            if (this.Children.Count == 0) return;
            if (this.Children[0].GetType() != typeof(MainPage)) return;

            (this.Children[0] as MainPage).SafeSave();
            (this.Children[1] as CheckListPage).SafeSave();
        }

        public void LoadPageStates()
        {
            if (this.Children.Count == 0) return;
            if (this.Children[0].GetType() != typeof(MainPage)) return;

            (this.Children[0] as MainPage).SafeLoad();
            (this.Children[1] as CheckListPage).SafeLoad();
        }
    }
}