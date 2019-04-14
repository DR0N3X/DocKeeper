using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

using DataStructure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DocKeeper
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new TabbedWrapperPage();
        }

        protected override void OnStart()
        {
            // Load the previous state of the application
            //(MainPage as TabbedWrapperPage).LoadPageStates();
        }

        protected override void OnSleep()
        {
            // Save the state of the application
            //(MainPage as TabbedWrapperPage).SavePageStates();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            // no need to load anything
        }
    }
}
