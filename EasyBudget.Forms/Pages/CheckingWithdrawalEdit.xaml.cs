using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class CheckingWithdrawalEdit : ContentPage
    {
        public CheckingWithdrawalEdit()
        {
            InitializeComponent();
        }

        protected async void OnSaveClicked(object sender, EventArgs e)
        {
            bool itemSaved = await (this.BindingContext as CheckingWithdrawalViewModel).SaveChangesAsync();
            if (itemSaved)
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                // Notify the user that save failed
                string errorMessage = (BindingContext as CheckingWithdrawalViewModel).ErrorCondition;
                await DisplayAlert("Error", errorMessage, "Close");
            }
        }

        protected async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        protected async void CategoryTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            await (BindingContext as CheckingWithdrawalViewModel).OnCategorySelected();
        }
    }
}
