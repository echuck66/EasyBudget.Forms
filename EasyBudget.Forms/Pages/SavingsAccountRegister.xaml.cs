using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class SavingsAccountRegister : ContentPage
    {
        public SavingsAccountRegister()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (this.BindingContext as BankAccountViewModel).SelectedRegisterItem = e.SelectedItem as AccountRegisterItemViewModel;
        }

        protected async void OnItemEdit(object sender, EventArgs e)
        {
            var btn = sender as MenuItem;
            var regItem = btn.BindingContext as AccountRegisterItemViewModel;

            switch (regItem.ItemType)
            {
                case AccountRegisterItemViewModel.AccountItemType.Deposits:
                    SavingsDepositEdit depEditor = new SavingsDepositEdit();
                    depEditor.BindingContext = regItem as SavingsDepositViewModel;
                    await Navigation.PushModalAsync(depEditor);
                    break;
                case AccountRegisterItemViewModel.AccountItemType.Withdrawals:
                    SavingsWithdrawalEdit witEditor = new SavingsWithdrawalEdit();
                    witEditor.BindingContext = regItem as SavingsWithdrawalViewModel;
                    await Navigation.PushModalAsync(witEditor);
                    break;
            }
        }

        protected async void OnItemDelete(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirmation", "Are you sure you want to delete this item?", "Yes", "No");

            if (answer)
            {
                //var btn = sender as MenuItem;
                //var category = btn.BindingContext as BudgetCategoryViewModel;
                //bool deleted = await vm.DeleteBudgetCategoryAsync(category);
                //if (deleted)
                //{
                //    await DisplayAlert("Results", "Item Deleted", "Dismiss");
                //}
                //else
                //{
                //    await DisplayAlert("Error", "Unable to delte this Category. Message: " + category.ErrorCondition, "Ok");
                //}
            }
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        protected async void OnNewItemClicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("New Item Type", "Cancel", null, "Deposit", "Withdrawal");

            switch (action)
            {
                case "Deposit":

                    break;
                case "Withdrawal":

                    break;
            }
        }

    }
}
