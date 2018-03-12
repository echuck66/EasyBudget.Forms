using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class SavingsDepositEdit : ContentPage
    {
        public SavingsDepositEdit()
        {
            InitializeComponent();
        }

        protected async void OnSaveClicked(object sender, EventArgs e)
        {
            //await (this.BindingContext as SavingsDepositViewModel).SaveChangesAsync();
            bool itemSaved = await (this.BindingContext as SavingsDepositViewModel).SaveChangesAsync();
            if (itemSaved)
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                // Notify the user that save failed
                string errorMessage = (BindingContext as SavingsDepositViewModel).ErrorCondition;
                await DisplayAlert("Error", errorMessage, "Close");
            }
            await Navigation.PopModalAsync();
        }

        protected async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected async void CategoryTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            await (BindingContext as SavingsDepositViewModel).OnCategorySelected();
        }
    }
}
