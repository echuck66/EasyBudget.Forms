using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using EasyBudget.Forms.Converters;
using EasyBudget.Forms.Utility;
using EasyBudget.Forms.Utility.ColorUtility;
using Microcharts;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetCategoryEdit : ContentPage
    {
        BudgetCategoryViewModel vm;

        public BudgetCategoryEdit()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            vm = this.BindingContext as BudgetCategoryViewModel;
            //chartCategory.Chart = await ChartUtility.Instance.GetChartAsync(vm);
            //var provider = new MicrochartsProvider<BudgetCategoryViewModel>();
            //chartCategory.Chart = await provider.GetChartAsync(vm, 0, false);

        }

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
            vm = null;
		}

		protected async void OnSaveClicked(object sender, EventArgs e)
        {
            await (this.BindingContext as BudgetCategoryViewModel).SaveChangesAsync();
            await Navigation.PopAsync();
        }

        protected async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected  void CategoryTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected async void btnPickColor_Clicked(object sender, EventArgs e)
        {
            var pkr = new Utility.ColorUtility.ColorFamilyPickListPage();
            await Navigation.PushAsync(pkr);
        }

        protected async void OnItemColorTapped(object sender, TappedEventArgs e)
        {
            var pkr = new Utility.ColorUtility.ColorFamilyPickListPage();
            pkr.OnItemColorSelected += CategoryItemColorSelected;

            await Navigation.PushAsync(pkr);
        }

        protected async void CategoryItemColorSelected(object sender, ItemColorSelectedEventArgs e)
        {
            (sender as ColorFamilyPickListPage).OnItemColorSelected -= CategoryItemColorSelected;
            //await Navigation.PopAsync();

            vm.ColorCode = e.colorCode;
            await vm.SaveChangesAsync();
        }
    }
}
