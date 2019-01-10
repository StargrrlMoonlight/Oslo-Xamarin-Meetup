using System;
using Peak.Models;
using Xamarin.Forms;

namespace Peak.Views
{
    public partial class PeakItemPage : ContentPage
    {
        public static readonly BindableProperty PeakItemProperty =
            BindableProperty.Create("PeakItem", typeof(PeakItem), typeof(PeakItemPage), null);

        public PeakItem PeakItem
        {
            get { return (PeakItem)GetValue(PeakItemProperty); }
            set { SetValue(PeakItemProperty, value); }
        }

        public PeakItemPage()
        {
            InitializeComponent();
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PeakItem.Title))
            {
               //add new peak, update itemSource
                await App.PeakItemManager.SavePeakItemAsync(PeakItem);
                await App.PeakItemManager.PushChangesAsync();
            }

            await Navigation.PopAsync();
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
