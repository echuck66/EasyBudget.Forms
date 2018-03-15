using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using EasyBudget.Forms.Utility;
using EasyBudget.Models;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class SavingsAccountView : ContentPage
    {
        BankAccountViewModel vm;

        public SavingsAccountView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            vm = this.BindingContext as BankAccountViewModel;
            vm.SelectedRegisterItem = null;

            var chart = await ChartUtility.Instance.GetChartAsync(vm);
            chartAccountSummary.Chart = chart;
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
            var _vmDeposit = depositVM as SavingsDepositViewModel;
            await _vmDeposit.LoadBudgetData();

            vm.SelectedRegisterItem = _vmDeposit;
            if (depositVM != null)
            {
                SavingsDepositEdit depositViewer = new SavingsDepositEdit();
                depositViewer.BindingContext = _vmDeposit;
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
            var _vmWithdrawal = withdrawalVM as SavingsWithdrawalViewModel;
            await _vmWithdrawal.LoadBudgetData();

            vm.SelectedRegisterItem = _vmWithdrawal;
            if (withdrawalVM != null)
            {
                SavingsWithdrawalEdit withdrawalViewer = new SavingsWithdrawalEdit();
                withdrawalViewer.BindingContext = _vmWithdrawal;
                await Navigation.PushAsync(withdrawalViewer);
            }
        }

        protected void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            vm.SelectedRegisterItem = e.SelectedItem as AccountRegisterItemViewModel;
        }

        protected async void OnItemEdit(object sender, EventArgs e)
        {
            var btn = sender as MenuItem;
            var regItem = btn.BindingContext as AccountRegisterItemViewModel;

            vm.SelectedRegisterItem = regItem;
            switch (regItem.ItemType)
            {
                case AccountRegisterItemViewModel.AccountItemType.Deposits:
                    SavingsDepositEdit depEditor = new SavingsDepositEdit();
                    SavingsDepositViewModel _vmDep = regItem as SavingsDepositViewModel;
                    await _vmDep.LoadBudgetData();

                    depEditor.BindingContext = _vmDep;
                    await Navigation.PushAsync(depEditor);
                    break;
                case AccountRegisterItemViewModel.AccountItemType.Withdrawals:
                    SavingsWithdrawalEdit witEditor = new SavingsWithdrawalEdit();
                    SavingsWithdrawalViewModel _vmWithdrawal = regItem as SavingsWithdrawalViewModel;
                    await _vmWithdrawal.LoadBudgetData();

                    witEditor.BindingContext = _vmWithdrawal;
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
                vm.SelectedRegisterItem = regItem;

                bool deleted = false;
                switch (regItem.ItemType)
                {
                    case AccountRegisterItemViewModel.AccountItemType.Deposits:
                        deleted = await vm.DeleteRegisterItemAsync(regItem as DepositViewModel);

                        break;
                    case AccountRegisterItemViewModel.AccountItemType.Withdrawals:
                        deleted = await vm.DeleteRegisterItemAsync(regItem as WithdrawalViewModel);

                        break;
                }
                if (deleted)
                {
                    await DisplayAlert("Results", "Item Deleted", "Dismiss");
                }
                else
                {
                    await DisplayAlert("Error", "Unable to delete this Item. Message: " + regItem.ErrorCondition, "Ok");
                }
            }
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var itemVM = e.Item as AccountRegisterItemViewModel;
            vm.SelectedRegisterItem = itemVM;
            switch (itemVM.ItemType)
            {
                case AccountRegisterItemViewModel.AccountItemType.Deposits:
                    SavingsDepositView depositViewer = new SavingsDepositView();
                    var _vmDep = itemVM as SavingsDepositViewModel;
                    await _vmDep.LoadBudgetData();

                    depositViewer.BindingContext = _vmDep;
                    await Navigation.PushAsync(depositViewer);
                    break;
                case AccountRegisterItemViewModel.AccountItemType.Withdrawals:
                    SavingsWithdrawalView withdrawalViewer = new SavingsWithdrawalView();
                    var _vmWithdrawal = itemVM as SavingsWithdrawalViewModel;
                    await _vmWithdrawal.LoadBudgetData();

                    withdrawalViewer.BindingContext = _vmWithdrawal;
                    await Navigation.PushAsync(withdrawalViewer);
                    break;
            }
        }
        protected void OnShowMoreTapped(object sender, TappedEventArgs e)
        {
            stackRoutingNumber.IsVisible = !stackRoutingNumber.IsVisible;
            stackAccountNumber.IsVisible = !stackAccountNumber.IsVisible;
            if (lblShowMoreLess.Text == "Show More")
            {
                lblShowMoreLess.Text = "Show Less";
            }
            else
            {
                lblShowMoreLess.Text = "Show More";
            }
        }

        protected async void OnAccountEditTapped(object sender, TappedEventArgs e)
        {
            SavingsAccountEdit editor = new SavingsAccountEdit();
            editor.BindingContext = vm;
            await Navigation.PushAsync(editor);
        }
    }
}
