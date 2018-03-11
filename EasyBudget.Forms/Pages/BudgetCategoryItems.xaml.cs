using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyBudget.Business.ViewModels;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetCategoryItems : ContentPage
    {
        BudgetCategoryViewModel vm;

        public BudgetCategoryItems()
        {
            InitializeComponent();
            //NavigationPage.SetHasNavigationBar(this, false);
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();
            vm = this.BindingContext as BudgetCategoryViewModel;
		}

		protected void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            vm.SelectedBudgetItem = e.SelectedItem as BudgetItemViewModel;
        }

        protected async void OnItemEdit(object sender, EventArgs e)
        {
            var btn = sender as MenuItem;
            BudgetItemEdit editor = new BudgetItemEdit();
            editor.BindingContext = btn.BindingContext;
            await Navigation.PushAsync(editor);
        }

        protected async void OnItemDelete(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirmation", "Are you sure you want to delete this Item?", "Yes", "No");

            if (answer)
            {
                var btn = sender as MenuItem;
                var budgetItem = btn.BindingContext as BudgetItemViewModel;
                bool deleted = await budgetItem.DeleteAsync();
                if (deleted)
                {
                    await DisplayAlert("Results", "Item Deleted", "Dismiss");
                }
                else
                {
                    await DisplayAlert("Error", "Unable to delte this Item. Message: " + budgetItem.ErrorCondition, "Ok");
                }
            }
        }

        protected async void OnNewItemClicked(object sender, EventArgs e)
        {
            BudgetItemEdit editor = new BudgetItemEdit();
            BudgetItemViewModel newItem = await vm.AddBudgetItemAsync();
            editor.BindingContext = vm.SelectedBudgetItem;
            await Navigation.PushAsync(editor);
        }

        protected async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var budgetItemViewModel = e.Item as BudgetItemViewModel;
            //BudgetCategoryViewTabs viewer = new BudgetCategoryViewTabs();
            //viewer.BindingContext = categoryViewModel;
            //await Navigation.PushModalAsync(viewer);
            BudgetItemView viewer = new BudgetItemView();
            viewer.BindingContext = budgetItemViewModel;
            await Navigation.PushAsync(viewer);
        }
    }
}
