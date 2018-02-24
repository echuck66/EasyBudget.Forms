using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class CheckingAccountRegister : ContentPage
    {
        public CheckingAccountRegister()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            (this.BindingContext as BankAccountViewModel).SelectedRegisterItem = null;
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
                    CheckingDepositEdit depEditor = new CheckingDepositEdit();
                    depEditor.BindingContext = regItem as CheckingDepositViewModel;
                    await Navigation.PushModalAsync(depEditor);
                    break;
                case AccountRegisterItemViewModel.AccountItemType.Withdrawals:
                    CheckingWithdrawalEdit witEditor = new CheckingWithdrawalEdit();
                    witEditor.BindingContext = regItem as CheckingWithdrawalViewModel;
                    await Navigation.PushModalAsync(witEditor);
                    break;
            }
        }

        protected async void OnItemDelete(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirmation", "Are you sure you want to delete this item?", "Yes", "No");

            if (answer)
            {
                var btn = sender as MenuItem;
                var regItem = btn.BindingContext as AccountRegisterItemViewModel;

                bool deleted = false;
                //await vm.DeleteBudgetCategoryAsync(category);
                switch(regItem.ItemType)
                {
                    case AccountRegisterItemViewModel.AccountItemType.Deposits:
                        //(regItem as DepositViewModel).

                        break;
                    case AccountRegisterItemViewModel.AccountItemType.Withdrawals:

                        break;
                }
                if (deleted)
                {
                    await DisplayAlert("Results", "Item Deleted", "Dismiss");
                }
                else
                {
                    await DisplayAlert("Error", "Unable to delte this Category. Message: " + regItem.ErrorCondition, "Ok");
                }
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
