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
        EasyBudgetDataService ds;
        BudgetItemViewModel vm;
        int budgetItemId;
        BudgetItemType budgetItemType;

        public BudgetItemView(int itemId, BudgetItemType itemType)
        {
            InitializeComponent();
            ds = EasyBudgetDataService.Instance;
            budgetItemId = itemId;
            budgetItemType = itemType;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            vm = await ds.GetBudgetItemVM(budgetItemId, budgetItemType);
        }
    }
}
