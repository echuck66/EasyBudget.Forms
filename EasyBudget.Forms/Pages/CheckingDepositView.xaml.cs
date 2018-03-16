using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class CheckingDepositView : ContentPage
    {
        CheckingDepositViewModel vm;

        public CheckingDepositView()
        {
            InitializeComponent();
        }

		protected async override void OnAppearing()
		{
			base.OnAppearing();
            vm = this.BindingContext as CheckingDepositViewModel;
            await vm.LoadBudgetData();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
            vm = null;
		}

		public async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected async void OnDepositEditTapped(object sender, TappedEventArgs e)
        {
            CheckingDepositEdit editor = new CheckingDepositEdit();
            await vm.LoadBudgetData();

            editor.BindingContext = vm;
            await Navigation.PushAsync(editor);
        }
    }
}
