using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using EasyBudget.Forms.Utility;
using Microcharts;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetCategoryEdit : ContentPage
    {
        public BudgetCategoryEdit()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var entries = GetEntries();
            var chart = new DonutChart() { Entries = entries };
            chartCategory.Chart = chart;

        }

        protected async void OnSaveClicked(object sender, EventArgs e)
        {
            await (this.BindingContext as BudgetCategoryViewModel).SaveChangesAsync();
            await Navigation.PopModalAsync();
        }

        protected async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        protected  void CategoryTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected Microcharts.Entry[] GetEntries()
        {
            var context = this.BindingContext as BudgetCategoryViewModel;
            return ChartUtility.Instance.GetEntries(context);
        }

    }
}
