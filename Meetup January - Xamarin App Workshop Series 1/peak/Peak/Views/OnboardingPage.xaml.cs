using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Peak.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnboardingPage : ContentPage
    {
        public OnboardingPage()
        {
            InitializeComponent();
        }

        private void OnLogInTapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PeakListPage());
        }
    }
}
