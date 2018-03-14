using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class SavingsWithdrawalView : ContentPage
    {
        SavingsWithdrawalViewModel vm;

        public SavingsWithdrawalView()
        {
            InitializeComponent();
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();
            vm = this.BindingContext as SavingsWithdrawalViewModel;
		}

		public async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected async void OnDepositEditTapped(object sender, TappedEventArgs e)
        {
            SavingsWithdrawalEdit editor = new SavingsWithdrawalEdit();
            editor.BindingContext = vm;
            await Navigation.PushAsync(editor);
        }
    }
}
