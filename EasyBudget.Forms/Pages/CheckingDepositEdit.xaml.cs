using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class CheckingDepositEdit : ContentPage
    {
        CheckingDepositViewModel vm;

        public CheckingDepositEdit()
        {
            InitializeComponent();
        }

		protected async override void OnAppearing()
		{
			base.OnAppearing();

            vm = this.BindingContext as CheckingDepositViewModel;
            await vm.LoadBudgetData();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
            vm = null;
		}

		protected async void OnSaveClicked(object sender, EventArgs e)
        {
            bool itemSaved = await vm.SaveChangesAsync();
            if (itemSaved)
            {
                await Navigation.PopAsync();
            }
            else
            {
                // Notify the user that save failed
                string errorMessage = (BindingContext as CheckingDepositViewModel).ErrorCondition;
                await DisplayAlert("Error", errorMessage, "Close");
            }
        }

        protected async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected async void CategoryTypes_SelectedIndexChanged(object sender, EventArgs e)
        {


            //await vm.CategorySelected();

        }

        protected async void BudgetItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            //await vm.LoadBudgetData();
        }
    }
}
