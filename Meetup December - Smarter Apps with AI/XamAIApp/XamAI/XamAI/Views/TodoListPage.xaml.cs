using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamAI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodoListPage : ContentPage
	{
		public TodoListPage ()
		{
			InitializeComponent ();
		}

        async void OnItemAdded(object sender, EventArgs e)
        {
            //todo: Next Meetup :)
        }

        async void OnRateApplication(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RateAppPage());
        }
    }
}