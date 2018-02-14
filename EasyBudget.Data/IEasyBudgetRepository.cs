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

namespace EasyBudget.Data
{
    public interface IEasyBudgetRepository : IDisposable
    {
        
        Task<BudgetCategory> GetBudgetCategoryAsync(int id);

        Task<ICollection<BudgetCategory>> GetAllCategoriesAsync();

        Task<ICollection<BudgetCategory>> GetMatchingCategoriesAsync(string searchText);

        Task<CheckingAccount> GetCheckingAccountAsync(int id);

        Task<ICollection<CheckingAccount>> GetAllCheckingAccountsAsync();

        Task<CheckingDeposit> GetCheckingDepositAsync(int id);

        Task<CheckingWithdrawal> GetCheckingWithdrawalAsync(int id);

        Task<ICollection<CheckingDeposit>> GetCheckingDepositsByDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate);

        Task<ICollection<CheckingWithdrawal>> GetCheckingWithdrawalsByDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate);

        Task<SavingsAccount> GetSavingsAccountAsync(int id);

        Task<ICollection<SavingsAccount>> GetAllSavingsAccountsAsync();

        Task<SavingsDeposit> GetSavingsDepositAsync(int id);

        Task<SavingsWithdrawal> GetSavingsWithdrawalAsync(int id);

        Task<ICollection<SavingsDeposit>> GetSavingsDepositsByDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate);

        Task<ICollection<SavingsWithdrawal>> GetSavingsWithdrawalsByDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate);

        Task<ExpenseItem> GetExpenseItemAsync(int id);

        Task<ICollection<IncomeItem>> GetIncomeItemsForBudgetCategoryAsync(int categoryId);

        Task<IncomeItem> GetIncomeItemAsync(int id);

        Task<ICollection<ExpenseItem>> GetExpenseItemsForBudgetCategoryAsync(int categoryId);

        Task<CheckingAccount> AddCheckingAccountAsync(CheckingAccount account);

        Task<SavingsAccount> AddSavingsAccountAsync(SavingsAccount account);

        Task<BudgetCategory> AddBudgetCategoryAsync(BudgetCategory category);

        Task<CheckingDeposit> AddCheckingDepositAsync(CheckingDeposit deposit);

        Task<CheckingWithdrawal> AddCheckingWithdrawalAsync(CheckingWithdrawal withdrawal);

        Task<SavingsWithdrawal> AddSavingsWithdrawalAsync(SavingsWithdrawal withdrawal);

        Task<SavingsDeposit> AddSavingsDepositAsync(SavingsDeposit deposit);

        Task<ExpenseItem> AddExpenseItemAsync(ExpenseItem expense);

        Task<IncomeItem> AddIncomeItemAsync(IncomeItem income);

        Task DeleteCheckingAccountAsync(CheckingAccount account);

        Task DeleteSavingsAccountAsync(SavingsAccount account);

        Task DeleteBudgetCategoryAsync(BudgetCategory category);

        Task DeleteCheckingDepositAsync(CheckingDeposit deposit);

        Task DeleteCheckingWithdrawalAsync(CheckingWithdrawal withdrawal);

        Task DeleteSavingsDepositAsync(SavingsDeposit deposit);

        Task DeleteExpenseItemAsync(ExpenseItem expense);

        Task DeleteIncomeItemAsync(IncomeItem income);

        Task UpdateCheckingAccountAsync(CheckingAccount account);

        Task UpdateSavingsAccountAsync(SavingsAccount account);

        Task UpdateBudgetCategoryAsync(BudgetCategory category);

        Task UpdateCheckingDepositAsync(CheckingDeposit deposit);

        Task UpdateCheckingWithdrawalAsync(CheckingWithdrawal withdrawal);

        Task UpdateSavingsDepositAsync(SavingsDeposit deposit);

        Task UpdateSavingsWithdrawalAsync(SavingsWithdrawal withdrawal);

        Task UpdateExpenseItemAsync(ExpenseItem expense);

        Task UpdateIncomeItemAsync(IncomeItem income);

        Task<BankAccountFundsTransfer> GetBankAccountFundsTransfer(int transferId);

        Task AddBankAccountFundsTransferAsync(BankAccountFundsTransfer fundsTransfer);

        Task UpdateBankAccountFundsTransferAsync(BankAccountFundsTransfer fundsTransfer);

        Task<int> SaveChangesAsync();
    }
}
