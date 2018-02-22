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
                CheckingAccountEditTabs editor = new CheckingAccountEditTabs();
                editor.BindingContext = btn.BindingContext;
                await Navigation.PushModalAsync(editor);
            }
            else
            {
                SavingsAccountEditTabs editor = new SavingsAccountEditTabs();
                editor.BindingContext = btn.BindingContext;
                await Navigation.PushModalAsync(editor);
            }

        }

        protected async void OnItemDelete(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirmation", "Are you sure you want to delete this Account?", "Yes", "No");

            if (answer)
            {
                var btn = sender as MenuItem;
                //var category = btn.BindingContext as BankAccountViewModel;
                //bool deleted = await vm.(category);
                //if (deleted)
                //{
                //    await DisplayAlert("Results", "Item Deleted", "Dismiss");
                //}
                //else
                //{
                //    await DisplayAlert("Error", "Unable to delte this Category. Message: " + category.ErrorCondition, "Ok");
                //}
            }
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var accountViewModel = e.Item as BankAccountViewModel;

            switch (accountViewModel.AccountType)
            {
                case BankAccountType.Checking:
                    CheckingAccountViewTabs checkingViewer = new CheckingAccountViewTabs();
                    checkingViewer.BindingContext = accountViewModel;
                    await Navigation.PushModalAsync(checkingViewer);
                    break;
                case BankAccountType.Savings:
                    SavingsAccountViewTabs savingsViewer = new SavingsAccountViewTabs();
                    savingsViewer.BindingContext = accountViewModel;
                    await Navigation.PushModalAsync(savingsViewer);
                    break;
            }
        }

        public async void OnNewItemClicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("New Account Type", "Cancel", null, "Checking", "Savings");

            switch (action)
            {
                case "Checking":
                    await vm.AddCheckingAccountAsync();
                    CheckingAccountEditTabs checkingEditor = new CheckingAccountEditTabs();
                    checkingEditor.BindingContext = vm.SelectedBankAccount;
                    await Navigation.PushModalAsync(checkingEditor);
                    break;
                case "Savings":
                    await vm.AddsavingsAccountAsync();
                    SavingsAccountEditTabs savingsEditor = new SavingsAccountEditTabs();
                    savingsEditor.BindingContext = vm.SelectedBankAccount;
                    await Navigation.PushModalAsync(savingsEditor);
                    break;
            }
        }
    }
}
