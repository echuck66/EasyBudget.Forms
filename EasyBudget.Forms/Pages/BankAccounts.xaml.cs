using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using EasyBudget.Models;
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
            vm.SelectedBankAccount = null;
            this.BindingContext = vm;

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            vm = null;
        }


        protected void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedVM = e.SelectedItem as BankAccountViewModel;
            vm.SelectedBankAccount = selectedVM;
        }

        protected async void OnItemEdit(object sender, EventArgs e)
        {
            var btn = sender as MenuItem;
            vm.SelectedBankAccount = btn.BindingContext as BankAccountViewModel;
            if (vm.SelectedBankAccount.AccountType == BankAccountType.Checking)
            {
                
            }
            else
            {
                
            }
            //BudgetCategoryEditTabs editor = new BudgetCategoryEditTabs();
            //editor.BindingContext = btn.BindingContext;
            //await Navigation.PushModalAsync(editor);
        }

    }
}
