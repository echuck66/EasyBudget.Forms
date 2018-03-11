using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EasyBudget.Forms.Utility.ColorUtility
{
    public partial class ColorFamilyPickListPage : ContentPage
    {
        public ColorFamilyPickListPage()
        {
            InitializeComponent();
        }

        protected async void PrimaryColorTapped(object sender, TappedEventArgs e)
        {
            var vcell = sender != null ? (ViewCell)sender : null;
            var appColor = vcell?.BindingContext as AppColor;

            ColorUtility utility = new ColorUtility();
            utility.AccentColors = appColor?.GetAccentColors();

            ColorPickListPage _accentColorsPage = new ColorPickListPage();
            _accentColorsPage.OnItemColorSelected += OnColorSelected;
            _accentColorsPage.BindingContext = utility;

            await Navigation.PushAsync(_accentColorsPage);

        }

        public delegate void ItemColorSelectedEventHandler(object sender, ItemColorSelectedEventArgs e);

        public event ItemColorSelectedEventHandler OnItemColorSelected;

        public async Task ColorSelected(string code)
        {
            if (this.OnItemColorSelected != null)
            {
                await Task.Run(() => OnItemColorSelected(this, new ItemColorSelectedEventArgs() { colorCode = code }));
                //await Navigation.PopAsync();
            }
        }

        public async void OnColorSelected(object sender, ItemColorSelectedEventArgs e)
        {
            (sender as ColorPickListPage).OnItemColorSelected -= OnColorSelected;
            string colorCode = e.colorCode;
            await ColorSelected(colorCode);

            //await Navigation.PopAsync();
        }
    }
}
