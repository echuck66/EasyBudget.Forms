using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetCategoryEdit : ContentPage
    {
        //EasyBudgetDataService ds;
        //BudgetCategoryViewModel vm;
        //int budgetCategoryId;

        public BudgetCategoryEdit()
        {
            InitializeComponent();
            //ds = EasyBudgetDataService.Instance;

        }

        //public BudgetCategoryEdit(BudgetCategoryViewModel viewmodel)
        //{
        //    InitializeComponent();
        //    ds = EasyBudgetDataService.Instance;
        //    vm = viewmodel;
        //}

        //public BudgetCategoryEdit(int categoryId)
        //{
        //    InitializeComponent();
        //    ds = EasyBudgetDataService.Instance;
        //    budgetCategoryId = categoryId;
        //}

        //protected async override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    vm = await ds.GetBudgetCategoryVM(budgetCategoryId);
        //}

        protected async void OnSaveClicked(object sender, EventArgs e)
        {
            await (this.BindingContext as BudgetCategoryViewModel).SaveChangesAsync();
            await Navigation.PopModalAsync();
        }

        protected async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}
