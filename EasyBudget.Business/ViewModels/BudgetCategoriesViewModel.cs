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
using EasyBudget.Business.ChartModels;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class BudgetCategoriesViewModel : BaseViewModel, INotifyPropertyChanged, IDisposable
    {
        public ObservableCollection<BudgetCategoryViewModel> BudgetCategories { get; set; }
        public ObservableCollection<Grouping<string, BudgetCategoryViewModel>> BudgetCategoriesGrouped { get; set; }

        BudgetCategoryViewModel _SelectedBudgetCategory;
        public BudgetCategoryViewModel SelectedBudgetCategory
        {
            get
            {
                return _SelectedBudgetCategory;
            }
            set
            {
                _SelectedBudgetCategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedBudgetCategory)));
            }
        }

        int _CurrentMonth;
        public int CurrentMonth {
            get
            {
                return _CurrentMonth;
            }
            set
            {
                if (_CurrentMonth != value && value > 0 && value < 13)
                {
                    _CurrentMonth = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentMonth)));
                }
            }
        }

        public decimal TotalBudgetedExpenses
        {
            get
            {
                var _total = this.BudgetCategories.Where(c => c.CategoryType == Models.BudgetCategoryType.Expense).Sum(c => c.Amount);
                return _total;
            }
        }

        public decimal TotalBudgetedIncome
        {
            get
            {
                var _total = this.BudgetCategories.Where(c => c.CategoryType == Models.BudgetCategoryType.Income).Sum(c => c.Amount);
                return _total;
            }
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        public BudgetCategoriesViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.BudgetCategories = new ObservableCollection<BudgetCategoryViewModel>();
            this.BudgetCategoriesGrouped = new ObservableCollection<Grouping<string, BudgetCategoryViewModel>>();
            this.CurrentMonth = DateTime.Now.Month;
        }

        internal async Task LoadVMAsync()
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllBudgetCategoriesAsync();
                var _tempList = new List<BudgetCategoryViewModel>();
                if (_results.Successful)
                {
                    foreach (var category in _results.Results)
                    {
                        var vm = new BudgetCategoryViewModel(this.dbFilePath);
                        vm.CanEdit = true;
                        vm.CanDelete = true;
                        await vm.PopulateVMAsync(category);
                        this.BudgetCategories.Add(vm);
                    }
                    await GroupCategoriesAsync();
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
                        WriteErrorCondition("An unknown error has occurred");
                    }
                }

                                    
            }
        }

        public void GroupCategories()
        {
            var grouped = from cat in this.BudgetCategories
                          orderby cat.Name
                          group cat by cat.CategoryType into Group
                          select new Grouping<string, BudgetCategoryViewModel>(Group.Key.ToString(), Group);
            
            this.BudgetCategoriesGrouped = new ObservableCollection<Grouping<string, BudgetCategoryViewModel>>(grouped.OrderBy(g => g.Key));
        }

        public async Task GroupCategoriesAsync()
        {
            var grouped = from cat in this.BudgetCategories
                          orderby cat.Name
                          group cat by cat.CategoryType into Group
                          select new Grouping<string, BudgetCategoryViewModel>(Group.Key.ToString(), Group);

            this.BudgetCategoriesGrouped = await Task.Run(() => new ObservableCollection<Grouping<string, BudgetCategoryViewModel>>(grouped.OrderBy(g => g.Key)));
        }

        public void AddNewBudgetCategory()
        {
            BudgetCategoryViewModel vm = new BudgetCategoryViewModel(this.dbFilePath);
            BudgetCategory category = new BudgetCategory();
            Task.Run(() => vm.PopulateVMAsync(category));
            vm.CanDelete = false;
            vm.CanEdit = true;
            vm.IsNew = true;
            this.BudgetCategories.Add(vm);
            GroupCategories();
            this.SelectedBudgetCategory = vm;
        }

        public async Task AddNewBudgetCategoryAsync()
        {
            BudgetCategoryViewModel vm = new BudgetCategoryViewModel(this.dbFilePath);
            BudgetCategory category = new BudgetCategory();
            await vm.PopulateVMAsync(category);
            vm.IsNew = true;
            vm.CanDelete = false;
            vm.CanEdit = true;
            this.BudgetCategories.Add(vm);
            await GroupCategoriesAsync();
            this.SelectedBudgetCategory = vm;
        }

        public async Task<bool> DeleteBudgetCategoryAsync(BudgetCategoryViewModel vm)
        {
            bool deleted = false;
            var itemList = new List<BudgetCategoryViewModel>();

            if (vm.CanDelete && this.BudgetCategories.Contains(vm, new BudgetCategoryViewModelComparer()))
            {
                deleted = await vm.DeleteAsync();
                if (deleted) 
                {
                    this.BudgetCategories.Remove(vm);
                    await GroupCategoriesAsync();
                }
            }
            else
            {
                this.WriteErrorCondition("Unable to locate provided item in the source collection");
            }

            return deleted;
        }

        public void Dispose()
        {
            foreach(BudgetCategoryViewModel vm in this.BudgetCategories)
            {
                vm.Dispose();
            }
        }

        public override IChartDataPack GetChartData()
        {
            throw new NotImplementedException();
        }
    }

}
