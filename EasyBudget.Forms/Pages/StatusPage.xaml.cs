using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class StatusPage : ContentPage
    {
        public StatusPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected async void btnBudgetCategries_Clicked(object sender, EventArgs e)
        {
            //DisplayAlert("Budget Categories", "You clicked Budget Categories", "Cancel");   
            await Navigation.PushAsync(new BudgetCategories());
            //NavigationPage.BarTextColorProperty = UIColor.

        }

        protected async void btnBankAccounts_Clicked(object sender, EventArgs e)
        {
            //DisplayAlert("Bank Accounts", "You clicked Bank Accounts", "Cancel");
            await Navigation.PushAsync(new BankAccounts());
        }
    }
}
