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
        BudgetCategoryViewModel vm;
        public BudgetCategoryView()
        {
            InitializeComponent();
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();

            vm = (this.BindingContext as BudgetCategoryViewModel);
            //this.chartBudget.Chart = await ChartUtility.Instance.GetChartAsync(vm);

            var provider = new MicrochartsProvider<BudgetCategoryViewModel>();
            chartCategory.Chart = await provider.GetChartAsync(vm, 0, false);
        }

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
            vm = null;
		}

		//public async void OnBackClicked(object sender, EventArgs e)
		//{
		//    await Navigation.PopModalAsync();
		//}


		public async void btnNewBudgetItem_Clicked(object sender, EventArgs eventArgs)
        {
            if (vm == null)
            {
                vm = this.BindingContext as BudgetCategoryViewModel;
            }

            BudgetItemEdit editor = new BudgetItemEdit();
            BudgetItemViewModel newItem = await vm.AddBudgetItemAsync();
            editor.BindingContext = newItem;
            await Navigation.PushAsync(editor);
            //await Navigation.PushModalAsync(editor);
        }

        public async void btnBudgetItems_Clicked(object sender, EventArgs e)
        {
            BudgetCategoryItems itemsView = new BudgetCategoryItems();
            itemsView.BindingContext = vm;
            await Navigation.PushAsync(itemsView);
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
