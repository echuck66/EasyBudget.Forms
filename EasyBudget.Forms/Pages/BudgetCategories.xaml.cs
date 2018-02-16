using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetCategories : ContentPage
    {
        EasyBudgetDataService ds;
        BudgetCategoriesViewModel vm;

        public BudgetCategories()
        {
            InitializeComponent();
            ds = EasyBudgetDataService.Instance;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            vm = await ds.GetBudgetCategoriesViewModelAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            vm = null;
        }
    }
}
