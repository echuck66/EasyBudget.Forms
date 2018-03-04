//
//  Copyright 2018  CrawfordNET Solutions, LLC
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class CheckingWithdrawalViewModel : WithdrawalViewModel, INotifyPropertyChanged
    {
        CheckingWithdrawal model { get; set; }

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

        public int checkNumber 
        {
            get
            {
                return model.checkNumber;
            }
            set
            {
                if (model.checkNumber != value)
                {
                    model.checkNumber = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(checkNumber)));
                }
            }
        }

        public string payToTheOrderOf 
        {
            get
            {
                return model.payToTheOrderOf;
            }
            set
            {
                if (model.payToTheOrderOf != value)
                {
                    model.payToTheOrderOf = value;
                    this.ItemDescription = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(payToTheOrderOf)));
                }
            }
        }

        public string memo 
        {
            get
            {
                return model.memo;
            }
            set
            {
                if (model.memo != value)
                {
                    model.memo = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(memo)));
                }
            }
        }

        public int BudgetItemId 
        {
            get
            {
                return model.budgetExpenseId;
            }
            set
            {
                if (model.budgetExpenseId != value)
                {
                    model.budgetExpenseId = value;
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

        public bool isTaxDeductable 
        {
            get
            {
                return model.isTaxDeductable;
            }
            set
            {
                if (model.isTaxDeductable != value)
                {
                    model.isTaxDeductable = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(isTaxDeductable)));
                }
            }
        }

        internal CheckingWithdrawalViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        public void PopulateVM(CheckingWithdrawal withdrawal)
        {
            this.model = withdrawal;
            this.accountModel = withdrawal.checkingAccount;
            this.ItemId = this.model.id;
            this.ItemDescription = this.model.payToTheOrderOf;
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

        public async Task PopulateVMAsync(CheckingWithdrawal withdrawal)
        {
            this.model = withdrawal;
            this.accountModel = withdrawal.checkingAccount;
            this.ItemId = this.model.id;
            this.ItemDescription = this.model.payToTheOrderOf;
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
                    var _resultsAddWithdrawal = await uow.AddCheckingWithdrawalAsync(model);
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
                            WriteErrorCondition("An unknown error has occurred saving withdrawal record");
                        }
                    }
                }
                else
                {
                    var _resultsUpdateWithdrawal = await uow.UpdateCheckingWithdrawalAsync(model);
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
                var _results = await uow.DeleteCheckingWithdrawalAsync(model);
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
                        WriteErrorCondition("An unknown error has occurred deleting withdrawal record");
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
                this.BudgetItems.Clear();
                int categoryId = this.SelectedCategory.id;

                using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
                {
                    var _results = await uow.GetCategoryExpenseItemsAsync(this.SelectedCategory);
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
