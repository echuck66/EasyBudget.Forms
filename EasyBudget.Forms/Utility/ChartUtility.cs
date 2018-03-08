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

        //public Chart GetChart(EasyBudgetStatusViewModel vm)
        //{
        //    var entries = GetEntries(vm);
        //    return new BarChart() { Entries = entries };
        //}
        
        //public Chart GetChart(BudgetCategoriesViewModel vm, bool fullSize = false)
        //{
        //    var entries = GetEntries(vm, fullSize);
        //    return new BarChart() { Entries = entries };
        //}
        
        //public Chart GetChart(BudgetCategoryViewModel vm, bool fullSize = false)
        //{
        //    var entries = GetEntries(vm, fullSize);
        //    return new DonutChart() { Entries = entries };
        //}
        
        //public Chart GetChart(BudgetCategoryViewModel vm, CategoryChartType chartType, bool fullSize = false)
        //{
        //    var entries = GetEntries(vm, chartType, fullSize);
        //    return new DonutChart() { Entries = entries };
        //}

        public async Task<Chart> GetChartAsync(EasyBudgetStatusViewModel vm, bool fullSize = false)
        {
            var entries = await GetEntriesAsync(vm, fullSize);
            return new BarChart() { Entries = entries };
        }

        public async Task<Chart> GetChartAsync(BudgetCategoriesViewModel vm, bool fullSize = false)
        {
            var entries = await GetEntriesAsync(vm, fullSize);
            return new BarChart() { Entries = entries };
        }

        public async Task<Chart> GetChartAsync(BudgetCategoryViewModel vm, bool fullSize = false)
        {
            var entries = await GetEntriesAsync(vm, fullSize);
            return new DonutChart() { Entries = entries };
        }

        public async Task<Chart> GetChartAsync(BudgetCategoryViewModel vm, CategoryChartType chartType, bool fullSize = false)
        {
            var entries = await GetEntriesAsync(vm, chartType, fullSize);
            return new DonutChart() { Entries = entries };
        }

        public async Task<Chart> GetChartAsync(BankAccountViewModel vm, AccountRegisterItemViewModel.AccountItemType objectType, bool fullSize = false)
        {
            var entries = await GetEntriesAsync(vm, objectType, fullSize);
            return new LineChart() { Entries = entries };
        }

        public enum CategoryChartType 
        {
            Categories,
            Category
        }

        public enum StatusChartType
        {
            BudgetStatus,
            BankingStatus,
            AccountStatus
        }

        #region Helper Methods

        //Entry[] GetEntries(EasyBudgetStatusViewModel vm, bool fullSize)
        //{
        //    Entry[] IncomeEntries = GetIncomeEntries(vm, fullSize);
        //    Entry[] ExpensesEntries = GetExpensesEntries(vm, fullSize);
        //    Entry[] _Entries = new Entry[IncomeEntries.Length + ExpensesEntries.Length];
        //    Array.Copy(IncomeEntries, _Entries, IncomeEntries.Length);
        //    Array.Copy(ExpensesEntries, 0, _Entries, IncomeEntries.Length, ExpensesEntries.Length);
        //    return _Entries;
        //}
        
        //Entry[] GetIncomeEntries(EasyBudgetStatusViewModel vm, bool fullSize)
        //{
        //    var context = vm;
        //    List<Entry> entries = new List<Entry>();
        //    var fltIncome = (float)context.vmCategories.BudgetCategories
        //                                  .Where(c => c.CategoryType == BudgetCategoryType.Income)
        //                                  .Sum(c => c.Amount);
        //    var fltIncomeActual = (float)context.vmAccounts.BankAccounts
        //                                        .Sum(bk => bk.AccountRegister
        //                                             .Where(ar => ar.ItemType == AccountRegisterItemViewModel.AccountItemType.Deposits)
        //                                             .Sum(d => d.ItemAmount));
        //    if (fullSize)
        //    {
        //        entries.Add(EntryUtility.GetEntry(fltIncome, SKColors.LightGreen, "Budgeted", fltIncome.ToString("C")));
        //        entries.Add(EntryUtility.GetEntry(fltIncomeActual, SKColors.Green, "Actual", fltIncomeActual.ToString("C")));
        //    }
        //    else
        //    {
        //        entries.Add(EntryUtility.GetEntry(fltIncome, SKColors.LightGreen, "Budgeted"));
        //        entries.Add(EntryUtility.GetEntry(fltIncomeActual, SKColors.Green, "Actual"));
        //    }
        //    return entries.ToArray();
        //}
        
        //Entry[] GetExpensesEntries(EasyBudgetStatusViewModel vm, bool fullSize)
        //{
        //    var context = vm;
        //    List<Entry> entries = new List<Entry>();
        //    var fltExpenses = (float)context.vmCategories.BudgetCategories
        //                                    .Where(c => c.CategoryType == Models.BudgetCategoryType.Expense)
        //                                    .Sum(c => c.Amount);
        //    var fltExpenseActual = (float)context.vmAccounts.BankAccounts
        //                                         .Sum(bk => bk.AccountRegister
        //                                              .Where(ar => ar.ItemType == AccountRegisterItemViewModel.AccountItemType.Withdrawals)
        //                                              .Sum(d => d.ItemAmount));
        //    if (fullSize)
        //    {
        //        entries.Add(EntryUtility.GetEntry(fltExpenses, SKColors.Pink, "Budgeted", fltExpenses.ToString("C")));
        //        entries.Add(EntryUtility.GetEntry(fltExpenseActual, SKColors.Red, "Actual", fltExpenseActual.ToString("C")));
        //    }
        //    else
        //    {
        //        entries.Add(EntryUtility.GetEntry(fltExpenses, SKColors.Pink, "Budgeted"));
        //        entries.Add(EntryUtility.GetEntry(fltExpenseActual, SKColors.Red, "Actual"));
        //    }
        //    return entries.ToArray();
        //}
        
        //Entry[] GetEntries(BudgetCategoriesViewModel vm, bool fullSize)
        //{
        //    return GetCategoriesEntries(vm, fullSize);
        //}
        
        //Entry[] GetCategoriesEntries(BudgetCategoriesViewModel vm, bool fullSize)
        //{
        //    var context = vm.BudgetCategories;
        //    List<Entry> entries = new List<Entry>();
        //    var fltCatExpenseSum = (float)context.Where(c => c.CategoryType == BudgetCategoryType.Expense).Sum(c => c.Amount);
        //    var fltCatIncomeSum = (float)context.Where(c => c.CategoryType == BudgetCategoryType.Income).Sum(c => c.Amount);
        //    if (fullSize)
        //    {
        //        entries.Add(EntryUtility.GetEntry(fltCatExpenseSum, SKColors.Red, "Expenses", fltCatExpenseSum.ToString("C")));
        //        entries.Add(EntryUtility.GetEntry(fltCatIncomeSum, SKColors.Green, "Income", fltCatIncomeSum.ToString("C")));
        //    }
        //    else
        //    {
        //        entries.Add(EntryUtility.GetEntry(fltCatExpenseSum, SKColors.Red, "Expenses"));
        //        entries.Add(EntryUtility.GetEntry(fltCatIncomeSum, SKColors.Green, "Income"));
        //    }
        //    return entries.ToArray();
        //}
      
        //Entry[] GetEntries(BudgetCategoryViewModel vm, bool fullSize)
        //{
        //    return GetCategoryEntries(vm, fullSize);
        //}
        
        //Entry[] GetCategoryEntries(BudgetCategoryViewModel vm, bool fullSize)
        //{
        //    var context = vm.BudgetItems;
        //    List<Entry> entries = new List<Entry>();
        //    foreach (var itm in context)
        //    {
        //        if (fullSize)
        //        {
        //            entries.Add(EntryUtility.GetEntry((float)itm.BudgetedAmount, GetColor(), itm.ItemDescription, itm.BudgetedAmount.ToString("C")));
        //        }
        //        else
        //        {
        //            entries.Add(EntryUtility.GetEntry((float)itm.BudgetedAmount));
        //        }
        //    }
        //    return entries.ToArray();
        //}

        //Entry[] GetEntries(BudgetCategoryViewModel vm, CategoryChartType chartType, bool fullSize)
        //{
        //    return GetCategoryEntries(vm, chartType, fullSize);
        //}
        
        //Entry[] GetCategoryEntries(BudgetCategoryViewModel vm, CategoryChartType chartType, bool fullSize)
        //{
        //    var context = vm.BudgetItems;
        //    List<Entry> entries = new List<Entry>();
        //    foreach (var itm in context)
        //    {
        //        if (fullSize)
        //        {
        //            entries.Add(EntryUtility.GetEntry((float)itm.BudgetedAmount, GetColor(), itm.ItemDescription, itm.BudgetedAmount.ToString("C")));
        //        }
        //        else
        //        {
        //            entries.Add(EntryUtility.GetEntry((float)itm.BudgetedAmount));
        //        }
        //    }
        //    return entries.ToArray();
        //}

        async Task<Entry[]> GetEntriesAsync(EasyBudgetStatusViewModel vm, bool fullSize)
        {
            Entry[] IncomeEntries = await GetIncomeEntriesAsync(vm, fullSize);
            Entry[] ExpensesEntries = await GetExpensesEntriesAsync(vm, fullSize);
            Entry[] _Entries = new Entry[IncomeEntries.Length + ExpensesEntries.Length];
            Array.Copy(IncomeEntries, _Entries, IncomeEntries.Length);
            Array.Copy(ExpensesEntries, 0, _Entries, IncomeEntries.Length, ExpensesEntries.Length);

            return _Entries;

        }

        async Task<Entry[]> GetIncomeEntriesAsync(EasyBudgetStatusViewModel vm, bool fullSize)
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
            Entry[] _entryArray;
            if (fullSize)
            {
                _entryArray = new Entry[]
                {
                    EntryUtility.GetEntry(fltIncome, SKColors.LightGreen, "Budgeted", fltIncome.ToString("C")),
                    EntryUtility.GetEntry(fltIncomeActual, SKColors.Green, "Actual", fltIncomeActual.ToString("C"))
                };
            }
            else
            {
                _entryArray = new Entry[]
                {
                    EntryUtility.GetEntry(fltIncome, SKColors.LightGreen, "Budgeted"),
                    EntryUtility.GetEntry(fltIncomeActual, SKColors.Green, "Actual")
                };
            }

            var _entries = await BuildEntryCollectionAsync(_entryArray);
            entries.AddRange(_entries);

            return entries.ToArray();

        }

        async Task<Entry[]> GetExpensesEntriesAsync(EasyBudgetStatusViewModel vm, bool fullSize)
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

            Entry[] _entryArray;


            if (fullSize)
            {
                _entryArray = new Entry[] 
                {
                    EntryUtility.GetEntry(fltExpenses, SKColors.Pink, "Budgeted", fltExpenses.ToString("C")),
                    EntryUtility.GetEntry(fltExpenseActual, SKColors.Red, "Actual", fltExpenseActual.ToString("C"))
                };
            }
            else
            {
                _entryArray = new Entry[]
                {
                    EntryUtility.GetEntry(fltExpenses, SKColors.Pink, "Budgeted"),
                    EntryUtility.GetEntry(fltExpenseActual, SKColors.Red, "Actual")
                };
            }

            var _entries = await BuildEntryCollectionAsync(_entryArray);
            entries.AddRange(_entries);

            return entries.ToArray();
        }

        async Task<Entry[]> GetEntriesAsync(BudgetCategoriesViewModel vm, bool fullSize)
        {
            return await GetCategoriesEntriesAsync(vm, fullSize);
        }
        
        async Task<Entry[]> GetCategoriesEntriesAsync(BudgetCategoriesViewModel vm, bool fullSize)
        {
            var context = vm.BudgetCategories;

            List<Entry> entries = new List<Entry>();

            var fltCatExpenseSum = (float)context.Where(c => c.CategoryType == BudgetCategoryType.Expense).Sum(c => c.Amount);
            var fltCatIncomeSum = (float)context.Where(c => c.CategoryType == BudgetCategoryType.Income).Sum(c => c.Amount);

            Entry[] _entryArray;
            if (fullSize)
            {
                _entryArray = new Entry[]
                {
                    EntryUtility.GetEntry(fltCatExpenseSum, SKColors.Red, "Expenses", fltCatExpenseSum.ToString("C")),
                    EntryUtility.GetEntry(fltCatIncomeSum, SKColors.Green, "Income", fltCatIncomeSum.ToString("C"))
                };
            }
            else
            {
                _entryArray = new Entry[]
                {
                    EntryUtility.GetEntry(fltCatExpenseSum, SKColors.Red, "Expenses"),
                    EntryUtility.GetEntry(fltCatIncomeSum, SKColors.Green, "Income")
                };
            }

            var _entries = await BuildEntryCollectionAsync(_entryArray);
            entries.AddRange(_entries.ToList());

            return entries.ToArray();
        }

        async Task<Entry[]> GetEntriesAsync(BudgetCategoryViewModel vm, bool fullSize)
        {
            return await GetCategoryEntriesAsync(vm, fullSize);
        }

        async Task<Entry[]> GetCategoryEntriesAsync(BudgetCategoryViewModel vm, bool fullSize)
        {
            var context = vm.BudgetItems;

            List<Entry> entries = new List<Entry>();

            foreach (var itm in context)
            {
                if (fullSize)
                {
                    await Task.Run(() => entries.Add(EntryUtility.GetEntry((float)itm.BudgetedAmount, GetColor(), itm.ItemDescription, itm.BudgetedAmount.ToString("C"))));
                }
                else
                {
                    await Task.Run(() => entries.Add(EntryUtility.GetEntry((float)itm.BudgetedAmount)));
                }
            }

            return entries.ToArray();
        }

        async Task<Entry[]> GetEntriesAsync(BudgetCategoryViewModel vm, CategoryChartType chartType, bool fullSize)
        {
            return await GetCategoryEntriesAsync(vm, chartType, fullSize);
        }

        async Task<Entry[]> GetCategoryEntriesAsync(BudgetCategoryViewModel vm, CategoryChartType chartType, bool fullSize)
        {
            var context = vm.BudgetItems;

            List<Entry> entries = new List<Entry>();

            foreach (var itm in context)
            {
                if (fullSize)
                {
                    await Task.Run(() => entries.Add(EntryUtility.GetEntry((float)itm.BudgetedAmount, GetColor(), itm.ItemDescription, itm.BudgetedAmount.ToString("C"))));
                }
                else
                {
                    await Task.Run(() => entries.Add(EntryUtility.GetEntry((float)itm.BudgetedAmount)));
                }

            }

            return entries.ToArray();
        }

        async Task<Entry[]> GetEntriesAsync(BankAccountViewModel vm, AccountRegisterItemViewModel.AccountItemType objectType, bool fullSize)
        {
            return await GetBankAccountEntriesAsync(vm, objectType, fullSize);
        }

        async Task<Entry[]> GetBankAccountEntriesAsync(BankAccountViewModel vm, AccountRegisterItemViewModel.AccountItemType objectType, bool fullSize)
        {
            var context = vm;

            List<Entry> entries = new List<Entry>();

            var chartData = await vm.GetChartData();
            foreach (var model in chartData)
            {

                if (objectType == model.ItemType && model.ItemType == AccountRegisterItemViewModel.AccountItemType.Deposits)
                {
                    if (fullSize)
                    {
                        entries.Add(EntryUtility.GetEntry((float)model.ItemAmount, SKColors.Green, model.ItemDescription, model.ItemAmount.ToString("C")));
                    }
                    else
                    {
                        entries.Add(EntryUtility.GetEntry((float)model.ItemAmount, SKColors.Green));
                    }
                }
                else if (objectType == model.ItemType && model.ItemType == AccountRegisterItemViewModel.AccountItemType.Withdrawals)
                {
                    entries.Add(EntryUtility.GetEntry((float)model.ItemAmount, SKColors.Red));
                }
            }

            return entries.ToArray();
        }

        async Task<ICollection<Entry>> BuildEntryCollectionAsync(Entry[] entries)
        {
            List<Entry> _entries = await Task.Run(() => new List<Entry>(entries));
            return _entries;
        }

        #endregion
    }
}
