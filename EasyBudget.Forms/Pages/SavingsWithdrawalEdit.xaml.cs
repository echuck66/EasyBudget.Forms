using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class SavingsWithdrawalEdit : ContentPage
    {
        public SavingsWithdrawalEdit()
        {
            InitializeComponent();
        }

        protected async void OnSaveClicked(object sender, EventArgs e)
        {
            //await (this.BindingContext as SavingsWithdrawalViewModel).SaveChangesAsync();
            bool itemSaved = await (this.BindingContext as SavingsWithdrawalViewModel).SaveChangesAsync();
            if (itemSaved)
            {
                await Navigation.PopAsync();
            }
            else
            {
                // Notify the user that save failed
                string errorMessage = (BindingContext as SavingsWithdrawalViewModel).ErrorCondition;
                await DisplayAlert("Error", errorMessage, "Close");
            }
            await Navigation.PopAsync();
        }

        protected async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected async void CategoryTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            await (BindingContext as SavingsWithdrawalViewModel).CategorySelected();
        }
    }
}
