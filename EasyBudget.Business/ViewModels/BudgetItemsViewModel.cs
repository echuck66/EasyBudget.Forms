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
using System.Threading.Tasks;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class BudgetItemsViewModel : BaseViewModel
    {

        List<BudgetItem> BudgetItems { get; set; }

        public ICollection<BudgetItemViewModel> BudgetItemVMs { get; set; }

        internal BudgetItemsViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            BudgetItems = new List<BudgetItem>();
            BudgetItemVMs = new List<BudgetItemViewModel>();
        }

        internal async Task LoadBudgetItemsAsync(int categoryId)
        {
            await LoadIncomeItemsAsync(categoryId);
            await LoadExpenseItemsAsync(categoryId);
        }

        async Task LoadIncomeItemsAsync(int categoryId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _resultsCategory = await uow.GetBudgetCategoryAsync(categoryId);
                if (_resultsCategory.Successful)
                {
                    BudgetCategory category = _resultsCategory.Results;
                    var _resultsIncomeItems = await uow.GetCategoryIncomeItemsAsync(category);
                    if (_resultsIncomeItems.Successful)
                    {
                        foreach (var item in _resultsIncomeItems.Results)
                        {
                            item.budgetCategory = category;
                            item.ItemType = BudgetItemType.Income;
                            this.BudgetItems.Add(item);
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
                            WriteErrorCondition("An unknown error has occurred");
                        }
                    }
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
                        WriteErrorCondition("An unknown error has occurred");
                    }
                }
            }
        }

        async Task LoadExpenseItemsAsync(int categoryId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _resultsCategory = await uow.GetBudgetCategoryAsync(categoryId);
                if (_resultsCategory.Successful)
                {
                    BudgetCategory category = _resultsCategory.Results;
                    var _resultsIncomeItems = await uow.GetCategoryExpenseItemsAsync(category);
                    if (_resultsIncomeItems.Successful)
                    {
                        foreach (var item in _resultsIncomeItems.Results)
                        {
                            item.budgetCategory = category;
                            item.ItemType = BudgetItemType.Expense;
                            this.BudgetItems.Add(item);
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
                            WriteErrorCondition("An unknown error has occurred loading Expense Items");
                        }
                    }
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
                        WriteErrorCondition("An unknown error has occurred");
                    }
                }
            }
        }

    }

}
