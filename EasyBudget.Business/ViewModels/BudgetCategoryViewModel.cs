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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class BudgetCategoryViewModel : BaseViewModel, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public int CategoryId { get; set; }

        string _Name;
        public string Name 
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        string _Description;
        public string Description 
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                }
            }
        }

        decimal _Amount;
        public decimal Amount 
        {
            get{
                return _Amount;
            }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
                }
            }
        }

        public bool IsSystemCategory { get; set; }

        public bool IsUserSelected { get; set; }

        AppIcon _CategoryIcon;
        public AppIcon CategoryIcon 
        {
            get
            {
                return _CategoryIcon;
            }
            set
            {
                if (_CategoryIcon != value)
                {
                    _CategoryIcon = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryId)));
                }
            }
        }

        public BudgetCategoryType CategoryType { get; set; }

        public ICollection<BudgetItemViewModel> BudgetItemVMs { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool IsNew { get; set; }

        public bool IsDirty { get; set; }

        public BudgetCategoryViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.BudgetItemVMs = new List<BudgetItemViewModel>();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        internal async Task PopulateVMAsync(BudgetCategory category)
        {

            this.CategoryId = category.id;
            _Name = category.categoryName;
            _Description = category.description;
            _Amount = category.budgetAmount;
            _CategoryIcon = category.categoryIcon;
            this.IsSystemCategory = category.systemCategory;
            this.IsUserSelected = category.userSelected;
            this.CategoryType = category.categoryType;
            this.IsNew = category.IsNew;
            this.CanEdit = category.CanEdit;
            this.CanDelete = category.CanDelete;

            using (UnitOfWork uow = new UnitOfWork(dbFilePath))
            {
                switch (category.categoryType)
                {
                    case BudgetCategoryType.Expense:
                        var _resultsExpenseItems = await uow.GetCategoryExpenseItemsAsync(category);
                        if (_resultsExpenseItems.Successful)
                        {
                            foreach (var item in _resultsExpenseItems.Results)
                            {
                                item.ItemType = BudgetItemType.Expense;
                                item.budgetCategory = category;
                                var vm = new BudgetItemViewModel(this.dbFilePath);
                                await vm.PopulateVMAsync(item);
                                this.BudgetItemVMs.Add(vm);
                            }
                        }
                        else
                        {
                            if (_resultsExpenseItems.WorkException != null)
                            {
                                WriteErrorCondition(_resultsExpenseItems.WorkException);
                            }
                            else if (!string.IsNullOrEmpty(_resultsExpenseItems.Message))
                            {
                                WriteErrorCondition(_resultsExpenseItems.Message);
                            }
                            else
                            {
                                WriteErrorCondition("An unknown error has occurred loading Expense Items");
                            }
                        }
                        break;
                    case BudgetCategoryType.Income:
                        var _resultsIncomeItems = await uow.GetCategoryIncomeItemsAsync(category);
                        if (_resultsIncomeItems.Successful)
                        {
                            foreach (var item in _resultsIncomeItems.Results)
                            {
                                item.ItemType = BudgetItemType.Income;
                                item.budgetCategory = category;
                                var vm = new BudgetItemViewModel(this.dbFilePath);
                                await vm.PopulateVMAsync(item);
                                this.BudgetItemVMs.Add(vm);
                            }
                        }
                        else
                        {
                            if (_resultsIncomeItems.WorkException != null)
                            {
                                WriteErrorCondition(_resultsIncomeItems.WorkException);
                            }
                            else if (!string.IsNullOrEmpty(_resultsIncomeItems.Message))
                            {
                                WriteErrorCondition(_resultsIncomeItems.Message);
                            }
                            else
                            {
                                WriteErrorCondition("An unknown error has occurred loading Income Items");
                            }
                        }
                        break;
                }
            }

        }

        internal async Task LoadBudgetCategoryDetails(int categoryId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetBudgetCategoryAsync(categoryId);
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
                        WriteErrorCondition("An unknown error has occurred loading the Budget Category");
                    }
                }
            }
        }

        public async Task<bool> DeleteAsync()
        {
            bool deleted = false;

            if (this.BudgetItemVMs.Count == 0)
            {
                using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
                {
                    var _resultsCategory = await uow.GetBudgetCategoryAsync(this.CategoryId);
                    if (_resultsCategory.Successful)
                    {
                        var _resultsDeleteCategory = await uow.DeleteBudgetCategoryAsync(_resultsCategory.Results);
                        deleted = _resultsDeleteCategory.Successful;
                        if (!_resultsDeleteCategory.Successful)
                        {
                            // TODO handle failure here

                        }
                    }
                }
            }
            else
            {
                // TODO make aware that BudgetItems must first be cleared

            }

            return deleted;
        }

        public async Task SaveChangesAsync()
        {
            bool _saveOk = true;

            BudgetCategory _category;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (this.IsNew)
                {
                    _category = new BudgetCategory();
                    _category.categoryName = this.Name;
                    _category.description = this.Description;
                    _category.categoryType = this.CategoryType;
                    _category.budgetAmount = this.Amount;
                    _category.CanDelete = this.CanDelete;
                    _category.CanEdit = this.CanEdit;
                    _category.categoryIcon = this.CategoryIcon;
                    _category.IsNew = this.IsNew;
                    _category.systemCategory = this.IsSystemCategory;
                    _category.userSelected = this.IsUserSelected;

                    var _resultsSaveNew = await uow.AddBudgetCategoryAsync(_category);
                    _saveOk = _resultsSaveNew.Successful;
                    if (!_saveOk)
                    {
                        if (_resultsSaveNew.WorkException != null)
                        {
                            WriteErrorCondition(_resultsSaveNew.WorkException);
                        }
                        else if (!string.IsNullOrEmpty(_resultsSaveNew.Message))
                        {
                            WriteErrorCondition(_resultsSaveNew.Message);
                        }
                        else
                        {
                            WriteErrorCondition("An unknown error has occurred saving the Budget Category");
                        }
                    }
                }
                else
                {
                    var _resultsGetExisting = await uow.GetBudgetCategoryAsync(this.CategoryId);
                    if (_resultsGetExisting.Successful)
                    {
                        _category = _resultsGetExisting.Results;
                        _category.categoryName = this.Name;
                        _category.description = this.Description;
                        _category.categoryType = this.CategoryType;
                        _category.budgetAmount = this.Amount;
                        _category.CanDelete = this.CanDelete;
                        _category.CanEdit = this.CanEdit;
                        _category.categoryIcon = this.CategoryIcon;
                        _category.IsNew = this.IsNew;
                        _category.systemCategory = this.IsSystemCategory;
                        _category.userSelected = this.IsUserSelected;

                        var _resultsUpdate = await uow.UpdateBudgetCategoryAsync(_category);
                        _saveOk = _resultsUpdate.Successful;
                        if (!_saveOk)
                        {
                            if (_resultsUpdate.WorkException != null)
                            {
                                WriteErrorCondition(_resultsUpdate.WorkException);
                            }
                            else if (!string.IsNullOrEmpty(_resultsUpdate.Message))
                            {
                                WriteErrorCondition(_resultsUpdate.Message);
                            }
                            else
                            {
                                WriteErrorCondition("An unknown error has occurred saving the Budget Category");
                            }
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

        public BudgetItemViewModel AddBudgetItem()
        {
            BudgetItemViewModel item = new BudgetItemViewModel(this.dbFilePath);
            item.CategoryId = this.CategoryId;
            item.ItemType = this.CategoryType == BudgetCategoryType.Expense ? BudgetItemType.Expense : BudgetItemType.Income;
            item.IsNew = true;
            this.BudgetItemVMs.Add(item);
            OnCollectionChanged(this, NotifyCollectionChangedAction.Add);
            return item;
        }

        public void OnCollectionChanged(object sender, NotifyCollectionChangedAction action)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(sender, new NotifyCollectionChangedEventArgs(action));
            }
        }
    }

    public class BudgetCategoryViewModelComparer : IEqualityComparer<BudgetCategoryViewModel>
    {
        public bool Equals(BudgetCategoryViewModel x, BudgetCategoryViewModel y)
        {
            return x.CategoryId == y.CategoryId;
        }

        public int GetHashCode(BudgetCategoryViewModel obj)
        {
            int hashCode = obj.CategoryId.GetHashCode() + obj.Name.GetHashCode() + obj.Description.GetHashCode();
            return hashCode;
        }
    }
}
