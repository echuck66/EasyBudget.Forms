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
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using EasyBudget.Business.ViewModels;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;

// TODO Convert VMs to only expose VM and ICollection<VM> object types instead of the actual
//      underlying data models.
//      e.g. 
//      BudgetCategoriesVM should expose 
//          ICollection<BudgetCategoryVM> 
//              NOT
//          ICollection<BudgetCategory>
//
// TODO Change iOS project's ViewControllers to reflect this change
//
namespace EasyBudget.Business
{
    public sealed class EasyBudgetDataService
    {
        string dbFilePath;

        public EasyBudgetDataService(string dbFilePath)
        {
            this.dbFilePath = dbFilePath;
        }

        private async Task EnsureSystemItemsExistAsync()
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.EnsureSystemDataItemsAsync();
                if (_results.Successful)
                {
                    var _categoryCount = _results.BudgetCategoriesCount;
                }
                else
                {
                    var _categoriesExist = _results.BudgetCategoriesExist;
                }
            }
        }
        
        public async Task<BudgetCategoriesViewModel> GetBudgetCategoriesViewModelAsync()
        {
            BudgetCategoriesViewModel vm = new BudgetCategoriesViewModel(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadBudgetCategoriesAsync();

            return vm;
        }

        public async Task<BudgetCategoryViewModel> GetBudgetCategoryVM(int categoryId)
        {
            BudgetCategoryViewModel vm = new BudgetCategoryViewModel(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadBudgetCategoryDetails(categoryId);

            return vm;
        }

        //public BudgetCategoryViewModel CreateBudgetCategoryVM()
        //{
        //    var vm = new BudgetCategoryViewModel(this.dbFilePath);
        //    vm.CreateBudgetCategory();

        //    return vm;
        //}

        public async Task<BudgetItemsViewModel> GetBudgetItemsVM(int categoryId)
        {
            BudgetItemsViewModel vm = new BudgetItemsViewModel(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadBudgetItemsAsync(categoryId);

            return vm;
        }

        public async Task<IncomeItemViewModel> GetIncomeItemVMAsync(int itemId)
        {
            IncomeItemViewModel vm = new IncomeItemViewModel(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadIncomeItemAsync(itemId);

            return vm;
        }

        public async Task<ExpenseItemViewModel> GetExpenseItemVMAsync(int itemId)
        {
            ExpenseItemViewModel vm = new ExpenseItemViewModel(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadExpenseItemAsync(itemId);

            return vm;
        }
    
        public async Task<BankAccountsViewModel> GetBankAccountsViewModelAsync()
        {
            BankAccountsViewModel vm = new BankAccountsViewModel(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadBankAccountsAsync();
            return vm;
        }

        public async Task<CheckingAccountViewModel> GetCheckingAccountVMAsync(int accountId)
        {
            CheckingAccountViewModel vm = new CheckingAccountViewModel(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadCheckingAccountDetailsAsync(accountId);

            return vm;
        }

        public async Task<SavingsAccountViewModel> GetSavingsAccountVMAsync(int accountId)
        {
            SavingsAccountViewModel vm = new SavingsAccountViewModel(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadSavingsAccountDetailsAsync(accountId);

            return vm;
        }

        //public async Task<bool> DeleteBudgetCategoryAsync(BudgetCategory category)
        //{
        //    bool _success = false;  
        //    using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
        //    {
        //        var _resultsIncomeItems = await uow.GetCategoryIncomeItemsAsync(category);
        //        if (_resultsIncomeItems.Successful)
        //        {
        //            foreach(IncomeItem itm in _resultsIncomeItems.Results)
        //            {
        //                var _resDel = await uow.DeleteIncomeItemAsync(itm);
        //            }
        //        }
        //        var _resultsExpenseItems = await uow.GetCategoryExpenseItemsAsync(category);
        //        if (_resultsExpenseItems.Successful)
        //        {
        //            foreach(ExpenseItem itm in _resultsExpenseItems.Results)
        //            {
        //                var _resDel = await uow.DeleteExpenseItemAsync(itm);
        //            }
        //        }
        //        var _resultsDeleteCategory = await uow.DeleteBudgetCategoryAsync(category);
        //        _success = _resultsDeleteCategory.Successful;
        //    }
        //    return _success;
        //}
    
        public CheckingAccountViewModel CreateCheckingAccountVM()
        {
            var vm = new CheckingAccountViewModel(this.dbFilePath);
            vm.CreateCheckingAccount();

            return vm;
        }

        public SavingsAccountViewModel CreateSavingsAccountVM()
        {
            var vm = new SavingsAccountViewModel(this.dbFilePath);
            vm.CreateSavingsAccount();

            return vm;
        }
    }

}
