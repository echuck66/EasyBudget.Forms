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
            await (this.BindingContext as SavingsWithdrawalViewModel).SaveChangesAsync();
            await Navigation.PopModalAsync();
        }

        protected async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        protected async void CategoryTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            await (BindingContext as SavingsWithdrawalViewModel).OnCategorySelected();
            //pickerBudgetItems.SetBinding(Picker.ItemsSourceProperty, "BudgetItems");
        }
    }
}
