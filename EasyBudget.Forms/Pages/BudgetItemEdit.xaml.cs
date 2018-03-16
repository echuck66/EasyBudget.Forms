using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using EasyBudget.Forms.Utility.ColorUtility;
using EasyBudget.Models.DataModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetItemEdit : ContentPage
    {
        BudgetItemViewModel vm;

        public BudgetItemEdit()
        {
            InitializeComponent();
        }

		protected async override void OnAppearing()
		{
			base.OnAppearing();
            vm = this.BindingContext as BudgetItemViewModel;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
            vm = null;
		}

		protected async void OnSaveClicked(object sender, EventArgs e)
        {
            await (this.BindingContext as BudgetItemViewModel).SaveChangesAsync();
            await Navigation.PopAsync();
        }

        protected async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
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
