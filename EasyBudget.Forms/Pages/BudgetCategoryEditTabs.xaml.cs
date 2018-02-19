using System;
using System.Collections.Generic;
using EasyBudget.Business.ViewModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetCategoryEditTabs : TabbedPage
    {
        public BudgetCategoryEditTabs()
        {
            InitializeComponent();
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    var vm = this.BindingContext as BudgetCategoryViewModel;
        //    foreach(var page in this.Children)
        //    {
        //        page.BindingContext = vm;

        //    }
        //}
    }
}
