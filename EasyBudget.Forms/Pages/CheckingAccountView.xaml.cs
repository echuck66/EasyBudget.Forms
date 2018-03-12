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
    }
}
