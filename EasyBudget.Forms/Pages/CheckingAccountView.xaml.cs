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
        
        public CheckingAccountView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var vm = (this.BindingContext as BankAccountViewModel);

            chartDeposits.Chart = await ChartUtility.Instance.GetChartAsync(vm, AccountRegisterItemViewModel.AccountItemType.Deposits);
            chartWithdrawals.Chart = await ChartUtility.Instance.GetChartAsync(vm, AccountRegisterItemViewModel.AccountItemType.Withdrawals);
        }

        public async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
