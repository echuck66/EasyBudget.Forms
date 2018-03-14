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

		protected override void OnAppearing()
		{
			base.OnAppearing();
            vm = this.BindingContext as CheckingDepositViewModel;
		}

		public async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected async void OnDepositEditTapped(object sender, TappedEventArgs e)
        {
            CheckingDepositEdit editor = new CheckingDepositEdit();
            editor.BindingContext = vm;
            await Navigation.PushAsync(editor);
        }
    }
}
