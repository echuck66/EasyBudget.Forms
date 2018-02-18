﻿//
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

namespace EasyBudget.Business
{
    public sealed class EasyBudgetDataService
    {
        
        public static EasyBudgetDataService Instance { get; private set; }

        string dbFilePath;
    
        public EasyBudgetDataService(IDataServiceHelper serviceHelper)
        {
            if (Instance != null)
                throw new Exception("Only one instance of EasyBudgetDataService is allowed!");
            Instance = this;
            this.dbFilePath = serviceHelper.DbFilePath;

        }

        public async Task<BudgetCategoriesViewModel> GetBudgetCategoriesViewModelAsync()
        {
            BudgetCategoriesViewModel vm = new BudgetCategoriesViewModel(this.dbFilePath);
            await vm.LoadBudgetCategoriesAsync();

            return vm;
        }

        public async Task<BudgetCategoryViewModel> GetBudgetCategoryVM(int categoryId)
        {
            BudgetCategoryViewModel vm = new BudgetCategoryViewModel(this.dbFilePath);
            await vm.LoadBudgetCategoryDetails(categoryId);

            return vm;
        }


        public async Task<BudgetItemsViewModel> GetBudgetItemsVM(int categoryId)
        {
            BudgetItemsViewModel vm = new BudgetItemsViewModel(this.dbFilePath);
            await vm.LoadBudgetItemsAsync(categoryId);

            return vm;
        }

        //public async Task<IncomeItemViewModel> GetIncomeItemVMAsync(int itemId)
        //{
        //    IncomeItemViewModel vm = new IncomeItemViewModel(this.dbFilePath);
        //    await vm.LoadIncomeItemAsync(itemId);

        //    return vm;
        //}

        //public async Task<ExpenseItemViewModel> GetExpenseItemVMAsync(int itemId)
        //{
        //    ExpenseItemViewModel vm = new ExpenseItemViewModel(this.dbFilePath);
        //    await vm.LoadExpenseItemAsync(itemId);

        //    return vm;
        //}
    
        public async Task<BankAccountsViewModel> GetBankAccountsViewModelAsync()
        {
            BankAccountsViewModel vm = new BankAccountsViewModel(this.dbFilePath);
            await vm.LoadBankAccountsAsync();
            return vm;
        }

        //public async Task<CheckingAccountViewModel> GetCheckingAccountVMAsync(int accountId)
        //{
        //    CheckingAccountViewModel vm = new CheckingAccountViewModel(this.dbFilePath);
        //    await vm.LoadCheckingAccountDetailsAsync(accountId);

        //    return vm;
        //}

        //public async Task<SavingsAccountViewModel> GetSavingsAccountVMAsync(int accountId)
        //{
        //    SavingsAccountViewModel vm = new SavingsAccountViewModel(this.dbFilePath);
        //    await vm.LoadSavingsAccountDetailsAsync(accountId);

        //    return vm;
        //}

    }

}
