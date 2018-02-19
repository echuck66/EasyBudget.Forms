using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetCategoryEdit : ContentPage
    {
        EasyBudgetDataService ds;
        BudgetCategoryViewModel vm;
        int budgetCategoryId;

        public BudgetCategoryEdit(int categoryId)
        {
            InitializeComponent();
            ds = EasyBudgetDataService.Instance;
            budgetCategoryId = categoryId;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            vm = await ds.GetBudgetCategoryVM(budgetCategoryId);
        }
    }
}
