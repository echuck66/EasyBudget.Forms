using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{
    public class SavingsDepositViewModel : DepositViewModel, INotifyPropertyChanged
    {
        SavingsDeposit model { get; set; }

        public DateTime transactionDate
        {
            get
            {
                return model.transactionDate;
            }
            set
            {
                if (model.transactionDate != value)
                {
                    model.transactionDate = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(transactionDate)));
                }
            }
        }

        public decimal transactionAmount
        {
            get
            {
                return model.transactionAmount;
            }
            set
            {
                if (model.transactionAmount != value)
                {
                    model.transactionAmount = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(transactionAmount)));
                }
            }
        }

        public string description
        {
            get
            {
                return model.description;
            }
            set
            {
                if (model.description != value)
                {
                    model.description = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(description)));
                }
            }
        }

        public string notation
        {
            get
            {
                return model.notation;
            }
            set
            {
                if (model.notation != value)
                {
                    model.notation = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(notation)));
                }
            }
        }

        public int? budgetItemId
        {
            get
            {
                return model.budgetIncomeId;
            }
            set
            {
                if (model.budgetIncomeId != value)
                {
                    model.budgetIncomeId = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(budgetItemId)));
                }
            }
        }

        public bool reconciled
        {
            get
            {
                return model.reconciled;
            }
            set
            {
                if (model.reconciled != value)
                {
                    model.reconciled = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(reconciled)));
                }
            }
        }

        public ICollection<BudgetCategory> BudgetCategories { get; set; }


        public SavingsDepositViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.BudgetCategories = new List<BudgetCategory>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void PopulateVM(SavingsDeposit deposit)
        {
            this.model = deposit;
            this.accountType = Models.BankAccountType.Savings;
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = Task.Run(() => uow.GetAllBudgetCategoriesAsync()).Result;
                if (_results.Successful)
                {
                    foreach (BudgetCategory category in _results.Results)
                    {
                        if (category.categoryType == Models.BudgetCategoryType.Expense)
                        {
                            this.BudgetCategories.Add(category);
                        }
                    }
                }
            }
        }

        public async Task PopulateVMAsync(SavingsDeposit deposit)
        {
            this.model = deposit;
            this.accountType = Models.BankAccountType.Savings;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllBudgetCategoriesAsync();
                if (_results.Successful)
                {
                    foreach (BudgetCategory category in _results.Results)
                    {
                        if (category.categoryType == Models.BudgetCategoryType.Expense)
                        {
                            this.BudgetCategories.Add(category);
                        }
                    }
                }
            }
        }
    }
}
