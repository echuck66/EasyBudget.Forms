using System;
using System.Threading.Tasks;
using System.ComponentModel;
using EasyBudget.Models.DataModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EasyBudget.Business.ChartModels;

namespace EasyBudget.Business.ViewModels
{
    public class CheckingDepositViewModel : DepositViewModel, INotifyPropertyChanged
    {
        CheckingDeposit model { get; set; }

        public DateTime TransactionDate 
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransactionDate)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanSave)));
                }
            }
        }

        public decimal TransactionAmount 
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransactionAmount)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanSave)));
                }
            }
        }

        public string Description 
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanSave)));
                }
            }
        }

        public string Notation 
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Notation)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanSave)));
                }
            }
        }

        public int BudgetItemId 
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BudgetItemId)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanSave)));
                }
            }
        }

        BudgetCategory _SelectedCategory;
        public BudgetCategory SelectedCategory
        {
            get
            {
                return _SelectedCategory;
            }
            set
            {
                _SelectedCategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCategory)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanSave)));
            }
        }

        BudgetItem _SelectedBudgetItem;
        public BudgetItem SelectedBudgetItem
        {
            get
            {
                return _SelectedBudgetItem;
            }
            set
            {
                _SelectedBudgetItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedBudgetItem)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanSave)));
                if (value != null)
                {
                    this.BudgetItemId = _SelectedBudgetItem.id;
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanSave)));
                }
            }
        }

        public bool CanSave
        {
            get
            {
                bool _canSave = BudgetItemId > 0 &&
                                SelectedCategory != null && 
                                SelectedBudgetItem != null && 
                                !string.IsNullOrEmpty(this.Description) &&
                                TransactionAmount > 0 &&
                                TransactionDate > DateTime.MinValue;
                
                return _canSave;
            }
        }

        public bool CategorySelectEnabled
        {
            get
            {
                return this.BudgetCategories.Count > 0;
            }
        }

        public bool BudgetItemSelectEnabled
        {
            get
            {
                return this.BudgetItems.Count > 0;
            }
        }

        public DateTime MinTransactionDate
        {
            get
            {
                DateTime _minDate = this.TransactionDate > DateTime.MinValue ? this.TransactionDate.AddYears(-1) : DateTime.Now.AddYears(-2);
                return _minDate;
            }
        }

        public DateTime MaxTransactionDate
        {
            get
            {
                DateTime _maxDate = DateTime.Now.AddYears(2);
                return _maxDate;
            }
        }

        public CheckingDepositViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        public async Task PopulateVMAsync(CheckingDeposit deposit)
        {
            this.model = deposit;
            this.accountModel = deposit.checkingAccount;
            this.ItemId = this.model.id;
            this.ItemType = AccountItemType.Deposits;
            this.ItemAmount = model.transactionAmount;
            this.EndingBalance = model.endingBalance;
            this.ItemDescription = model.description;
            this.Description = this.model.description;
            this.TransactionDate = deposit.transactionDate > DateTime.MinValue ? deposit.transactionDate : DateTime.Now;
            this.ItemDate = this.TransactionDate;
            this.TransactionAmount = model.transactionAmount;

            this.BudgetItemId = model.budgetIncomeId;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllBudgetCategoriesAsync();
                if (_results.Successful)
                {
                    foreach(BudgetCategory category in _results.Results)
                    {
                        var _resultsItemCountCheck = await uow.GetCategoryIncomeItemsAsync(category);
                        if (_resultsItemCountCheck.Successful && _resultsItemCountCheck.Results.Count > 0)
                        {
                            if (category.categoryType == Models.BudgetCategoryType.Income)
                            {
                                this.BudgetCategories.Add(category);
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategorySelectEnabled)));
                            }
                        }
                    }

                    if (this.BudgetItemId > 0)
                    {
                        var _resultsGetBudgetItem = await uow.GetIncomeItemAsync(this.BudgetItemId);
                        if (_resultsGetBudgetItem.Successful)
                        {
                            var selectedItem = _resultsGetBudgetItem.Results;
                            if (this.BudgetCategories.Any(c => c.id == selectedItem.budgetCategoryId))
                            {
                                this.SelectedCategory = this.BudgetCategories.FirstOrDefault(c => c.id == selectedItem.budgetCategoryId);
                                this.SelectedBudgetItem = selectedItem;
                            }
                        }
                        else
                        {
                            if (_resultsGetBudgetItem.WorkException != null)
                            {
                                WriteErrorCondition(_resultsGetBudgetItem.WorkException);
                            }
                            else if (!string.IsNullOrEmpty(_resultsGetBudgetItem.Message))
                            {
                                WriteErrorCondition(_resultsGetBudgetItem.Message);
                            }
                            else
                            {
                                WriteErrorCondition("An unknown error has occurred populating deposit record");
                            }
                        }
                    }
                }
                else
                {
                    if (_results.WorkException != null)
                    {
                        WriteErrorCondition(_results.WorkException);
                    }
                    else if (!string.IsNullOrEmpty(_results.Message))
                    {
                        WriteErrorCondition(_results.Message);
                    }
                    else
                    {
                        WriteErrorCondition("An unknown error has occurred getting category records");
                    }
                }
            }
        }
    
        public async override Task<bool> SaveChangesAsync()
        {
            bool _saveOk = false;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (this.IsNew)
                {
                    var _resultsAddWithdrawal = await uow.DepositMoneyCheckingAsync(model);
                    _saveOk = _resultsAddWithdrawal.Successful;
                    if (_saveOk)
                    {
                        this.IsDirty = false;
                        this.IsNew = false;
                        this.CanEdit = true;
                        this.CanDelete = true;
                        OnItemUpdated();
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
                        OnItemUpdated();
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

            return _saveOk;
        }

        public async override Task<bool> DeleteAsync()
        {
            bool deleted = false;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.DeleteCheckingDepositAsync(model);
                deleted = _results.Successful;
                if (deleted)
                {
                    OnItemUpdated();
                }
                else
                {
                    if (_results.WorkException != null)
                    {
                        WriteErrorCondition(_results.WorkException);
                    }
                    else if (!string.IsNullOrEmpty(_results.Message))
                    {
                        WriteErrorCondition(_results.Message);
                    }
                    else
                    {
                        WriteErrorCondition("An unknown error has occurred deleting deposit record");
                    }
                }
            }

            return deleted;
        }

        public delegate void ItemUpdatedEventHandler(object sender, BankingItemUpdatedEventArgs e);

        public event ItemUpdatedEventHandler ItemUpdated;

        public void OnItemUpdated()
        {
            if (this.ItemUpdated != null)
            {
                var args = new BankingItemUpdatedEventArgs();
                args.AccountType = Models.BankAccountType.Checking;
                args.TransactionAmount = this.TransactionAmount;
                ItemUpdated(this, args);
            }
        }
    
        public async Task OnCategorySelected()
        {
            if (this.SelectedCategory != null)
            {
                if (this.BudgetItems.Count > 0)
                {
                    for (int i = this.BudgetItems.Count - 1; i >= 0; i--)
                    {
                        var itm = this.BudgetItems[i];

                        this.BudgetItems.Remove(itm);
                    }
                }
                int categoryId = this.SelectedCategory.id;

                using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
                {
                    var _results = await uow.GetCategoryIncomeItemsAsync(this.SelectedCategory);
                    if (_results.Successful)
                    {
                        foreach(var itm in _results.Results)
                        {
                            this.BudgetItems.Add(itm);
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BudgetItemSelectEnabled)));
                        }
                        this.SelectedBudgetItem = null;
                    }
                    else
                    {
                        if (_results.WorkException != null)
                        {
                            WriteErrorCondition(_results.WorkException);
                        }
                        else if (!string.IsNullOrEmpty(_results.Message))
                        {
                            WriteErrorCondition(_results.Message);
                        }
                        else
                        {
                            WriteErrorCondition("An unknown error has occurred while retrieving a list of related Budget Items");
                        }
                    }
                }
            }
        }
    }
}
