using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EasyBudget.Forms.Utility.ColorUtility
{
    public partial class ColorPickListPage : ContentPage
    {
        public ColorPickListPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        protected async void AccentColorTapped(object sender, TappedEventArgs e)
        {
            var vcell = sender != null ? (ViewCell)sender : null;
            var appColor = vcell?.BindingContext as AppColor;

            await ColorSelected(appColor.Name);
            await Navigation.PopAsync();
        }

        public delegate void ItemColorSelectedEventHandler(object sender, ItemColorSelectedEventArgs e);

        public event ItemColorSelectedEventHandler OnItemColorSelected;

        public async Task ColorSelected(string code)
        {
            if (this.OnItemColorSelected != null)
            {
                await Task.Run(() => OnItemColorSelected(this, new ItemColorSelectedEventArgs() { colorCode = code }));
            }
        }

    }
    public class ItemColorSelectedEventArgs : EventArgs
    {
        public string colorCode { get; set; }
    }
}
