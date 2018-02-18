using System;
using System.Threading.Tasks;
using System.ComponentModel;
using EasyBudget.Models.DataModels;
using System.Collections.Generic;

namespace EasyBudget.Business.ViewModels
{
    public class CheckingDepositViewModel : DepositViewModel, INotifyPropertyChanged
    {
        CheckingDeposit model { get; set; }

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

        public CheckingDepositViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.BudgetCategories = new List<BudgetCategory>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void PopulateVM(CheckingDeposit deposit)
        {
            this.model = deposit;
            this.accountType = Models.BankAccountType.Checking;
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = Task.Run(() => uow.GetAllBudgetCategoriesAsync()).Result;
                if (_results.Successful)
                {
                    foreach (BudgetCategory category in _results.Results)
                    {
                        if (category.categoryType == Models.BudgetCategoryType.Income)
                        {
                            this.BudgetCategories.Add(category);
                        }
                    }
                }
            }
        }

        public async Task PopulateVMAsync(CheckingDeposit deposit)
        {
            this.model = deposit;
            this.accountType = Models.BankAccountType.Checking;
            this.accountId = deposit.checkingAccountId;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllBudgetCategoriesAsync();
                if (_results.Successful)
                {
                    foreach(BudgetCategory category in _results.Results)
                    {
                        if (category.categoryType == Models.BudgetCategoryType.Income)
                        {
                            this.BudgetCategories.Add(category);
                        }
                    }
                }
            }
        }
    
        public async override Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
