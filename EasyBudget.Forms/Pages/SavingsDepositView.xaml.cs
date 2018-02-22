using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class SavingsDepositView : ContentPage
    {
        public SavingsDepositView()
        {
            InitializeComponent();
        }

        public async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
