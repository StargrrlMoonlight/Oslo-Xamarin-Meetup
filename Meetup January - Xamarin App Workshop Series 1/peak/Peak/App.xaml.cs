using System;
using Peak.Repositories;
using Peak.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Peak
{
    public partial class App : Application
    {
        static PeakItemManager manager;
        public static PeakItemManager PeakItemManager
        {
            get
            {
                if(manager == null)
                    manager = PeakItemManager.DefaultManager;
                return manager;
            }
        }

        public App()
        {
            InitializeComponent();

            var page = new OnboardingPage();
            MainPage = page;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
