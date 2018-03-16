using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using EasyBudget.Forms.Utility;
using EasyBudget.Models;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class SavingsAccountEdit : ContentPage
    {

        BankAccountViewModel vm;

        public SavingsAccountEdit()
        {
            InitializeComponent();
        }

		protected async override void OnAppearing()
		{
			base.OnAppearing();
            vm = (this.BindingContext as BankAccountViewModel);
            //chartAccountSummary.Chart = await ChartUtility.Instance.GetChartAsync(vm);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
            vm = null;
		}

		protected async void OnSaveClicked(object sender, EventArgs e)
        {
            await (this.BindingContext as BankAccountViewModel).SaveChangesAsync();
            await Navigation.PopAsync();
        }

        protected async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}
