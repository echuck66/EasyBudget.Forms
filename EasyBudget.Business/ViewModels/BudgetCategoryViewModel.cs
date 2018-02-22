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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class BudgetCategoryViewModel : BaseViewModel, INotifyPropertyChanged
    {
        BudgetCategory model { get; set; }

        public int CategoryId 
        {
            get
            {
                return model.id;
            }
        }

        public string Name 
        {
            get
            {
                return model.categoryName;
            }
            set
            {
                if (model.categoryName != value)
                {
                    model.categoryName = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
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
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                }
            }
        }

        public decimal Amount 
        {
            get{
                return model.budgetAmount;
            }
            set
            {
                if (model.budgetAmount != value)
                {
                    model.budgetAmount = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
                }
            }
        }

        public AppIcon CategoryIcon 
        {
            get
            {
                return model.categoryIcon;
            }
            set
            {
                if (model.categoryIcon != value)
                {
                    model.categoryIcon = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryId)));
                }
            }
        }

        public BudgetCategoryType CategoryType 
        {
            get 
            {
                return model.categoryType;
            }
            set
            {
                if (model.categoryType != value)
                {
                    model.categoryType = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryType)));
                }
            }
        }

        public bool CanAddItems
        {
            get
            {
                return !this.IsNew;
            }
        }

        BudgetItemViewModel _SelectedBudgetItem;
        public BudgetItemViewModel SelectedBudgetItem
        {
            get
            {
                return _SelectedBudgetItem;
            }
            set
            {
                _SelectedBudgetItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedBudgetItem)));
            }
        }

        public ObservableCollection<BudgetCategoryType> CategoryTypes { get; set; }

        public ObservableCollection<BudgetItemViewModel> BudgetItems { get; set; }

        public BudgetCategoryViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.BudgetItems = new ObservableCollection<BudgetItemViewModel>();
            this.CategoryTypes = new ObservableCollection<BudgetCategoryType>() {
                BudgetCategoryType.Income,
                BudgetCategoryType.Expense
            };
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        internal async Task PopulateVMAsync(BudgetCategory category)
        {
            model = category;

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
                                this.BudgetItems.Add(vm);
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
                                this.BudgetItems.Add(vm);
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

        internal async Task LoadVMAsync(int categoryId)
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

            if (this.BudgetItems.Count == 0)
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

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (this.IsNew)
                {
                    var _resultsSaveNew = await uow.AddBudgetCategoryAsync(model);
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

                    var _resultsUpdate = await uow.UpdateBudgetCategoryAsync(model);
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

            if (_saveOk)
            {
                foreach(var item in BudgetItems)
                {
                    if (item.IsDirty)
                    {
                        await item.SaveChangesAsync();
                    }
                }
            }
        }

        public BudgetItemViewModel AddBudgetItem()
        {
            BudgetItemViewModel vm = new BudgetItemViewModel(this.dbFilePath);
            vm.ItemType = this.CategoryType == BudgetCategoryType.Expense ? BudgetItemType.Expense : BudgetItemType.Income;
            vm.IsNew = true;
            this.BudgetItems.Add(vm);

            return vm;
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
