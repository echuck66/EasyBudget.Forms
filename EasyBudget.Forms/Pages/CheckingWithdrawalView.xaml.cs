using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class CheckingWithdrawalView : ContentPage
    {
        CheckingWithdrawalViewModel vm;

        public CheckingWithdrawalView()
        {
            InitializeComponent();
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();
            vm = this.BindingContext as CheckingWithdrawalViewModel;
		}

		public async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected async void OnWithdrawalEditTapped(object sender, TappedEventArgs e)
        {
            CheckingWithdrawalEdit editor = new CheckingWithdrawalEdit();
            await vm.LoadBudgetData();
            editor.BindingContext = vm;
            await Navigation.PushAsync(editor);
        }
    }
}
