using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetCategoryItems : ContentPage
    {
        public BudgetCategoryItems()
        {
            InitializeComponent();
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

        }

        private async Task PushContentPageModalAsync(ContentPage page)
        {
            // Wrap ContentPage in a NavigationPage to get the toolbar
            NavigationPage np = new NavigationPage(page);
            await Navigation.PushModalAsync(np);
        }
    }
}
