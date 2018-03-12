using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class CheckingDepositView : ContentPage
    {
        public CheckingDepositView()
        {
            InitializeComponent();
        }

        public async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
