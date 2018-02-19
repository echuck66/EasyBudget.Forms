using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class BankAccounts : ContentPage
    {
        EasyBudgetDataService ds;
        BankAccountsViewModel vm;

        public BankAccounts()
        {
            InitializeComponent();
            ds = EasyBudgetDataService.Instance;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            vm = await ds.GetBankAccountsViewModelAsync();
        }
    }
}
