using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class CheckingDepositEdit : ContentPage
    {
        public CheckingDepositEdit()
        {
            InitializeComponent();
        }

        protected async void OnSaveClicked(object sender, EventArgs e)
        {
            bool itemSaved = await (this.BindingContext as CheckingDepositViewModel).SaveChangesAsync();
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
            await (BindingContext as CheckingDepositViewModel).CategorySelected();
        }
    }
}
