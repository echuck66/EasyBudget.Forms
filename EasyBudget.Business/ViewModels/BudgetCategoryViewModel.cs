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
        BudgetCategory Category { get; set; }

        public int CategoryId 
        {
            get
            {
                return Category.id;
            }
        }

        public string Name 
        {
            get
            {
                return Category.categoryName;
            }
            set
            {
                if (Category.categoryName != value)
                {
                    Category.categoryName = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        public string Description 
        {
            get
            {
                return Category.description;
            }
            set
            {
                if (Category.description != value)
                {
                    Category.description = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                }
            }
        }

        public decimal Amount 
        {
            get{
                return Category.budgetAmount;
            }
            set
            {
                if (Category.budgetAmount != value)
                {
                    Category.budgetAmount = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
                }
            }
        }

        public AppIcon CategoryIcon 
        {
            get
            {
                return Category.categoryIcon;
            }
            set
            {
                if (Category.categoryIcon != value)
                {
                    Category.categoryIcon = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryId)));
                }
            }
        }

        public BudgetCategoryType CategoryType 
        {
            get 
            {
                return Category.categoryType;
            }
            set
            {
                if (Category.categoryType != value)
                {
                    Category.categoryType = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryType)));
                }
            }
        }

        public ICollection<BudgetItemViewModel> BudgetItemVMs { get; set; }

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
            Category = category;

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

            //BudgetCategory _category;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (this.IsNew)
                {
                    var _resultsSaveNew = await uow.AddBudgetCategoryAsync(Category);
                    _saveOk = _resultsSaveNew.Successful;
                    if (_saveOk)
                    {
                        this.IsDirty = false;
                        this.IsNew = false;
                        this.CanEdit = true;
                        this.CanDelete = true;
                    }
                    else
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

                    var _resultsUpdate = await uow.UpdateBudgetCategoryAsync(Category);
                    _saveOk = _resultsUpdate.Successful;
                    if (_saveOk)
                    {
                        this.IsDirty = false;
                        this.IsNew = false;
                        this.CanEdit = true;
                        this.CanDelete = true;
                    }
                    else
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
        }

        public BudgetItemViewModel AddBudgetItem()
        {
            BudgetItemViewModel item = new BudgetItemViewModel(this.dbFilePath);
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
