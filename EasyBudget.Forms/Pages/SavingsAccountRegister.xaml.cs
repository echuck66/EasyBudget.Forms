using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class SavingsAccountRegister : ContentPage
    {
        public SavingsAccountRegister()
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
                    SavingsDepositEdit depEditor = new SavingsDepositEdit();
                    depEditor.BindingContext = regItem as SavingsDepositViewModel;
                    await Navigation.PushModalAsync(depEditor);
                    break;
                case AccountRegisterItemViewModel.AccountItemType.Withdrawals:
                    SavingsWithdrawalEdit witEditor = new SavingsWithdrawalEdit();
                    witEditor.BindingContext = regItem as SavingsWithdrawalViewModel;
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
                switch (regItem.ItemType)
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
                    SavingsDepositView depositViewer = new SavingsDepositView();
                    depositViewer.BindingContext = itemVM as SavingsDepositViewModel;
                    await Navigation.PushModalAsync(depositViewer);
                    break;
                case AccountRegisterItemViewModel.AccountItemType.Withdrawals:
                    SavingsWithdrawalView withdrawalViewer = new SavingsWithdrawalView();
                    withdrawalViewer.BindingContext = itemVM as SavingsWithdrawalViewModel;
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
                        SavingsDepositView depositViewer = new SavingsDepositView();
                        depositViewer.BindingContext = depositVM as SavingsDepositViewModel;
                        await Navigation.PushModalAsync(depositViewer);
                    }
                    break;
                case "Withdrawal":
                    var withdrawalVM = await (this.BindingContext as BankAccountViewModel).AddWithdrawalAsync();
                    if (withdrawalVM != null)
                    {
                        SavingsWithdrawalView withdrawalViewer = new SavingsWithdrawalView();
                        withdrawalViewer.BindingContext = withdrawalVM as SavingsWithdrawalViewModel;
                        await Navigation.PushModalAsync(withdrawalViewer);
                    }
                    break;
            }
        }

    }
}
