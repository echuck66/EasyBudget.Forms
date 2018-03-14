using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using EasyBudget.Business.ChartModels;
using EasyBudget.Models;

namespace EasyBudget.Business.ViewModels
{
    public class EasyBudgetStatusViewModel : BaseViewModel, INotifyPropertyChanged
    {
        int _SelectedMonth;
        public int SelectedMonth 
        {
            get 
            {
                return _SelectedMonth;
            }
            set
            {
                if (_SelectedMonth != value)
                {
                    _SelectedMonth = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMonth)));
                }
            }
        }

        BankAccountsViewModel _vmAccounts;
        public BankAccountsViewModel vmAccounts 
        {
            get
            {
                return _vmAccounts;
            }
            set
            {
                _vmAccounts = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(vmAccounts)));
            }
        }

        BudgetCategoriesViewModel _vmCategories;
        public BudgetCategoriesViewModel vmCategories
        {
            get
            {
                return _vmCategories;
            }
            set
            {
                _vmCategories = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(vmCategories)));
            }
        }

        BudgetCategoryViewModel _SelectedCategory;
        public BudgetCategoryViewModel SelectedCategory
        {
            get
            {
                return _SelectedCategory;
            }
            set
            {
                _SelectedCategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCategory)));
            }

        }

        public EasyBudgetStatusViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        public async Task LoadVMAsync()
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _resultsBanking = await EasyBudgetDataService.Instance.GetBankAccountsViewModelAsync();
                var _resultsCategories = await EasyBudgetDataService.Instance.GetBudgetCategoriesViewModelAsync();

                if (_resultsCategories != null)
                {
                    this.vmCategories = _resultsCategories;
                }
                else
                {
                    // TODO Something went wrong with Categories
                }

                if (_resultsBanking != null){
                    this.vmAccounts = _resultsBanking;
                }
                else
                {
                    // TODO Something went wrong with Banking
                }
            }
        }

        public override IChartDataPack GetChartData()
        {
            var dataPack = new ChartDataPack();
            // Income Group Chart Data
            var budgetIncomeGroup = new ChartDataGroup();
            budgetIncomeGroup.ChartDisplayType = ChartType.Pie;
            budgetIncomeGroup.ChartDisplayOrder = 0;
            // Construct and build the data points
            // Collect the income values
            var fltIncome = (float)this.vmCategories.BudgetCategories
                                          .Where(c => c.CategoryType == BudgetCategoryType.Income)
                                          .Sum(c => c.Amount);
            var fltIncomeActual = (float)this.vmAccounts.BankAccounts
                                                .Sum(bk => bk.AccountRegister
                                                     .Where(ar => ar.ItemType == AccountRegisterItemViewModel.AccountItemType.Deposits)
                                                     .Sum(d => d.ItemAmount));
            string incomeBudgetColor = "#4CAF50";
            string incomeActualColor = "#81C784";

            //if (fltIncomeActual >= fltIncome)
                //incomeActualColor = "#E0E0E0";

            float fltIncomeValue = fltIncome > fltIncomeActual ? fltIncome - fltIncomeActual : 0;

            var budgetIncomeEntry = new ChartDataEntry()
            {
                FltValue = fltIncomeValue,
                Label = "Budgeted",
                ValueLabel = fltIncome.ToString("C"),
                ColorCode = incomeBudgetColor
            };
            var actualIncomeEntry = new ChartDataEntry()
            {
                FltValue = fltIncomeActual,
                Label = "Actual",
                ValueLabel = fltIncomeActual.ToString("C"),
                ColorCode = incomeActualColor
            };
            budgetIncomeGroup.ChartDataItems.Add(actualIncomeEntry);
            budgetIncomeGroup.ChartDataItems.Add(budgetIncomeEntry);

            // Expense Group Chart Data
            var budgetExpenseGroup = new ChartDataGroup();
            budgetExpenseGroup.ChartDataItems = new List<ChartDataEntry>();
            budgetExpenseGroup.ChartDisplayType = ChartType.Pie;
            budgetExpenseGroup.ChartDisplayOrder = 1;
            // Construct and build the data points
            // Collect the expense values
            var fltExpenses = (float)this.vmCategories.BudgetCategories
                                            .Where(c => c.CategoryType == Models.BudgetCategoryType.Expense)
                                            .Sum(c => c.Amount);
            var fltExpenseActual = (float)this.vmAccounts.BankAccounts
                                                 .Sum(bk => bk.AccountRegister
                                                      .Where(ar => ar.ItemType == AccountRegisterItemViewModel.AccountItemType.Withdrawals)
                                                      .Sum(d => d.ItemAmount));
            //string expenseBudgetColor = "#F44336";
            string expenseBudgetColor = "#F48FB1";
            string expenseActualColor = "#81C784";

            if (fltExpenseActual >= fltExpenses)
                expenseActualColor = "#F44336";
                //expenseActualColor = "#E57373";

            float fltExpenseValue = fltExpenses > fltExpenseActual ? fltExpenses - fltExpenseActual : 0;
            var budgetExpenseEntry = new ChartDataEntry()
            {
                FltValue = fltExpenseValue,
                Label = "Budgeted",
                ValueLabel = fltExpenses.ToString("C"),
                ColorCode = expenseBudgetColor
            };
            var actualExpenseEntry = new ChartDataEntry()
            {
                FltValue = fltExpenseActual,
                Label = "Actual",
                ValueLabel = fltExpenseActual.ToString("C"),
                ColorCode = expenseActualColor
            };
            budgetExpenseGroup.ChartDataItems.Add(actualExpenseEntry);
            budgetExpenseGroup.ChartDataItems.Add(budgetExpenseEntry);

            dataPack.Charts.Add(budgetIncomeGroup);
            dataPack.Charts.Add(budgetExpenseGroup);

            //var accountsStatusGroup = new ChartDataGroup();
            //accountsStatusGroup.ChartDataItems = new List<ChartDataEntry>();
            //accountsStatusGroup.ChartDisplayType = ChartType.Line;
            //accountsStatusGroup.ChartDisplayOrder = 1;
            // Construct and build the data points

            //dataPack.Charts.Add(accountsStatusGroup);

            return dataPack;
        }

    }
}
