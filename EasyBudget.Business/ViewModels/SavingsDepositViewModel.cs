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
                //if (value != null)
                //{
                //    Task.Run(() => OnCategorySelected());
                //}
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
                this.BudgetItemId = value.id;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedBudgetItem)));
                if (value != null)
                {
                    this.BudgetItemId = value.id;
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

        public SavingsDepositViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        public void PopulateVM(SavingsDeposit deposit)
        {
            this.model = deposit;
            this.accountModel = deposit.savingsAccount;
            this.ItemId = this.model.id;
            this.ItemDescription = this.model.description;
            this.ItemType = AccountItemType.Withdrawals;
            this.ItemDate = model.transactionDate;
            this.ItemAmount = model.transactionAmount;

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
            this.accountModel = deposit.savingsAccount;
            this.ItemId = this.model.id;
            this.ItemDescription = this.model.description;
            this.ItemType = AccountItemType.Withdrawals;
            this.ItemDate = model.transactionDate;
            this.ItemAmount = model.transactionAmount;

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

        public async override Task SaveChangesAsync()
        {
            bool _saveOk = false;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (this.IsNew)
                {
                    var _resultsAddWithdrawal = await uow.AddSavingsDepositAsync(model);
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
                    var _resultsUpdateWithdrawal = await uow.UpdateSavingsDepositAsync(model);
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
                            WriteErrorCondition("An unknown error has occurred saving withdrawal record");
                        }
                    }
                }
            }
        }

        public async override Task<bool> DeleteAsync()
        {
            bool deleted = false;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.DeleteSavingsDepositAsync(model);
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

        public delegate void ItemUpdatedEventHandler(object sender, EventArgs e);

        public event ItemUpdatedEventHandler ItemUpdated;

        public void OnItemUpdated()
        {
            if (this.ItemUpdated != null)
            {
                ItemUpdated(this, new EventArgs());
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
                        foreach (var itm in _results.Results)
                        {
                            this.BudgetItems.Add(itm);
                        }
                        this.SelectedBudgetItem = null;
                    }
                }
            }
        }
    }
}
