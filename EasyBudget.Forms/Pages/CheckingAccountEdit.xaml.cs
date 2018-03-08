using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using EasyBudget.Forms.Utility;
using EasyBudget.Models;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class CheckingAccountEdit : ContentPage
    {
        
        public CheckingAccountEdit()
        {
            InitializeComponent();
        }

		protected async override void OnAppearing()
		{
			base.OnAppearing();
            var vm = (this.BindingContext as BankAccountViewModel);

            chartAccountSummary.Chart = await ChartUtility.Instance.GetChartAsync(vm);
		}

		protected async void OnSaveClicked(object sender, EventArgs e)
        {
            await (this.BindingContext as BankAccountViewModel).SaveChangesAsync();
            await Navigation.PopModalAsync();
        }

        protected async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}
