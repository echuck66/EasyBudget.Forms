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
            vm.SelectedBudgetCategoryVM = null;
            this.BindingContext = vm;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            vm = null;
        }

        protected void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedVM = e.SelectedItem as BudgetCategoryViewModel;
            vm.SelectedBudgetCategoryVM = selectedVM;
        }

        protected async void OnItemEdit(object sender, EventArgs e)
        {
            BudgetCategoryEditTabs editor = new BudgetCategoryEditTabs();
            editor.BindingContext = vm.SelectedBudgetCategoryVM;
            await Navigation.PushModalAsync(editor);
        }

        protected void OnItemDelete(object sender, EventArgs e)
        {
            
        }

        protected async void OnNewItemClicked(object sender, EventArgs e)
        {
            await vm.AddNewBudgetCategoryAsync();
            BudgetCategoryEditTabs editor = new BudgetCategoryEditTabs();
            editor.BindingContext = vm.SelectedBudgetCategoryVM;
            await Navigation.PushModalAsync(editor);
        }
    }
}
