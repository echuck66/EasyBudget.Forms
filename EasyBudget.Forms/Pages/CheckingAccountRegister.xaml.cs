using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class CheckingAccountRegister : ContentPage
    {
        public CheckingAccountRegister()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (this.BindingContext as BankAccountViewModel).SelectedRegisterItem = e.SelectedItem as AccountRegisterItemViewModel;
        }

        protected async void OnItemEdit(object sender, EventArgs e)
        {
            var btn = sender as MenuItem;
            var regItem = btn.BindingContext as AccountRegisterItemViewModel;

            switch (regItem.ItemType)
            {
                case AccountRegisterItemViewModel.AccountItemType.Deposits:
                    CheckingDepositEdit depEditor = new CheckingDepositEdit();
                    depEditor.BindingContext = regItem as CheckingDepositViewModel;
                    await Navigation.PushModalAsync(depEditor);
                    break;
                case AccountRegisterItemViewModel.AccountItemType.Withdrawals:
                    CheckingWithdrawalEdit witEditor = new CheckingWithdrawalEdit();
                    witEditor.BindingContext = regItem as CheckingWithdrawalViewModel;
                    await Navigation.PushModalAsync(witEditor);
                    break;
            }
        }

        protected async void OnItemDelete(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirmation", "Are you sure you want to delete this item?", "Yes", "No");

            if (answer)
            {
                var btn = sender as MenuItem;
                var regItem = btn.BindingContext as AccountRegisterItemViewModel;

                bool deleted = false;
                switch(regItem.ItemType)
                {
                    case AccountRegisterItemViewModel.AccountItemType.Deposits:
                        deleted = await (this.BindingContext as BankAccountViewModel).DeleteRegisterItemAsync(regItem as DepositViewModel);

                        break;
                    case AccountRegisterItemViewModel.AccountItemType.Withdrawals:
                        deleted = await (this.BindingContext as BankAccountViewModel).DeleteRegisterItemAsync(regItem as WithdrawalViewModel);

                        break;
                }
                if (deleted)
                {
                    await DisplayAlert("Results", "Item Deleted", "Dismiss");
                }
                else
                {
                    await DisplayAlert("Error", "Unable to delte this Item. Message: " + regItem.ErrorCondition, "Ok");
                }
            }
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var itemVM = e.Item as AccountRegisterItemViewModel;
            switch (itemVM.ItemType)
            {
                case AccountRegisterItemViewModel.AccountItemType.Deposits:
                    CheckingDepositView depositViewer = new CheckingDepositView();
                    depositViewer.BindingContext = itemVM as CheckingDepositViewModel;
                    await Navigation.PushModalAsync(depositViewer);
                    break;
                case AccountRegisterItemViewModel.AccountItemType.Withdrawals:
                    CheckingWithdrawalView withdrawalViewer = new CheckingWithdrawalView();
                    withdrawalViewer.BindingContext = itemVM as CheckingWithdrawalViewModel;
                    await Navigation.PushModalAsync(withdrawalViewer);
                    break;
            }

        }

        protected async void OnNewItemClicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("New Item Type", "Cancel", null, "Deposit", "Withdrawal");

            switch (action)
            {
                case "Deposit":
                    var depositVM = await (this.BindingContext as BankAccountViewModel).AddDepositAsync();
                    if (depositVM != null)
                    {
                        CheckingDepositView depositViewer = new CheckingDepositView();
                        depositViewer.BindingContext = depositVM as CheckingDepositViewModel;
                        await Navigation.PushModalAsync(depositViewer);
                    }
                    break;
                case "Withdrawal":
                    var withdrawalVM = await (this.BindingContext as BankAccountViewModel).AddWithdrawalAsync();
                    if (withdrawalVM != null)
                    {
                        CheckingWithdrawalView withdrawalViewer = new CheckingWithdrawalView();
                        withdrawalViewer.BindingContext = withdrawalVM as CheckingWithdrawalViewModel;
                        await Navigation.PushModalAsync(withdrawalViewer);
                    }
                    break;
            }
        }
    }
}
