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
        BankAccountViewModel vm = null;

        public CheckingAccountView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            vm = (this.BindingContext as BankAccountViewModel);
            vm.SelectedRegisterItem = null;


            //chartAccountSummary.Chart = await ChartUtility.Instance.GetChartAsync(vm);
            var provider = new MicrochartsProvider<BankAccountViewModel>();
            chartAccountSummary.Chart = await provider.GetChartAsync(vm, 0, false);
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

            var _vmWithdrawal = withdrawalVM as CheckingWithdrawalViewModel;
            await _vmWithdrawal.LoadBudgetData();

            vm.SelectedRegisterItem = _vmWithdrawal;
            if (_vmWithdrawal != null)
            {
                CheckingWithdrawalEdit withdrawalViewer = new CheckingWithdrawalEdit();
                withdrawalViewer.BindingContext = _vmWithdrawal;
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
                    CheckingDepositViewModel _vmDep = regItem as CheckingDepositViewModel;
                    await _vmDep.LoadBudgetData();
                    depEditor.BindingContext = _vmDep;
                    await Navigation.PushAsync(depEditor);
                    break;
                case AccountRegisterItemViewModel.AccountItemType.Withdrawals:
                    CheckingWithdrawalEdit witEditor = new CheckingWithdrawalEdit();
                    CheckingWithdrawalViewModel _vmWithdrawal = regItem as CheckingWithdrawalViewModel;
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
            vm.SelectedRegisterItem = itemVM;
            switch (itemVM.ItemType)
            {
                case AccountRegisterItemViewModel.AccountItemType.Deposits:
                    CheckingDepositView depositViewer = new CheckingDepositView();
                    CheckingDepositViewModel _vmDep = itemVM as CheckingDepositViewModel;
                    await _vmDep.LoadBudgetData();
                    depositViewer.BindingContext = _vmDep;
                    await Navigation.PushAsync(depositViewer);
                    break;
                case AccountRegisterItemViewModel.AccountItemType.Withdrawals:
                    CheckingWithdrawalView withdrawalViewer = new CheckingWithdrawalView();
                    CheckingWithdrawalViewModel _vmWithdrawal = itemVM as CheckingWithdrawalViewModel;
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
            stackTotalDeposits.IsVisible = !stackTotalDeposits.IsVisible;
            stackTotalWithdrawals.IsVisible = !stackTotalWithdrawals.IsVisible;

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
            CheckingAccountEdit editor = new CheckingAccountEdit();
            editor.BindingContext = vm;
            await Navigation.PushAsync(editor);
        }
    }
}
