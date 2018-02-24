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
using System.Linq;
using System.Threading.Tasks;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class BudgetCategoryViewModel : BaseViewModel, INotifyPropertyChanged, IDisposable
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

        decimal _Amount;
        public decimal Amount 
        {
            get
            {
                decimal _bugetTotal = 0M;
                foreach (var itm in this.BudgetItems)
                {
                    _bugetTotal += itm.BudgetedAmount;
                }
                if (_Amount != _bugetTotal)
                {
                    _Amount = _bugetTotal;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
                }
                return _bugetTotal;
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
                                item.budgetCategoryId = category.id;
                                var vm = new BudgetItemViewModel(this.dbFilePath);
                                await vm.PopulateVMAsync(item);
                                vm.ItemUpdated += OnItemUpdate;
                                vm.IsNew = false;
                                vm.CanEdit = true;
                                vm.CanDelete = true;
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
                                item.budgetCategoryId = category.id;
                                var vm = new BudgetItemViewModel(this.dbFilePath);
                                await vm.PopulateVMAsync(item);
                                vm.ItemUpdated += OnItemUpdate;
                                vm.IsNew = false;
                                vm.CanEdit = true;
                                vm.CanDelete = true;
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
                                WriteErrorCondition("An unknown error occurred while attempting to delete record");
                            }
                        }
                    }
                }
            }
            else
            {
                WriteErrorCondition("All related Budget Items must first be deleted");
            }

            return deleted;
        }

        public async Task<bool> DelteBudgetItemAsync(BudgetItemViewModel vm)
        {
            bool deleted = false;
            var itemList = new List<BudgetItemViewModel>();

            if (vm.CanDelete && this.BudgetItems.Contains(vm, new BudgetItemViewModelComparer()))
            {
                deleted = await vm.DeleteAsync();
                if (deleted)
                {
                    this.BudgetItems.Remove(vm);
                }
            }
            else
            {
                this.WriteErrorCondition("Unable to locate provided item in the source collection");
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

        public async Task<BudgetItemViewModel> AddBudgetItemAsync()
        {
            BudgetItemViewModel vm = new BudgetItemViewModel(this.dbFilePath);
            vm.ItemUpdated += OnItemUpdate;
            BudgetItem item = null;
            if (this.CategoryType == BudgetCategoryType.Income)
            {
                item = new IncomeItem();
                item.ItemType = BudgetItemType.Income;
            }
            else
            {
                item = new ExpenseItem();
                item.ItemType = BudgetItemType.Expense;
            }
            item.budgetCategory = model;
            item.budgetCategoryId = model.id;
            item.StartDate = DateTime.Now;
            item.recurring = true;
            item.frequency = Frequency.Monthly;
            await vm.PopulateVMAsync(item);
            vm.IsNew = true;
            this.BudgetItems.Add(vm);
            this.SelectedBudgetItem = vm;
            return vm;
        }

        void OnItemUpdate(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
        }

        public void Dispose()
        {
            foreach(var item in this.BudgetItems)
            {
                item.ItemUpdated -= OnItemUpdate;
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
