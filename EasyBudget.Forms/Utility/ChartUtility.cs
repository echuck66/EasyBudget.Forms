using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;
using Microcharts;
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

        public Chart GetChart(EasyBudgetStatusViewModel vm)
        {
            var entries = GetEntries(vm);
            return new BarChart() { Entries = entries };
        }

        public Chart GetChart(BudgetCategoriesViewModel vm)
        {
            var entries = GetEntries(vm);
            return new BarChart() { Entries = entries };
        }

        public async Task<Chart> GetChartAsync(BudgetCategoriesViewModel vm)
        {
            var entries = GetEntries(vm);
            return new BarChart() { Entries = entries };
        }

        public Chart GetChart(BudgetCategoryViewModel vm)
        {
            var entries = GetEntries(vm);
            return new DonutChart() { Entries = entries };
        }

        public async Task<Chart> GetChartAsync(BankAccountViewModel vm, AccountRegisterItemViewModel.AccountItemType objectType)
        {
            var entries = await GetEntriesAsync(vm, objectType);
            return new LineChart() { Entries = entries };
        }

        #region Helper Methods
        
        Entry[] GetEntries(EasyBudgetStatusViewModel vm)
        {
            Entry[] IncomeEntries = GetIncomeEntries(vm);
            Entry[] ExpensesEntries = GetExpensesEntries(vm);
            Entry[] _Entries = new Entry[IncomeEntries.Length + ExpensesEntries.Length];
            Array.Copy(IncomeEntries, _Entries, IncomeEntries.Length);
            Array.Copy(ExpensesEntries, 0, _Entries, IncomeEntries.Length, ExpensesEntries.Length);

            return _Entries;

        }

        Entry[] GetEntries(BudgetCategoriesViewModel vm)
        {
            return GetCategoriesEntries(vm);
        }

        async Task<Entry[]> GetEntriesAsync(BudgetCategoriesViewModel vm)
        {
            return await GetCategoriesEntriesAsync(vm);
        }

        Entry[] GetEntries(BudgetCategoryViewModel vm)
        {
            return GetCategoryEntries(vm);
        }

        async Task<Entry[]> GetEntriesAsync(BankAccountViewModel vm, AccountRegisterItemViewModel.AccountItemType objectType)
        {
            return await GetBankAccountEntriesAsync(vm, objectType);
        }

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
                //ValueLabel = fltCatExpenseSum.ToString("C"),
                Color = SKColors.Red
            };
            entries.Add(_entryCatExpenseSum);

            Entry _entryCatIncome = new Entry(fltCatIncomeSum)
            {
                Label = "Income",
                //ValueLabel = fltCatIncomeSum.ToString("C"),
                Color = SKColors.Green
            };
            entries.Add(_entryCatIncome);
            return entries.ToArray();
        }

        async Task<Entry[]> GetCategoriesEntriesAsync(BudgetCategoriesViewModel vm)
        {
            var context = vm.BudgetCategories;

            List<Entry> entries = new List<Entry>();

            var fltCatExpenseSum = (float)context.Where(c => c.CategoryType == BudgetCategoryType.Expense).Sum(c => c.Amount);
            var fltCatIncomeSum = (float)context.Where(c => c.CategoryType == BudgetCategoryType.Income).Sum(c => c.Amount);

            Entry[] _entryArray = new Entry[]
            {
                new Entry(fltCatExpenseSum)
                {
                    Label = "Expenses",
                    //ValueLabel = fltCatExpenseSum.ToString("C"),
                    Color = SKColors.Red
                },
                new Entry(fltCatIncomeSum)
                {
                    Label = "Income",
                    //ValueLabel = fltCatIncomeSum.ToString("C"),
                    Color = SKColors.Green
                }
            };
            var _entries = await BuildEntryCollectionAsync(_entryArray);
            entries.AddRange(_entries.ToList());

            return entries.ToArray();
        }

        async Task<ICollection<Entry>> BuildEntryCollectionAsync(Entry[] entries)
        {
            List<Entry> _entries = await Task.Run(() => new List<Entry>(entries));
            return _entries;
        }

        Entry[] GetCategoryEntries(BudgetCategoryViewModel vm)
        {
            var context = vm.BudgetItems;

            List<Entry> entries = new List<Entry>();

            foreach(var itm in context)
            {
                Entry _entryItmIncome = new Entry((float)itm.BudgetedAmount)
                {
                    //Label = string.Empty,
                    //ValueLabel = itm.BudgetedAmount.ToString("C"),
                    Color = ChartColors.Instance.GetColor()
                };
                entries.Add(_entryItmIncome);
            }

            return entries.ToArray();
        }

        async Task<Entry[]> GetBankAccountEntriesAsync(BankAccountViewModel vm, AccountRegisterItemViewModel.AccountItemType objectType)
        {
            var context = vm;

            List<Entry> entries = new List<Entry>();

            var chartData = await vm.GetChartData();
            foreach (var model in chartData)
            {
                
                if (objectType == model.ItemType && model.ItemType == AccountRegisterItemViewModel.AccountItemType.Deposits)
                {
                    entries.Add(new Entry((float)model.ItemAmount)
                    {
                        Color = SKColors.Green
                    });
                }
                else if (objectType == model.ItemType && model.ItemType == AccountRegisterItemViewModel.AccountItemType.Withdrawals)
                {
                    entries.Add(new Entry((float)model.ItemAmount)
                    {
                        Color = SKColors.Red
                    });
                }
            }

            return entries.ToArray();
        }

        #endregion
    }
}
