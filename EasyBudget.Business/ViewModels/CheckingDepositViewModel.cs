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
                    this.ItemDate = value;
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
                    this.ItemAmount = value;
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
                    this.ItemDescription = value;
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

        public CheckingDepositViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        public void PopulateVM(CheckingDeposit deposit)
        {
            this.model = deposit;
            this.accountModel = deposit.checkingAccount;
            this.ItemDescription = this.model.description;
            this.ItemType = AccountItemType.Withdrawal;
            this.ItemDate = model.transactionDate;
            this.ItemAmount = model.transactionAmount;

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
            this.accountModel = deposit.checkingAccount;
            this.ItemDescription = this.model.description;
            this.ItemType = AccountItemType.Withdrawal;
            this.ItemDate = model.transactionDate;
            this.ItemAmount = model.transactionAmount;

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
            bool _saveOk = false;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (this.IsNew)
                {
                    var _resultsAddWithdrawal = await uow.AddCheckingDepositAsync(model);
                    _saveOk = _resultsAddWithdrawal.Successful;
                    if (_saveOk)
                    {
                        this.IsDirty = false;
                        this.IsNew = false;
                        this.CanEdit = true;
                        this.CanDelete = true;
                    }
                    else
                    {
                        if (_resultsAddWithdrawal.WorkException != null)
                        {
                            WriteErrorCondition(_resultsAddWithdrawal.WorkException);
                        }
                        else if (!string.IsNullOrEmpty(_resultsAddWithdrawal.Message))
                        {
                            WriteErrorCondition(_resultsAddWithdrawal.Message);
                        }
                        else
                        {
                            WriteErrorCondition("An unknown error has occurred saving deposit record");
                        }
                    }
                }
                else
                {
                    var _resultsUpdateWithdrawal = await uow.UpdateCheckingDepositAsync(model);
                    _saveOk = _resultsUpdateWithdrawal.Successful;
                    if (_saveOk)
                    {
                        this.IsDirty = false;
                        this.IsNew = false;
                        this.CanEdit = true;
                        this.CanDelete = true;
                    }
                    else
                    {
                        if (_resultsUpdateWithdrawal.WorkException != null)
                        {
                            WriteErrorCondition(_resultsUpdateWithdrawal.WorkException);
                        }
                        else if (!string.IsNullOrEmpty(_resultsUpdateWithdrawal.Message))
                        {
                            WriteErrorCondition(_resultsUpdateWithdrawal.Message);
                        }
                        else
                        {
                            WriteErrorCondition("An unknown error has occurred saving deposit record");
                        }
                    }
                }
            }
        }
    }
}
