using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;
using Microcharts;
using SkiaSharp;
using Entry = Microcharts.Entry;
using System.Linq;
using EasyBudget.Forms.Utility;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetCategoryView : ContentPage
    {
        Microcharts.Entry[] Entries = null;

        public BudgetCategoryView()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            var entries = GetEntries();
            var chart = new DonutChart() { Entries = entries };

            chartPieData.Chart = chart;
        }

        public async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }


        protected Microcharts.Entry[] GetEntries()
        {
            var context = this.BindingContext as BudgetCategoryViewModel;
            return ChartUtility.Instance.GetEntries(context);
        }



    }
}
