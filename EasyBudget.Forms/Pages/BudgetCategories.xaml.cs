using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            vm.SelectedBudgetCategory = null;
            this.BindingContext = vm;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            vm.Dispose();
            vm = null;
        }

        protected void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedVM = e.SelectedItem as BudgetCategoryViewModel;
            vm.SelectedBudgetCategory = selectedVM;
        }

        protected async void OnItemEdit(object sender, EventArgs e)
        {
            var btn = sender as MenuItem;
            BudgetCategoryEditTabs editor = new BudgetCategoryEditTabs();
            editor.BindingContext = btn.BindingContext; 
            await Navigation.PushModalAsync(editor);
        }

        protected async void OnItemDelete(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirmation", "Are you sure you want to delete this Category?", "Yes","No");

            if (answer) 
            {
                var btn = sender as MenuItem;
                var category = btn.BindingContext as BudgetCategoryViewModel;
                bool deleted = await vm.DeleteBudgetCategoryAsync(category);
                if (deleted) 
                {
                    await DisplayAlert("Results", "Item Deleted", "Dismiss");
                }
                else
                {
                    await DisplayAlert("Error", "Unable to delte this Category. Message: " + category.ErrorCondition, "Ok");
                }
            }
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var categoryViewModel = e.Item as BudgetCategoryViewModel;
            BudgetCategoryViewTabs viewer = new BudgetCategoryViewTabs();
            viewer.BindingContext = categoryViewModel;
            await Navigation.PushModalAsync(viewer);
        }

        protected async void OnNewItemClicked(object sender, EventArgs e)
        {
            await vm.AddNewBudgetCategoryAsync();
            BudgetCategoryEditTabs editor = new BudgetCategoryEditTabs();
            editor.BindingContext = vm.SelectedBudgetCategory;
            await Navigation.PushModalAsync(editor);
        }

    }
}
