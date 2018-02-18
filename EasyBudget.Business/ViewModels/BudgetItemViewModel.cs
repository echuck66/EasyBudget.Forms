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
        BudgetItem BudgetItem { get; set; }

        public string CategoryName 
        {
            get 
            {
                return BudgetItem.budgetCategory?.categoryName;
            }
        }

        public BudgetItemType ItemType 
        {
            get
            {
                return BudgetItem.ItemType;
            }
            set
            {
                if (BudgetItem.ItemType != value)
                {
                    BudgetItem.ItemType = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemType)));
                }
            }
        }

        public decimal BudgetedAmount 
        {
            get
            {
                return BudgetItem.BudgetedAmount;
            }
            set
            {
                if (BudgetItem.BudgetedAmount != value) 
                {
                    BudgetItem.BudgetedAmount = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BudgetedAmount)));
                }
            }
        }

        public string ItemDescription 
        {
            get
            {
                return BudgetItem.description;
            }
            set
            {
                if (BudgetItem.description != value)
                {
                    BudgetItem.description = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemDescription)));
                }
            }
        }

        public string ItemNotation 
        {
            get
            {
                return BudgetItem.notation;
            }
            set 
            {
                if (BudgetItem.notation != value) 
                {
                    BudgetItem.notation = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemNotation)));
                }
            }
        }

        public bool IsRecurring 
        {
            get
            {
                return BudgetItem.recurring;
            }
            set 
            {
                if (BudgetItem.recurring != value) 
                {
                    BudgetItem.recurring = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRecurring)));
                }
            }
        }

        public Frequency ItemFrequency 
        {
            get 
            {
                return BudgetItem.frequency;
            }
            set 
            {
                if (BudgetItem.frequency != value) 
                {
                    BudgetItem.frequency = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemFrequency)));
                }
            }
        }

        public DateTime StartDate 
        {
            get 
            {
                return BudgetItem.StartDate;
            }
            set
            {
                if (BudgetItem.StartDate != value) 
                {
                    BudgetItem.StartDate = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate)));
                }
            }
        }

        public DateTime? EndDate 
        {
            get
            {
                return BudgetItem.EndDate;
            }
            set
            {
                if (BudgetItem.EndDate != value) 
                {
                    BudgetItem.EndDate = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndDate)));
                }
            }
        }

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
                this.BudgetItem = item;
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

                        var _resultsDeleteExpense = await uow.DeleteExpenseItemAsync(this.BudgetItem as ExpenseItem);
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
                        break;
                    case BudgetItemType.Income:
                        var _resultsDeleteIncome = await uow.DeleteIncomeItemAsync(this.BudgetItem as IncomeItem);
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
                        break;
                }
            }

            return deleted;
        }
    
        public async Task SaveChangesAsync()
        {
            bool _saveOk = true;
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (this.IsNew)
                {
                    switch (this.ItemType)
                    {
                        case BudgetItemType.Expense:
                            var _resultsAddExpense = await uow.AddExpenseItemAsync(this.BudgetItem as ExpenseItem);
                            _saveOk = _resultsAddExpense.Successful;
                            if (_saveOk)
                            {
                                this.BudgetItem = _resultsAddExpense.Results;
                                this.IsNew = false;
                                this.CanEdit = true;
                                this.CanDelete = true;
                            }
                            else
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
                            var _resultsAddIncome = await uow.AddIncomeItemAsync(this.BudgetItem as IncomeItem);
                            _saveOk = _resultsAddIncome.Successful;
                            if (_saveOk)
                            {
                                this.BudgetItem = _resultsAddIncome.Results;
                                this.IsNew = false;
                                this.CanEdit = true;
                                this.CanDelete = true;
                            }
                            else
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
                            var _resultsUpdateExpense = await uow.UpdateExpenseItemAsync(this.BudgetItem as ExpenseItem);
                            _saveOk = _resultsUpdateExpense.Successful;
                            if (_saveOk)
                            {
                                this.BudgetItem = _resultsUpdateExpense.Results;
                                this.IsNew = false;
                                this.CanEdit = true;
                                this.CanDelete = true;
                            }
                            else
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


                            break;
                        case BudgetItemType.Income:

                            var _resultsUpdateIncome = await uow.UpdateIncomeItemAsync(this.BudgetItem as IncomeItem);
                            _saveOk = _resultsUpdateIncome.Successful;
                            if (_saveOk)
                            {
                                this.BudgetItem = _resultsUpdateIncome.Results;
                                this.IsNew = false;
                                this.CanEdit = true;
                                this.CanDelete = true;
                            }
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

                            break;
                    }
                }
            }
        }
    }
}
