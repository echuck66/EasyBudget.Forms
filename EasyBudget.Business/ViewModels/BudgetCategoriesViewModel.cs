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
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class BudgetCategoriesViewModel : BaseViewModel, INotifyPropertyChanged
    {

        public ObservableCollection<BudgetCategoryViewModel> BudgetCategoryVMs { get; set; }

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

        public BudgetCategoriesViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.BudgetCategoryVMs = new ObservableCollection<BudgetCategoryViewModel>();
            this.CurrentMonth = DateTime.Now.Month;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal async Task LoadVMAsync()
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllBudgetCategoriesAsync();
                if (_results.Successful)
                {
                    foreach (var category in _results.Results)
                    {
                        var vm = new BudgetCategoryViewModel(this.dbFilePath);
                        await vm.PopulateVMAsync(category);
                        this.BudgetCategoryVMs.Add(vm);
                    }
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

        public void AddNewBudgetCategory()
        {
            BudgetCategoryViewModel vm = new BudgetCategoryViewModel(this.dbFilePath);
            BudgetCategory category = new BudgetCategory();
            Task.Run(() => vm.PopulateVMAsync(category));
            vm.CanDelete = false;
            vm.CanEdit = true;
            vm.IsNew = true;
            this.BudgetCategoryVMs.Add(vm);
        }

        public async Task AddNewBudgetCategoryAsync()
        {
            BudgetCategoryViewModel vm = new BudgetCategoryViewModel(this.dbFilePath);
            BudgetCategory category = new BudgetCategory();
            await vm.PopulateVMAsync(category);
            vm.IsNew = true;
            vm.CanDelete = false;
            vm.CanEdit = true;
            this.BudgetCategoryVMs.Add(vm);
        }

        public async Task<bool> DeleteBudgetCategoryAsync(BudgetCategoryViewModel vm)
        {
            bool deleted = false;
            if (vm.CanDelete && this.BudgetCategoryVMs.Contains(vm, new BudgetCategoryViewModelComparer()))
            {
                deleted = await vm.DeleteAsync();

                if (deleted) 
                {
                    this.BudgetCategoryVMs.Remove(vm);
                }
            }

            return deleted;
        }
    }

}
