using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;
using Microcharts;
using SkiaSharp;
using Entry = Microcharts.Entry;
using System.Linq;
using EasyBudget.Forms.Utility;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetCategoryView : ContentPage
    {
        
        public BudgetCategoryView()
        {
            InitializeComponent();
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var vm = (this.BindingContext as BudgetCategoryViewModel);
            this.chartBudget.Chart = await ChartUtility.Instance.GetChartAsync(vm);
        }

        public async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }





    }
}
