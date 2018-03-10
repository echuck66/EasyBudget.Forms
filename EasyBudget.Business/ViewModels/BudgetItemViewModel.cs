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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using EasyBudget.Business.ChartModels;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class BudgetItemViewModel : BaseViewModel, INotifyPropertyChanged
    {
        BudgetItem model { get; set; }

        public int ItemId 
        {
            get
            {
                return model.id;
            }
        }

        public string CategoryName 
        {
            get 
            {
                return model.budgetCategory?.categoryName;
            }
        }

        public BudgetItemType ItemType 
        {
            get
            {
                return model.ItemType;
            }
            set
            {
                if (model.ItemType != value)
                {
                    model.ItemType = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemType)));
                }
            }
        }

        public decimal BudgetedAmount 
        {
            get
            {
                return model.BudgetedAmount;
            }
            set
            {
                if (model.BudgetedAmount != value) 
                {
                    model.BudgetedAmount = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BudgetedAmount)));
                }
            }
        }

        public string ItemDescription 
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemDescription)));
                }
            }
        }

        public string ItemNotation 
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemNotation)));
                }
            }
        }

        public bool IsRecurring 
        {
            get
            {
                return model.recurring;
            }
            set 
            {
                if (model.recurring != value) 
                {
                    model.recurring = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRecurring)));
                }
            }
        }

        public ObservableCollection<Frequency> AvailableFrequencies { get; set; }

        public Frequency ItemFrequency 
        {
            get 
            {
                return model.frequency;
            }
            set 
            {
                if (model.frequency != value) 
                {
                    model.frequency = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemFrequency)));
                }
            }
        }

        public DateTime MinStartDate
        {
            get
            {
                return DateTime.Now.AddYears(-1);
            }
        }

        public DateTime StartDate 
        {
            get 
            {
                return model.StartDate;
            }
            set
            {
                if (model.StartDate != value) 
                {
                    model.StartDate = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate)));
                }
            }
        }

        public DateTime MinEndDate
        {
            get
            {
                return this.MinStartDate.AddMonths(1);
            }
        }

        public DateTime? EndDate 
        {
            get
            {
                return model.EndDate;
            }
            set
            {
                if (model.EndDate != value) 
                {
                    model.EndDate = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndDate)));
                }
            }
        }

        internal BudgetItemViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            AvailableFrequencies = new ObservableCollection<Frequency>() {
                Frequency.OneTime,
                Frequency.Daily,
                Frequency.Weekly,
                Frequency.BiWeekly,
                Frequency.SemiMonthly,
                Frequency.Monthly,
                Frequency.Annually
            };
        }

        public override event PropertyChangedEventHandler PropertyChanged;

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

        internal async Task LoadVMAsync(int itemId, BudgetItemType itemType)
        {
            switch (itemType)
            {
                case BudgetItemType.Income:
                    await LoadIncomeItemAsync(itemId);
                    break;
                case BudgetItemType.Expense:
                    await LoadExpenseItemAsync(itemId);
                    break;
            }
        }

        async Task LoadExpenseItemAsync(int itemId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetExpenseItemAsync(itemId);
                if (_results.Successful)
                {
                    await PopulateVMAsync(_results.Results);
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
                        WriteErrorCondition("An unknown error has occurred loading expense item object");
                    }
                }
            }
        }

        async Task LoadIncomeItemAsync(int itemId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetIncomeItemAsync(itemId);
                if (_results.Successful)
                {
                    await PopulateVMAsync(_results.Results);
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
                        WriteErrorCondition("An unknown error has occurred loading income item object");
                    }
                }
            }
        }

        internal async Task PopulateVMAsync(BudgetItem item)
        {
            model = item;

            if (item.budgetCategory == null)
            {
                using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
                {
                    item.budgetCategory = await GetBudgetCategoryAsync(item.budgetCategoryId);
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

                        var _resultsDeleteExpense = await uow.DeleteExpenseItemAsync(this.model as ExpenseItem);
                        deleted = _resultsDeleteExpense.Successful;
                        if (deleted)
                        {
                            OnItemUpdated();
                        }
                        else
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
                        var _resultsDeleteIncome = await uow.DeleteIncomeItemAsync(this.model as IncomeItem);
                        deleted = _resultsDeleteIncome.Successful;
                        if (deleted)
                        {
                            OnItemUpdated();
                        }
                        else
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
                            var _resultsAddExpense = await uow.AddExpenseItemAsync(this.model as ExpenseItem);
                            _saveOk = _resultsAddExpense.Successful;
                            if (_saveOk)
                            {
                                //this.model = _resultsAddExpense.Results;
                                this.IsDirty = false;
                                this.IsNew = false;
                                this.CanEdit = true;
                                this.CanDelete = true;
                                OnItemUpdated();
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
                            var _resultsAddIncome = await uow.AddIncomeItemAsync(this.model as IncomeItem);
                            _saveOk = _resultsAddIncome.Successful;
                            if (_saveOk)
                            {
                                //this.model = _resultsAddIncome.Results;
                                this.IsDirty = false;
                                this.IsNew = false;
                                this.CanEdit = true;
                                this.CanDelete = true;
                                OnItemUpdated();
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
                            var _resultsUpdateExpense = await uow.UpdateExpenseItemAsync(this.model as ExpenseItem);
                            _saveOk = _resultsUpdateExpense.Successful;
                            if (_saveOk)
                            {
                                //this.model = _resultsUpdateExpense.Results;
                                this.IsDirty = false;
                                this.IsNew = false;
                                this.CanEdit = true;
                                this.CanDelete = true;
                                OnItemUpdated();
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

                            var _resultsUpdateIncome = await uow.UpdateIncomeItemAsync(this.model as IncomeItem);
                            _saveOk = _resultsUpdateIncome.Successful;
                            if (_saveOk)
                            {
                                //this.model = _resultsUpdateIncome.Results;
                                this.IsDirty = false;
                                this.IsNew = false;
                                this.CanEdit = true;
                                this.CanDelete = true;
                                OnItemUpdated();
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

        public delegate void ItemUpdatedEventHandler(object sender, EventArgs e);

        public event ItemUpdatedEventHandler ItemUpdated;

        public void OnItemUpdated()
        {
            if (this.ItemUpdated != null)
            {
                ItemUpdated(this, new EventArgs());
            }
        }

        public override IChartDataPack GetChartData()
        {
            throw new NotImplementedException();
        }
    }

    public class BudgetItemViewModelComparer : IEqualityComparer<BudgetItemViewModel>
    {
        public bool Equals(BudgetItemViewModel x, BudgetItemViewModel y)
        {
            return x.ItemId == y.ItemId;
        }

        public int GetHashCode(BudgetItemViewModel obj)
        {
            return obj.ItemId.GetHashCode() + obj.ItemType.GetHashCode() + obj.ItemFrequency.GetHashCode() + obj.ItemDescription.GetHashCode();
        }
    }
}
