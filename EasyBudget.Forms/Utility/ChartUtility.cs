using System;
using System.Collections.Generic;
using System.Linq;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;
using SkiaSharp;
using Entry = Microcharts.Entry;

namespace EasyBudget.Forms.Utility
{
    public sealed class ChartUtility
    {
        public static ChartUtility Instance { get; private set; }

        Random rnd;

        public ChartUtility()
        {
            if (Instance != null)
            {
                throw new Exception("Only one instance of ChartUtility is allowed!");
            }
            else
            {
                Instance = this;
                RandomGenerator generator = new RandomGenerator();
                this.rnd = generator.GetRandom(null);
            }
        }

        public SKColor GetColor()
        {
            return ChartColors.Instance.GetColor();
        }

        public Entry[] GetEntries(EasyBudgetStatusViewModel vm)
        {
            Entry[] IncomeEntries = GetIncomeEntries(vm);
            Entry[] ExpensesEntries = GetExpensesEntries(vm);
            Entry[] _Entries = new Entry[IncomeEntries.Length + ExpensesEntries.Length];
            Array.Copy(IncomeEntries, _Entries, IncomeEntries.Length);
            Array.Copy(ExpensesEntries, 0, _Entries, IncomeEntries.Length, ExpensesEntries.Length);

            return _Entries;

        }

        public Entry[] GetEntries(BudgetCategoriesViewModel vm)
        {
            return GetCategoriesEntries(vm);
        }

        public Entry[] GetEntries(BudgetCategoryViewModel vm)
        {
            return GetCategoryEntries(vm);
        }

        //public Entry[] GetEntries(BankAccountsViewModel vm)
        //{

        //}

        #region Helper Methods

        Entry[] GetIncomeEntries(EasyBudgetStatusViewModel vm)
        {
            var context = vm;

            List<Entry> entries = new List<Entry>();

            var fltIncome = (float)context.vmCategories.BudgetCategories
                                          .Where(c => c.CategoryType == BudgetCategoryType.Income)
                                          .Sum(c => c.Amount);

            var fltIncomeActual = (float)context.vmAccounts.BankAccounts
                                                .Sum(bk => bk.AccountRegister
                                                     .Where(ar => ar.ItemType == AccountRegisterItemViewModel.AccountItemType.Deposits)
                                                     .Sum(d => d.ItemAmount));

            Entry _entryBudget = new Entry(fltIncome)
            {
                Label = "Budgeted",
                ValueLabel = fltIncome.ToString("C"),
                Color = SKColors.Green
            };
            entries.Add(_entryBudget);
            Entry _entryActual = new Entry(fltIncomeActual)
            {
                Label = "Actual",
                ValueLabel = fltIncomeActual.ToString("C"),
                Color = SKColors.LightGreen
            };
            entries.Add(_entryActual);
            return entries.ToArray();

        }

        Entry[] GetExpensesEntries(EasyBudgetStatusViewModel vm)
        {
            var context = vm;

            List<Entry> entries = new List<Entry>();
            var fltExpenses = (float)context.vmCategories.BudgetCategories
                                            .Where(c => c.CategoryType == Models.BudgetCategoryType.Expense)
                                            .Sum(c => c.Amount);
            var fltExpenseActual = (float)context.vmAccounts.BankAccounts
                                                 .Sum(bk => bk.AccountRegister
                                                      .Where(ar => ar.ItemType == AccountRegisterItemViewModel.AccountItemType.Withdrawals)
                                                      .Sum(d => d.ItemAmount));
            Entry _entryBudget = new Entry(fltExpenses)
            {
                Label = "Budgeted",
                ValueLabel = fltExpenses.ToString("C"),
                Color = SKColors.Salmon
            };
            entries.Add(_entryBudget);
            Entry _entryActual = new Entry(fltExpenseActual)
            {
                Label = "Actual",
                ValueLabel = fltExpenseActual.ToString("C"),
                Color = SKColors.LightSalmon
            };
            entries.Add(_entryActual);
            return entries.ToArray();
        }

        Entry[] GetCategoriesEntries(BudgetCategoriesViewModel vm)
        {
            var context = vm.BudgetCategories;

            List<Entry> entries = new List<Entry>();

            var fltCatExpenseSum = (float)context.Where(c => c.CategoryType == BudgetCategoryType.Expense).Sum(c => c.Amount);
            var fltCatIncomeSum = (float)context.Where(c => c.CategoryType == BudgetCategoryType.Income).Sum(c => c.Amount);


            Entry _entryCatExpenseSum = new Entry(fltCatExpenseSum)
            {
                Label = "Expenses",
                ValueLabel = fltCatExpenseSum.ToString("C"),
                Color = SKColors.IndianRed
            };
            entries.Add(_entryCatExpenseSum);

            Entry _entryCatIncome = new Entry(fltCatIncomeSum)
            {
                Label = "Income",
                ValueLabel = fltCatIncomeSum.ToString("C"),
                Color = SKColors.Green
            };
            entries.Add(_entryCatIncome);
            return entries.ToArray();
        }

        Entry[] GetCategoryEntries(BudgetCategoryViewModel vm)
        {
            var context = vm.BudgetItems;

            List<Entry> entries = new List<Entry>();

            foreach(var itm in context)
            {
                Entry _entryItmIncome = new Entry((float)itm.BudgetedAmount)
                {
                    Label = string.Empty,
                    ValueLabel = itm.BudgetedAmount.ToString("C"),
                    Color = ChartColors.Instance.GetColor()
                };
                entries.Add(_entryItmIncome);
            }

            return entries.ToArray();
        }

        #endregion
    }
}
