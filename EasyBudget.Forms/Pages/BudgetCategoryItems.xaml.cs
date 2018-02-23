using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyBudget.Business.ViewModels;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetCategoryItems : ContentPage
    {
        public BudgetCategoryItems()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        protected async void OnItemEdit(object sender, EventArgs e)
        {

        }

        protected async void OnItemDelete(object sender, EventArgs e)
        {

        }

        protected async void OnNewItemClicked(object sender, EventArgs e)
        {
            BudgetItemEdit editor = new BudgetItemEdit();
            BudgetItemViewModel newItem = await (this.BindingContext as BudgetCategoryViewModel).AddBudgetItemAsync();
            editor.BindingContext = newItem;
            await Navigation.PushModalAsync(editor);
        }

    }
}
