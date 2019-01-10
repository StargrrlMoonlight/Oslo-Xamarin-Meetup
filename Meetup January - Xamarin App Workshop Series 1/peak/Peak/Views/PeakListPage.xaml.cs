using System;
using System.Threading.Tasks;
using Peak.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Peak.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PeakListPage : ContentPage
    {

        public PeakListPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await RefreshItems(true);

        }

        async void OnItemAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PeakItemPage 
            {
                PeakItem = new PeakItem()
            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new PeakItemPage
            {
                PeakItem = e.SelectedItem as PeakItem
            });
        }

        async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;

            try
            {
                await RefreshItems(true);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }

        async Task RefreshItems(bool syncItems)
        {
            PeakList.ItemsSource = await App.PeakItemManager.GetPeakItemsAsync(syncItems);
        }
    }
}
