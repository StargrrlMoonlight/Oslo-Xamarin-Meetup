using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //todo
        }

        async void OnRateApplication(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RateAppPage());
        }
    }
}