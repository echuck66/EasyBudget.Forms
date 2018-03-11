using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using EasyBudget.Models.DataModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetItemView : ContentPage
    {

        public BudgetItemView()
        {
            InitializeComponent();
        }

        public async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
