using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class MainPage : ContentPage
    {
        EasyBudgetDataService ds;
        EasyBudgetStatusViewModel vm;

        public MainPage()
        {
            InitializeComponent();
            ds = EasyBudgetDataService.Instance;

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            vm = await ds.GetStatusVM();
        }
    }
}
