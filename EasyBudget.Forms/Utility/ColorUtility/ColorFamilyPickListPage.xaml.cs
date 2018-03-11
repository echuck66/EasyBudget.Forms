using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EasyBudget.Forms.Utility.ColorUtility
{
    public partial class ColorFamilyPickListPage : ContentPage
    {
        public ColorFamilyPickListPage()
        {
            InitializeComponent();
        }

        public async void PrimaryColorTapped(object sender, TappedEventArgs e)
        {
            var vcell = sender != null ? (ViewCell)sender : null;
            var appColor = vcell?.BindingContext as AppColor;

            ColorUtility utility = new ColorUtility();
            utility.AccentColors = appColor?.GetAccentColors();

            ColorPickListPage _accentColorsPage = new ColorPickListPage();
            _accentColorsPage.BindingContext = utility;

            await Navigation.PushModalAsync(_accentColorsPage);
        }
    }
}
