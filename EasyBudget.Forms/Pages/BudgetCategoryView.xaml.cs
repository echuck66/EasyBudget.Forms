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
            chartCategory.Chart = await provider.GetChartAsync(vm, 0, true);
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

    }
}
