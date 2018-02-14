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
using System.ComponentModel;
using System.Threading.Tasks;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class BudgetItemViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public int CategoryId { get; set; }

        public int BudgetItemId { get; set; }

        public string CategoryName { get; set; }

        public BudgetItemType ItemType { get; set; }

        decimal _BudgetedAmount;
        public decimal BudgetedAmount 
        {
            get
            {
                return _BudgetedAmount;
            }
            set
            {
                if (_BudgetedAmount != value) 
                {
                    _BudgetedAmount = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BudgetedAmount)));
                }
            }
        }

        string _ItemDescription;
        public string ItemDescription 
        {
            get
            {
                return _ItemDescription;
            }
            set
            {
                if (_ItemDescription != value)
                {
                    _ItemDescription = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemDescription)));
                }
            }
        }

        string _ItemNotation;
        public string ItemNotation 
        {
            get
            {
                return _ItemNotation;
            }
            set 
            {
                if (_ItemNotation != value) 
                {
                    _ItemNotation = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemNotation)));
                }
            }
        }

        bool _IsRecurring;
        public bool IsRecurring 
        {
            get
            {
                return _IsRecurring;
            }
            set 
            {
                if (_IsRecurring != value) 
                {
                    _IsRecurring = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRecurring)));
                }
            }
        }

        Frequency _ItemFrequency;
        public Frequency ItemFrequency 
        {
            get 
            {
                return _ItemFrequency;
            }
            set 
            {
                if (_ItemFrequency != value) 
                {
                    _ItemFrequency = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemFrequency)));
                }
            }
        }

        DateTime _StartDate;
        public DateTime StartDate 
        {
            get 
            {
                return _StartDate;
            }
            set
            {
                if (_StartDate != value) 
                {
                    _StartDate = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate)));
                }
            }
        }

        DateTime? _EndDate;
        public DateTime? EndDate 
        {
            get
            {
                return _EndDate;
            }
            set
            {
                if (_EndDate != value) 
                {
                    _EndDate = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndDate)));
                }
            }
        }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool IsNew { get; set; }

        public bool IsDirty { get; set; }

        internal BudgetItemViewModel(string dbFilePath)
            : base(dbFilePath)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        async Task<BudgetCategory> GetBudgetCategoryAsync(int categoryId)
        {
            BudgetCategory _category = null;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {

                var _resultsCategory = await uow.GetBudgetCategoryAsync(categoryId);
                if (_resultsCategory.Successful)
                {
                    _category = _resultsCategory.Category;
                }
                else
                {
                    if (_resultsCategory.WorkException != null)
                    {
                        WriteErrorCondition(_resultsCategory.WorkException);
                    }
                    else if (!string.IsNullOrEmpty(_resultsCategory.Message))
                    {
                        WriteErrorCondition(_resultsCategory.Message);
                    }
                    else
                    {
                        WriteErrorCondition("An unknown error has occurred loading item's BudgetCategory object");
                    }
                }
            }

            return _category;
        }

        internal async Task PopulateVMAsync(BudgetItem item)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (item.budgetCategory == null)
                {
                    item.budgetCategory = await GetBudgetCategoryAsync(item.budgetCategoryId);
                }
                if (item.budgetCategory != null)
                {
                    this.CategoryId = item.budgetCategoryId;
                    this.BudgetItemId = item.id;
                    this.CategoryName = item.budgetCategory.categoryName;
                    this.ItemType = item.ItemType;
                    _BudgetedAmount = item.BudgetedAmount;
                    _ItemDescription = item.description;
                    _ItemNotation = item.notation;
                    _StartDate = item.StartDate;
                    _EndDate = item.EndDate;
                    _IsRecurring = item.recurring;
                    _ItemFrequency = item.frequency;
                    this.CanEdit = item.CanEdit;
                    this.CanDelete = item.CanDelete;
                    this.IsNew = item.IsNew;
                }
            }
        }

        public async Task<bool> DeleteAsync()
        {
            bool deleted = false;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                switch (this.ItemType)
                {
                    case BudgetItemType.Expense:
                        var _resultsGetExpenseItem = await uow.GetExpenseItemAsync(this.BudgetItemId);
                        if (_resultsGetExpenseItem.Successful)
                        {
                            var _resultsDeleteExpense = await uow.DeleteExpenseItemAsync(_resultsGetExpenseItem.Results);
                            deleted = _resultsDeleteExpense.Successful;
                            if (!_resultsDeleteExpense.Successful)
                            {
                                if (_resultsDeleteExpense.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsDeleteExpense.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsDeleteExpense.Message))
                                {
                                    WriteErrorCondition(_resultsDeleteExpense.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred loading item's BudgetCategory object");
                                }
                            }
                        }
                        else
                        {
                            if (_resultsGetExpenseItem.WorkException != null)
                            {
                                WriteErrorCondition(_resultsGetExpenseItem.WorkException);
                            }
                            else if (!string.IsNullOrEmpty(_resultsGetExpenseItem.Message))
                            {
                                WriteErrorCondition(_resultsGetExpenseItem.Message);
                            }
                            else
                            {
                                WriteErrorCondition("An unknown error has occurred loading item's BudgetCategory object");
                            }
                        }

                        break;
                    case BudgetItemType.Income:
                        var _resultsGetIncomeItem = await uow.GetIncomeItemAsync(this.BudgetItemId);
                        if (_resultsGetIncomeItem.Successful)
                        {
                            var _resultsDeleteIncome = await uow.DeleteIncomeItemAsync(_resultsGetIncomeItem.Results);
                            deleted = _resultsDeleteIncome.Successful;
                            if (!_resultsDeleteIncome.Successful)
                            {
                                if (_resultsDeleteIncome.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsDeleteIncome.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsDeleteIncome.Message))
                                {
                                    WriteErrorCondition(_resultsDeleteIncome.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred loading item's BudgetCategory object");
                                }
                            }
                        }
                        else
                        {
                            if (_resultsGetIncomeItem.WorkException != null)
                            {
                                WriteErrorCondition(_resultsGetIncomeItem.WorkException);
                            }
                            else if (!string.IsNullOrEmpty(_resultsGetIncomeItem.Message))
                            {
                                WriteErrorCondition(_resultsGetIncomeItem.Message);
                            }
                            else
                            {
                                WriteErrorCondition("An unknown error has occurred loading item's BudgetCategory object");
                            }
                        }
                        break;
                }
            }

            return deleted;
        }
    
        public async Task SaveChangesAsync()
        {
            bool _saveOk = true;
            BudgetItem item;
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (this.IsNew)
                {
                    switch (this.ItemType)
                    {
                        case BudgetItemType.Expense:
                            item = new ExpenseItem();
                            item.budgetCategoryId = this.CategoryId;
                            item.BudgetedAmount = this.BudgetedAmount;
                            item.CanDelete = this.CanDelete;
                            item.CanEdit = this.CanEdit;
                            item.description = this.ItemDescription;
                            item.EndDate = this.EndDate;
                            item.StartDate = this.StartDate;
                            item.frequency = this.ItemFrequency;
                            item.IsNew = this.IsNew;
                            item.ItemType = this.ItemType;
                            item.notation = this.ItemNotation;
                            item.recurring = this.IsRecurring;
                            var _resultsAddExpense = await uow.AddExpenseItemAsync(item as ExpenseItem);
                            _saveOk = _resultsAddExpense.Successful;
                            if (!_saveOk)
                            {
                                if (_resultsAddExpense.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsAddExpense.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsAddExpense.Message))
                                {
                                    WriteErrorCondition(_resultsAddExpense.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred adding budget item object");
                                }
                            }
                            break;
                        case BudgetItemType.Income:
                            item = new IncomeItem();
                            item.budgetCategoryId = this.CategoryId;
                            item.BudgetedAmount = this.BudgetedAmount;
                            item.CanDelete = this.CanDelete;
                            item.CanEdit = this.CanEdit;
                            item.description = this.ItemDescription;
                            item.EndDate = this.EndDate;
                            item.StartDate = this.StartDate;
                            item.frequency = this.ItemFrequency;
                            item.IsNew = this.IsNew;
                            item.ItemType = this.ItemType;
                            item.notation = this.ItemNotation;
                            item.recurring = this.IsRecurring;
                            var _resultsAddIncome = await uow.AddIncomeItemAsync(item as IncomeItem);
                            _saveOk = _resultsAddIncome.Successful;
                            if (!_saveOk)
                            {
                                if (_resultsAddIncome.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsAddIncome.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsAddIncome.Message))
                                {
                                    WriteErrorCondition(_resultsAddIncome.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred adding budget item object");
                                }
                            }
                            break;
                    }
                }
                else
                {
                    switch (this.ItemType)
                    {
                        case BudgetItemType.Expense:
                            var _resultsGetExpense = await uow.GetExpenseItemAsync(this.BudgetItemId);
                            if (_resultsGetExpense.Successful)
                            {
                                item = _resultsGetExpense.Results;
                                item.budgetCategoryId = this.CategoryId;
                                item.BudgetedAmount = this.BudgetedAmount;
                                item.CanDelete = this.CanDelete;
                                item.CanEdit = this.CanEdit;
                                item.description = this.ItemDescription;
                                item.EndDate = this.EndDate;
                                item.StartDate = this.StartDate;
                                item.frequency = this.ItemFrequency;
                                item.IsNew = this.IsNew;
                                item.ItemType = this.ItemType;
                                item.notation = this.ItemNotation;
                                item.recurring = this.IsRecurring;
                                var _resultsUpdateExpense = await uow.UpdateExpenseItemAsync(item as ExpenseItem);
                                _saveOk = _resultsUpdateExpense.Successful;
                                if (!_saveOk)
                                {
                                    if (_resultsUpdateExpense.WorkException != null)
                                    {
                                        WriteErrorCondition(_resultsUpdateExpense.WorkException);
                                    }
                                    else if (!string.IsNullOrEmpty(_resultsUpdateExpense.Message))
                                    {
                                        WriteErrorCondition(_resultsUpdateExpense.Message);
                                    }
                                    else
                                    {
                                        WriteErrorCondition("An unknown error has occurred updating budget item object");
                                    }
                                }
                            }
                            else
                            {
                                if (_resultsGetExpense.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsGetExpense.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsGetExpense.Message))
                                {
                                    WriteErrorCondition(_resultsGetExpense.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred locating original budget item object");
                                }
                            }
                            break;
                        case BudgetItemType.Income:
                            var _resultsGetIncome = await uow.GetIncomeItemAsync(this.BudgetItemId);
                            if (_resultsGetIncome.Successful)
                            {
                                item = _resultsGetIncome.Results;
                                item.budgetCategoryId = this.CategoryId;
                                item.BudgetedAmount = this.BudgetedAmount;
                                item.CanDelete = this.CanDelete;
                                item.CanEdit = this.CanEdit;
                                item.description = this.ItemDescription;
                                item.EndDate = this.EndDate;
                                item.StartDate = this.StartDate;
                                item.frequency = this.ItemFrequency;
                                item.IsNew = this.IsNew;
                                item.ItemType = this.ItemType;
                                item.notation = this.ItemNotation;
                                item.recurring = this.IsRecurring;
                                var _resultsUpdateIncome = await uow.UpdateIncomeItemAsync(item as IncomeItem);
                                _saveOk = _resultsUpdateIncome.Successful;
                                if (!_saveOk)
                                {
                                    if (_resultsUpdateIncome.WorkException != null)
                                    {
                                        WriteErrorCondition(_resultsUpdateIncome.WorkException);
                                    }
                                    else if (!string.IsNullOrEmpty(_resultsUpdateIncome.Message))
                                    {
                                        WriteErrorCondition(_resultsUpdateIncome.Message);
                                    }
                                    else
                                    {
                                        WriteErrorCondition("An unknown error has occurred updating budget item object");
                                    }
                                }
                            }
                            else
                            {
                                if (_resultsGetIncome.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsGetIncome.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsGetIncome.Message))
                                {
                                    WriteErrorCondition(_resultsGetIncome.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred locating original budget item object");
                                }
                            }
                            break;
                    }
                }
            }

            if (_saveOk)
            {
                this.IsNew = false;
                this.IsDirty = false;
            }
        }
    }

}
