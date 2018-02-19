using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using EasyBudget.Models.DataModels;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class BudgetItemEdit : ContentPage
    {
        EasyBudgetDataService ds;
        BudgetItemViewModel vm;
        int budgetItemId;
        BudgetItemType budgetItemType;

        public BudgetItemEdit(int itemId, BudgetItemType itemType)
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
