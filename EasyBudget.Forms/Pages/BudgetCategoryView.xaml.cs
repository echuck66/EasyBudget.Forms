using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;
using Microcharts;
using SkiaSharp;
using Entry = Microcharts.Entry;
using System.Linq;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetCategoryView : ContentPage
    {
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
            List<Entry> entries = new List<Entry>();
            foreach(BudgetItemViewModel vm in context.BudgetItems)
            {
                var fltSum = (float)context.BudgetItems.Sum(i => i.BudgetedAmount);
                //var fltValue = fltSum > 0 ? (float)vm.BudgetedAmount / fltSum : 0;
                var fltValue = (float)vm.BudgetedAmount;
                Entry _entry = new Entry(fltValue)
                {
                    Label = vm.ItemDescription,
                    ValueLabel = vm.BudgetedAmount.ToString("C"),
                    Color = GetColor(context.BudgetItems.IndexOf(vm))
                };
                entries.Add(_entry);
            }

            return entries.ToArray();
        }

        private SKColor GetColor(int idx)
        {
            if (idx < 11)
            {
                return Colors[idx];
            }
            else
            {
                return Colors[0];
            }
        }

        public static readonly SKColor[] Colors =
        {
                SKColor.Parse("#266489"),
                SKColor.Parse("#68B9C0"),
                SKColor.Parse("#90D585"),
                SKColor.Parse("#F3C151"),
                SKColor.Parse("#F37F64"),
                SKColor.Parse("#424856"),
                SKColor.Parse("#8F97A4"),
                SKColor.Parse("#DAC096"),
                SKColor.Parse("#76846E"),
                SKColor.Parse("#DABFAF"),
                SKColor.Parse("#A65B69"),
                SKColor.Parse("#97A69D"),
        };
    }
}
