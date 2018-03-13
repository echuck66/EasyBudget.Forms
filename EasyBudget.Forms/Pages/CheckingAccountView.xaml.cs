using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using EasyBudget.Forms.Utility;
using EasyBudget.Models;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class CheckingAccountView : ContentPage
    {
        BankAccountViewModel vm;

        public CheckingAccountView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            vm = (this.BindingContext as BankAccountViewModel);
            vm.SelectedRegisterItem = null;
            chartAccountSummary.Chart = await ChartUtility.Instance.GetChartAsync(vm);
        }

        public async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public async void btnNewDeposit_Clicked(object sender, EventArgs eventArgs)
        {
            if (vm == null)
            {
                vm = this.BindingContext as BankAccountViewModel;
            }
            var depositVM = await vm.AddDepositAsync();

            vm.SelectedRegisterItem = depositVM;
            if (depositVM != null)
            {
                CheckingDepositEdit depositViewer = new CheckingDepositEdit();
                depositViewer.BindingContext = depositVM as CheckingDepositViewModel;
                await Navigation.PushAsync(depositViewer);
            }
        }

        public async void btnNewWithdrawal_Clicked(object sender, EventArgs eventArgs)
        {
            if (vm == null)
            {
                vm = this.BindingContext as BankAccountViewModel;
            }
            var withdrawalVM = await vm.AddWithdrawalAsync();
            vm.SelectedRegisterItem = withdrawalVM;
            if (withdrawalVM != null)
            {
                CheckingWithdrawalEdit withdrawalViewer = new CheckingWithdrawalEdit();
                withdrawalViewer.BindingContext = withdrawalVM as CheckingWithdrawalViewModel;
                await Navigation.PushAsync(withdrawalViewer);
            }
        }

        protected void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (this.BindingContext as BankAccountViewModel).SelectedRegisterItem = e.SelectedItem as AccountRegisterItemViewModel;
        }

        protected async void OnItemEdit(object sender, EventArgs e)
        {
            var btn = sender as MenuItem;
            var regItem = btn.BindingContext as AccountRegisterItemViewModel;
            (this.BindingContext as BankAccountViewModel).SelectedRegisterItem = regItem;

            switch (regItem.ItemType)
            {
                case AccountRegisterItemViewModel.AccountItemType.Deposits:
                    CheckingDepositEdit depEditor = new CheckingDepositEdit();
                    depEditor.BindingContext = regItem;
                    await Navigation.PushAsync(depEditor);
                    break;
                case AccountRegisterItemViewModel.AccountItemType.Withdrawals:
                    CheckingWithdrawalEdit witEditor = new CheckingWithdrawalEdit();
                    witEditor.BindingContext = regItem as CheckingWithdrawalViewModel;
                    await Navigation.PushAsync(witEditor);
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
                (this.BindingContext as BankAccountViewModel).SelectedRegisterItem = regItem;

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
            (this.BindingContext as BankAccountViewModel).SelectedRegisterItem = itemVM;
            switch (itemVM.ItemType)
            {
                case AccountRegisterItemViewModel.AccountItemType.Deposits:
                    CheckingDepositView depositViewer = new CheckingDepositView();
                    depositViewer.BindingContext = itemVM as CheckingDepositViewModel;
                    await Navigation.PushAsync(depositViewer);
                    break;
                case AccountRegisterItemViewModel.AccountItemType.Withdrawals:
                    CheckingWithdrawalView withdrawalViewer = new CheckingWithdrawalView();
                    withdrawalViewer.BindingContext = itemVM as CheckingWithdrawalViewModel;
                    await Navigation.PushAsync(withdrawalViewer);
                    break;
            }
        }
    }
}
