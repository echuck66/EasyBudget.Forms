using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;
using Microcharts;
using SkiaSharp;
using Entry = Microcharts.Entry;
using System.Linq;
using EasyBudget.Forms.Utility;

namespace EasyBudget.Forms.Pages
{
    public partial class StatusPage : ContentPage
    {
        EasyBudgetDataService ds;
        EasyBudgetStatusViewModel vm;

        public StatusPage()
        {
            InitializeComponent();
            ds = EasyBudgetDataService.Instance;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            vm = await ds.GetStatusVM();

            chartStatus.Chart = ChartUtility.Instance.GetChart(vm);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            vm = null;
        }

        protected async void btnBudgetCategries_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BudgetCategories());

        }

        protected async void btnBankAccounts_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BankAccounts());
        }
    }
}
