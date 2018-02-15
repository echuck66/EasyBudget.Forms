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
using System.Linq;
using System.Threading.Tasks;

namespace EasyBudget.Business.ViewModels
{

    public class BudgetCategoriesViewModel : BaseViewModel, INotifyCollectionChanged
    {

        public ICollection<BudgetCategoryViewModel> BudgetCategoryVMs { get; set; }

        public string CurrentMonth {
            get
            {
                return DateTime.Now.Month.ToString();
            }
        }

        //public decimal CurrentMonthBudgetedIncome 
        //{ 
        //    get
        //    {
        //        return GetCurrentMonthBudgetedIncome();
        //    }
        //}

        //public decimal CurrentMonthBudgetedExpense
        //{
        //    get
        //    {
        //        return GetCurrentMonthBudgetedExpense();
        //    }
        //}

        //decimal GetCurrentMonthBudgetedIncome()
        //{
        //    decimal total = 0;
        //    var tot = from inc in this.BudgetCategoryVMs
        //              where inc.CategoryType == Models.BudgetCategoryType.Income
        //              select inc.Amount;
        //    total = tot.Sum();
        //    return total;
        //}

        //decimal GetCurrentMonthBudgetedExpense()
        //{
        //    decimal total = 0;
        //    var tot = from inc in this.BudgetCategoryVMs
        //              where inc.CategoryType == Models.BudgetCategoryType.Expense
        //              select inc.Amount;
        //    total = tot.Sum();
        //    return total;
        //}

        public BudgetCategoriesViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.BudgetCategoryVMs = new List<BudgetCategoryViewModel>();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        internal async Task LoadBudgetCategoriesAsync()
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
            vm.IsNew = true;

            this.BudgetCategoryVMs.Add(vm);
            OnCollectionChanged(this, NotifyCollectionChangedAction.Add);
        }

        public async Task AddNewBudgetCategoryAsync()
        {
            BudgetCategoryViewModel vm = new BudgetCategoryViewModel(this.dbFilePath);
            vm.IsNew = true;
            await Task.Run(() => this.BudgetCategoryVMs.Add(vm));
            OnCollectionChanged(this, NotifyCollectionChangedAction.Add);
        }

        public async Task<bool> DeleteBudgetCategoryAsync(BudgetCategoryViewModel vm)
        {
            bool deleted = false;
            if (vm.CanDelete && this.BudgetCategoryVMs.Contains(vm, new BudgetCategoryViewModelComparer()))
            {
                deleted = await vm.DeleteAsync();

                if (deleted) 
                {
                    await Task.Run(() => this.BudgetCategoryVMs.Remove(vm));
                    OnCollectionChanged(this, NotifyCollectionChangedAction.Remove);
                }
            }

            return deleted;
        }

        public void OnCollectionChanged(object sender, NotifyCollectionChangedAction action)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(sender, new NotifyCollectionChangedEventArgs(action));
            }
        }
    
        
    }

}
