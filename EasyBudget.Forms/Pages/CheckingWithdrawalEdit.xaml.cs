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
            await (this.BindingContext as CheckingWithdrawalViewModel).SaveChangesAsync();
            await Navigation.PopModalAsync();
        }

        protected async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        protected async void CategoryTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            await (BindingContext as CheckingWithdrawalViewModel).OnCategorySelected();
            //pickerBudgetItems.SetBinding(Picker.ItemsSourceProperty, "BudgetItems");
        }
    }
}
