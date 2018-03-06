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
            var entries = Entries ?? GetEntries();
            var chart = new DonutChart() { Entries = entries };

            chartPieData.Chart = chart;
        }

        public async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }


        protected Microcharts.Entry[] GetEntries()
        {
            var rnd = new Random();

            var context = this.BindingContext as BudgetCategoryViewModel;

            List<Entry> entries = new List<Entry>();
            foreach(BudgetItemViewModel vm in context.BudgetItems)
            {
                var fltSum = (float)context.BudgetItems.Sum(i => i.BudgetedAmount);
                var fltValue = (float)vm.BudgetedAmount;
                Entry _entry = new Entry(fltValue)
                {
                    Label = vm.ItemDescription,
                    ValueLabel = vm.BudgetedAmount.ToString("C"),
                    Color = ChartUtility.Instance.GetColor()
                };
                entries.Add(_entry);
            }

            return entries.ToArray();
        }



    }
}
