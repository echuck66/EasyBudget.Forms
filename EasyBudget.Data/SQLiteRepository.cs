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
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using EasyBudget.Models.DataModels;
using SQLite;

namespace EasyBudget.Data
{
    public class SQLiteRepository : IEasyBudgetRepository
    {
        SQLite.SQLiteAsyncConnection connection;
        //object asyncLock;

        public SQLiteRepository(string dbFilePath)
        {
            InitializeTables(dbFilePath);
            connection = new SQLiteAsyncConnection(dbFilePath);
        }

        public void Dispose()
        {
            connection = null;
        }

        private void InitializeTables(string dbfilePath)
        {
            using (SQLite.SQLiteConnection conn = new SQLiteConnection(dbfilePath))
            {
                
                if (!TableExists<BankAccountFundsTransfer>(conn))
                    conn.CreateTable<BankAccountFundsTransfer>();
                if (!TableExists<BudgetCategory>(conn))
                    conn.CreateTable<BudgetCategory>();
                if (!TableExists<CheckingAccount>(conn))
                    conn.CreateTable<CheckingAccount>();
                if (!TableExists<SavingsAccount>(conn))
                    conn.CreateTable<SavingsAccount>();
                if (!TableExists<CheckingDeposit>(conn))
                    conn.CreateTable<CheckingDeposit>();
                if (!TableExists<CheckingWithdrawal>(conn))
                    conn.CreateTable<CheckingWithdrawal>();
                if (!TableExists<SavingsDeposit>(conn))
                    conn.CreateTable<SavingsDeposit>();
                if (!TableExists<SavingsWithdrawal>(conn))
                    conn.CreateTable<SavingsWithdrawal>();
                if (!TableExists<IncomeItem>(conn))
                    conn.CreateTable<IncomeItem>();
                if (!TableExists<ExpenseItem>(conn))
                    conn.CreateTable<ExpenseItem>();
                
            }
        }

        private bool TableExists<T>(SQLiteConnection connection) 
        { 
            const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?"; 
            var cmd = connection.CreateCommand(cmdText, typeof(T).Name); 
            return cmd.ExecuteScalar<string>() != null; 
        }

        public async Task AddBankAccountFundsTransferAsync(BankAccountFundsTransfer fundsTransfer)
        {
            var inserted = await connection.InsertAsync(fundsTransfer);
        }

        public async Task<BudgetCategory> AddBudgetCategoryAsync(BudgetCategory category)
        {
            var inserted = await connection.InsertAsync(category);
            category.IsNew = false;
            return category;
        }

        public async Task<CheckingAccount> AddCheckingAccountAsync(CheckingAccount account)
        {
            var inserted = await connection.InsertAsync(account);
            account.IsNew = false;
            return account;
        }

        public async Task<CheckingDeposit> AddCheckingDepositAsync(CheckingDeposit deposit)
        {
            var inserted = await connection.InsertAsync(deposit);
            deposit.IsNew = false;
            return deposit;
        }

        public async Task<CheckingWithdrawal> AddCheckingWithdrawalAsync(CheckingWithdrawal withdrawal)
        {
            var inserted = await connection.InsertAsync(withdrawal);
            withdrawal.IsNew = false;
            return withdrawal;
        }

        public async Task<ExpenseItem> AddExpenseItemAsync(ExpenseItem expense)
        {
            var inserted = await connection.InsertAsync(expense);
            expense.IsNew = false;
            return expense;
        }

        public async Task<IncomeItem> AddIncomeItemAsync(IncomeItem income)
        {
            var inserted = await connection.InsertAsync(income);
            income.IsNew = false;
            return income;
        }

        public async Task<SavingsAccount> AddSavingsAccountAsync(SavingsAccount account)
        {
            var inserted = await connection.InsertAsync(account);
            account.IsNew = false;
            return account;
        }

        public async Task<SavingsDeposit> AddSavingsDepositAsync(SavingsDeposit deposit)
        {
            var inserted = await connection.InsertAsync(deposit);
            deposit.IsNew = false;
            return deposit;
        }

        public async Task<SavingsWithdrawal> AddSavingsWithdrawalAsync(SavingsWithdrawal withdrawal)
        {
            var inserted = await connection.InsertAsync(withdrawal);
            withdrawal.IsNew = false;
            return withdrawal;
        }

        public async Task DeleteBudgetCategoryAsync(BudgetCategory category)
        {
            var deleted = await connection.DeleteAsync(category);
        }

        public async Task DeleteCheckingAccountAsync(CheckingAccount account)
        {
            var deleted = await connection.DeleteAsync(account);
        }

        public async Task DeleteCheckingDepositAsync(CheckingDeposit deposit)
        {
            var deleted = await connection.DeleteAsync(deposit);
        }

        public async Task DeleteCheckingWithdrawalAsync(CheckingWithdrawal withdrawal)
        {
            var deleted = await connection.DeleteAsync(withdrawal);
        }

        public async Task DeleteExpenseItemAsync(ExpenseItem expense)
        {
            var deleted = await connection.DeleteAsync(expense);
        }

        public async Task DeleteIncomeItemAsync(IncomeItem income)
        {
            var deleted = await connection.DeleteAsync(income);
        }

        public async Task DeleteSavingsAccountAsync(SavingsAccount account)
        {
            var deleted = await connection.DeleteAsync(account);
        }

        public async Task DeleteSavingsDepositAsync(SavingsDeposit deposit)
        {
            var deleted = await connection.DeleteAsync(deposit);
        }

        public async Task<ICollection<BudgetCategory>> GetAllCategoriesAsync()
        {
            var categories = await connection.Table<BudgetCategory>().ToListAsync();

            return categories as ICollection<BudgetCategory>;
        }

        public async Task<ICollection<CheckingAccount>> GetAllCheckingAccountsAsync()
        {
            var accounts = await connection.Table<CheckingAccount>().ToListAsync();

            return accounts as ICollection<CheckingAccount>;
        }

        public async Task<ICollection<SavingsAccount>> GetAllSavingsAccountsAsync()
        {
            var accounts = await connection.Table<SavingsAccount>().ToListAsync();

            return accounts as ICollection<SavingsAccount>;
        }

        public async Task<BankAccountFundsTransfer> GetBankAccountFundsTransfer(int transferId)
        {
            BankAccountFundsTransfer transfer = await connection.FindAsync<BankAccountFundsTransfer>(transferId);

            return transfer;
        }

        public async Task<BudgetCategory> GetBudgetCategoryAsync(int id)
        {
            BudgetCategory category = await connection.FindAsync<BudgetCategory>(id);

            return category;
        }

        public async Task<CheckingAccount> GetCheckingAccountAsync(int id)
        {
            CheckingAccount account = await connection.FindAsync<CheckingAccount>(id);

            return account;
        }

        public async Task<CheckingDeposit> GetCheckingDepositAsync(int id)
        {
            CheckingDeposit deposit = await connection.FindAsync<CheckingDeposit>(id);

            return deposit;
        }

        public async Task<ICollection<CheckingDeposit>> GetCheckingDepositsByDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            var deps = from d in connection.Table<CheckingDeposit>()
                       where d.checkingAccountId == accountId &&
                       d.transactionDate >= fromDate && d.transactionDate <= toDate
                       select d;
            
            List<CheckingDeposit> deposits = await deps?.ToListAsync();

            return deposits as ICollection<CheckingDeposit>;
        }

        public async Task<CheckingWithdrawal> GetCheckingWithdrawalAsync(int id)
        {
            return await connection.FindAsync<CheckingWithdrawal>(id);
        }

        public async Task<ICollection<CheckingWithdrawal>> GetCheckingWithdrawalsByDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            var wids = from w in connection.Table<CheckingWithdrawal>()
                       where w.checkingAccountId == accountId &&
                       w.transactionDate >= fromDate && w.transactionDate <= toDate
                       select w;
            List<CheckingWithdrawal> withdrawals = await wids?.ToListAsync();

            return withdrawals as ICollection<CheckingWithdrawal>;
        }

        public async Task<ExpenseItem> GetExpenseItemAsync(int id)
        {
            return await connection.FindAsync<ExpenseItem>(id);
        }

        public async Task<ICollection<ExpenseItem>> GetExpenseItemsForBudgetCategoryAsync(int categoryId)
        {
            var xps = from x in connection.Table<ExpenseItem>()
                       where x.budgetCategoryId == categoryId 
                       select x;
            List<ExpenseItem> expenses = await xps?.ToListAsync();

            return expenses as ICollection<ExpenseItem>;
        }

        public async Task<IncomeItem> GetIncomeItemAsync(int id)
        {
            return await connection.FindAsync<IncomeItem>(id);
        }

        public async Task<ICollection<IncomeItem>> GetIncomeItemsForBudgetCategoryAsync(int categoryId)
        {
            var inc = from i in connection.Table<IncomeItem>()
                      where i.budgetCategoryId == categoryId
                      select i;
            List<IncomeItem> income = await inc?.ToListAsync();

            return income as ICollection<IncomeItem>;
        }

        public Task<ICollection<BudgetCategory>> GetMatchingCategoriesAsync(string searchText)
        {
            throw new NotImplementedException();
        }

        public async Task<SavingsAccount> GetSavingsAccountAsync(int id)
        {
            return await connection.FindAsync<SavingsAccount>(id);
        }

        public async Task<SavingsDeposit> GetSavingsDepositAsync(int id)
        {
            return await connection.FindAsync<SavingsDeposit>(id);
        }

        public async Task<ICollection<SavingsDeposit>> GetSavingsDepositsByDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            var deps = from d in connection.Table<SavingsDeposit>()
                       where d.savingsAccountId == accountId &&
                       d.transactionDate >= fromDate && d.transactionDate <= toDate
                       select d;
            List<SavingsDeposit> deposits = await deps?.ToListAsync();

            return deposits as ICollection<SavingsDeposit>;
        }

        public async Task<SavingsWithdrawal> GetSavingsWithdrawalAsync(int id)
        {
            return await connection.FindAsync<SavingsWithdrawal>(id);
        }

        public async Task<ICollection<SavingsWithdrawal>> GetSavingsWithdrawalsByDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            var wids = from w in connection.Table<SavingsWithdrawal>()
                       where w.savingsAccountId == accountId &&
                       w.transactionDate >= fromDate && w.transactionDate <= toDate
                       select w;
            List<SavingsWithdrawal> withdrawals = await wids?.ToListAsync();

            return withdrawals as ICollection<SavingsWithdrawal>;
        }

        public Task<int> SaveChangesAsync()
        {
            Task<int> task = Task<int>.Factory.StartNew(() => 1);
            return task;
        }

        public async Task UpdateBankAccountFundsTransferAsync(BankAccountFundsTransfer fundsTransfer)
        {
            var updated = await connection.UpdateAsync(fundsTransfer);
        }

        public async Task UpdateBudgetCategoryAsync(BudgetCategory category)
        {
            var updated = await connection.UpdateAsync(category);
        }

        public async Task UpdateCheckingAccountAsync(CheckingAccount account)
        {
            var updated = await connection.UpdateAsync(account);
        }

        public async Task UpdateCheckingDepositAsync(CheckingDeposit deposit)
        {
            var updated = await connection.UpdateAsync(deposit);
        }

        public async Task UpdateCheckingWithdrawalAsync(CheckingWithdrawal withdrawal)
        {
            var updated = await connection.UpdateAsync(withdrawal);
        }

        public async Task UpdateExpenseItemAsync(ExpenseItem expense)
        {
            var updated = await connection.UpdateAsync(expense);
        }

        public async Task UpdateIncomeItemAsync(IncomeItem income)
        {
            var updated = await connection.UpdateAsync(income);
        }

        public async Task UpdateSavingsAccountAsync(SavingsAccount account)
        {
            var updated = await connection.UpdateAsync(account);
        }

        public async Task UpdateSavingsDepositAsync(SavingsDeposit deposit)
        {
            var updated = await connection.UpdateAsync(deposit);
        }

        public async Task UpdateSavingsWithdrawalAsync(SavingsWithdrawal withdrawal)
        {
            var updated = await connection.UpdateAsync(withdrawal);
        }
    }
}
