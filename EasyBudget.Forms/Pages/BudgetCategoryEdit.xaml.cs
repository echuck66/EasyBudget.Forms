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

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var vm = (this.BindingContext as BudgetCategoryViewModel);
            chartCategory.Chart = await ChartUtility.Instance.GetChartAsync(vm);

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


    }
}
