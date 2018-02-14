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
using System.Linq;
using System.Threading.Tasks;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;
using SQLite;

namespace EasyBudget.Data
{
    public class EasyBudgetRepository : IEasyBudgetRepository
    {
        private EasyBudgetContext context;

        public EasyBudgetRepository(string dbFilePath)
        {
            context = new EasyBudgetContext(dbFilePath);
        }

        public EasyBudgetRepository(EasyBudgetContext testContext)
        {
            context = testContext;
        }

        public async Task<BudgetCategory> AddBudgetCategoryAsync(BudgetCategory category)
        {
            if (!context.BudgetCategory.Any(c => c.id == category.id && c.categoryName == category.categoryName))
            {
                await Task.Run(() => context.BudgetCategory.Add(category));
            }
            else if (context.BudgetCategory.Any(c => c.categoryName == category.categoryName))
            {
                throw new Exception("An existing category already exists with the same name");
            }
            else
            {
                throw new Exception("A category already exists with the same Primary Key value");
            }
            return category;
        }

        public async Task<CheckingAccount> AddCheckingAccountAsync(CheckingAccount account)
        {
            if (!context.CheckingAccount.Any(c => c.id == account.id))
            {
                await Task.Run(() => context.CheckingAccount.Add(account));
                await Task.Run(() => context.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Added);
            }
            else
            {
                throw new Exception("A Checking Account already exists with the same Primary Key value");
            }
            return account;
        }

        public async Task<CheckingDeposit> AddCheckingDepositAsync(CheckingDeposit deposit)
        {
            if (!context.CheckingDeposit.Any(d => d.id == deposit.id))
            {
                await Task.Run(() => context.CheckingDeposit.Add(deposit));
                await Task.Run(() => context.Entry(deposit).State = Microsoft.EntityFrameworkCore.EntityState.Added);
                await Task.Run(() => context.Entry(deposit.checkingAccount).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
                if (deposit.budgetIncome != null)    
                    await Task.Run(() => context.Entry(deposit.budgetIncome).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged);
            }
            else
            {
                throw new Exception("A deposit record already exists with the same Primary Key value");
            }
            return deposit;
        }

        public async Task<CheckingWithdrawal> AddCheckingWithdrawalAsync(CheckingWithdrawal withdrawal)
        {
            if (!context.CheckingWithdrawal.Any(w => w.id == withdrawal.id))
            {
                await Task.Run(() => context.CheckingWithdrawal.Add(withdrawal));
                await Task.Run(() => context.Entry(withdrawal.checkingAccount).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
                if (withdrawal.budgetExpense != null)
                    await Task.Run(() => context.Entry(withdrawal.budgetExpense).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged);
            }
            else
            {
                throw new Exception("A withdrawal record already exists with the same Primary Key value");
            }
            return withdrawal;
        }

        public async Task<ExpenseItem> AddExpenseItemAsync(ExpenseItem expense)
        {
            if (!context.ExpenseItem.Any(x => x.id == expense.id))
            {
                await Task.Run(() => context.ExpenseItem.Add(expense));
                await Task.Run(() => context.Entry(expense).State = Microsoft.EntityFrameworkCore.EntityState.Added); 
                if (expense.budgetCategory != null)
                    await Task.Run(() => context.Entry(expense.budgetCategory).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
            else
            {
                throw new Exception("An expense item record already exists with the same Primary Key value");
            }
            return expense;
        }

        public async Task<IncomeItem> AddIncomeItemAsync(IncomeItem income)
        {
            if (!context.IncomeItem.Any(i => i.id == income.id))
            {
                await Task.Run(() => context.IncomeItem.Add(income));
                await Task.Run(() => context.Entry(income).State = Microsoft.EntityFrameworkCore.EntityState.Added);
                if (income.budgetCategory != null)
                    await Task.Run(() => context.Entry(income.budgetCategory).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
            else
            {
                throw new Exception("An income item record already exists with the same Primary Key value");
            }
            return income;
        }

        public async Task<SavingsAccount> AddSavingsAccountAsync(SavingsAccount account)
        {
            if (!context.SavingsAccount.Any(s => s.id == account.id))
            {
                await Task.Run(() => context.SavingsAccount.Add(account));
                await Task.Run(() => context.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Added);
            }
            else
            {
                throw new Exception("An Savings Account already exists with the same Primary Key value");
            }
            return account;
        }

        public async Task<SavingsWithdrawal> AddSavingsWithdrawalAsync(SavingsWithdrawal withdrawal)
        {
            if (!context.SavingsWithdrawal.Any(w => w.id == withdrawal.id))
            {
                await Task.Run(() => context.SavingsWithdrawal.Add(withdrawal));
                await Task.Run(() => context.Entry(withdrawal.savingsAccount).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
            else
            {
                throw new Exception("A withdrawal record already exists with the same Primary Key value");
            }
            return withdrawal;
        }

        public async Task<SavingsDeposit> AddSavingsDepositAsync(SavingsDeposit deposit)
        {
            if (!context.SavingsDeposit.Any(d => d.id == deposit.id))
            {
                await Task.Run(() => context.SavingsDeposit.Add(deposit));
            }
            else
            {
                throw new Exception("A deposit record already exists with the same Primary Key value");
            }
            return deposit;
        }

        public async Task DeleteBudgetCategoryAsync(BudgetCategory category)
        {
            if (context.BudgetCategory.Any(c => c.id == category.id))
            {
                await Task.Run(() => context.BudgetCategory.Attach(category));
                await Task.Run(() => context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);
            }
        }

        public async Task DeleteCheckingAccountAsync(CheckingAccount account)
        {
            if (context.CheckingAccount.Any(c => c.id == account.id))
            {
                await Task.Run(() => context.CheckingAccount.Attach(account));
                if (!context.CheckingDeposit.Any(d => d.checkingAccountId == account.id) && 
                    !context.CheckingWithdrawal.Any(w => w.checkingAccountId == account.id))
                {
                    await Task.Run(() => context.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);
                }
                else
                {
                    throw new Exception("You first need to delete all deposit and withdrawa transactions for this account");
                }
            }
        }

        public async Task DeleteCheckingDepositAsync(CheckingDeposit deposit)
        {
            if (context.CheckingDeposit.Any(d => d.id == deposit.id))
            {
                await Task.Run(() => context.CheckingDeposit.Attach(deposit));
                await Task.Run(() => context.Entry(deposit).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);
                if (deposit.checkingAccount != null)
                    await Task.Run(() => context.Entry(deposit.checkingAccount).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
        }

        public async Task DeleteCheckingWithdrawalAsync(CheckingWithdrawal withdrawal)
        {
            if (context.CheckingWithdrawal.Any(w => w.id == withdrawal.id))
            {
                await Task.Run(() => context.CheckingWithdrawal.Attach(withdrawal));
                await Task.Run(() => context.Entry(withdrawal).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);
                if (withdrawal.checkingAccount != null)
                    await Task.Run(() => context.Entry(withdrawal.checkingAccount).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
        }

        public async Task DeleteExpenseItemAsync(ExpenseItem expense)
        {
            if (context.ExpenseItem.Any(x => x.id == expense.id))
            {
                await Task.Run(() => context.ExpenseItem.Attach(expense));
                await Task.Run(() => context.Entry(expense).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);
                if (expense.budgetCategory != null)
                    await Task.Run(() => context.Entry(expense.budgetCategory).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
        }

        public async Task DeleteIncomeItemAsync(IncomeItem income)
        {
            if (context.IncomeItem.Any(i => i.id == income.id))
            {
                await Task.Run(() => context.IncomeItem.Attach(income));
                await Task.Run(() => context.Entry(income).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);
                if (income.budgetCategory != null)
                    await Task.Run(() => context.Entry(income.budgetCategory).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
        }

        public async Task DeleteSavingsAccountAsync(SavingsAccount account)
        {
            if (context.SavingsAccount.Any(s => s.id == account.id))
            {
                await Task.Run(() => context.SavingsAccount.Attach(account));
                if (!context.SavingsDeposit.Any(d => d.savingsAccountId == account.id) && 
                    !context.SavingsWithdrawal.Any(w => w.savingsAccountId == account.id))
                {
                    await Task.Run(() => context.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);
                }
                else
                {
                    throw new Exception("You first need to delete all deposit and withdrawal transactions for this account");
                }
            }
        }

        public async Task DeleteSavingsDepositAsync(SavingsDeposit deposit)
        {
            if (context.SavingsDeposit.Any(d => d.id == deposit.id))
            {
                await Task.Run(() => context.SavingsDeposit.Attach(deposit));
                await Task.Run(() => context.Entry(deposit).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);
                if (deposit.savingsAccount != null)
                    await Task.Run(() => context.Entry(deposit.savingsAccount).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
        }

        public async Task<ICollection<BudgetCategory>> GetAllCategoriesAsync()
        {
            List<BudgetCategory> categories = new List<BudgetCategory>();

            if (context.BudgetCategory.Any())
            {
                categories = await Task.Run(() => context.BudgetCategory.ToList());
            }

            return categories;
        }

        public async Task<ICollection<CheckingAccount>> GetAllCheckingAccountsAsync()
        {
            List<CheckingAccount> accounts = new List<CheckingAccount>();

            if (context.CheckingAccount.Any())
            {
                accounts = await Task.Run(() => context.CheckingAccount.ToList());
            }

            return accounts;
        }

        public async Task<ICollection<SavingsAccount>> GetAllSavingsAccountsAsync()
        {
            List<SavingsAccount> accounts = new List<SavingsAccount>();

            if (context.SavingsAccount.Any())
            {
                accounts = await Task.Run(() => context.SavingsAccount.ToList());
            }

            return accounts;
        }

        public async Task<BudgetCategory> GetBudgetCategoryAsync(int id)
        {
            return await context.BudgetCategory.FindAsync(id);
        }

        public async Task<CheckingAccount> GetCheckingAccountAsync(int id)
        {
            return await context.CheckingAccount.FindAsync(id);
        }

        public async Task<CheckingDeposit> GetCheckingDepositAsync(int id)
        {
            return await context.CheckingDeposit.FindAsync(id);
        }

        public async Task<CheckingWithdrawal> GetCheckingWithdrawalAsync(int id)
        {
            return await context.CheckingWithdrawal.FindAsync(id);
        }

        public async Task<ICollection<CheckingDeposit>> GetCheckingDepositsByDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            List<CheckingDeposit> deposits = new List<CheckingDeposit>();

            if (context.CheckingDeposit.Any(d => d.transactionDate >= fromDate && d.transactionDate <= toDate))
            {
                var dps = await Task.Run(() =>
                                         from d in this.context.CheckingDeposit
                                         where d.transactionDate >= fromDate && d.transactionDate <= toDate && 
                                         d.checkingAccountId == accountId
                                         select d
                                        );
                if (dps != null && dps.Count() > 0)
                {
                    deposits = dps.ToList();
                }
            }

            return deposits;
        }

        public async Task<ICollection<CheckingWithdrawal>> GetCheckingWithdrawalsByDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            List<CheckingWithdrawal> withdrawals = new List<CheckingWithdrawal>();

            if (context.CheckingWithdrawal.Any(d => d.transactionDate >= fromDate && d.transactionDate <= toDate))
            {
                var wds = await Task.Run(() =>
                                         from d in this.context.CheckingWithdrawal
                                         where d.transactionDate >= fromDate && d.transactionDate <= toDate && 
                                         d.checkingAccountId == accountId
                                         select d
                                        );
                if (wds != null && wds.Count() > 0)
                {
                    withdrawals = wds.ToList();
                }
            }

            return withdrawals;
        }

        public async Task<ExpenseItem> GetExpenseItemAsync(int id)
        {
            var itm = await context.ExpenseItem.FindAsync(id);
            if (itm != null)
            {
                await context.Entry(itm).Reference(c => c.budgetCategory).LoadAsync();
            }
            return itm;
        }
        
        public async Task<ICollection<ExpenseItem>> GetExpenseItemsForBudgetCategoryAsync(int categoryId)
        {
            List<ExpenseItem> expenseItems = new List<ExpenseItem>();

            if (context.ExpenseItem.Any(e => e.budgetCategoryId == categoryId))
            {
                var xps = await Task.Run(() =>
                                         from x in context.ExpenseItem 
                                         where x.budgetCategoryId == categoryId
                                         select x
                                        );
                if (xps != null && xps.Count() > 0)
                {
                    foreach(var itm in xps)
                    {
                        await context.Entry(itm).Reference(c => c.budgetCategory).LoadAsync();
                    }
                    expenseItems = xps.ToList();
                }
            }

            return expenseItems;
        }

        public async Task<IncomeItem> GetIncomeItemAsync(int id)
        {
            var itm = await context.IncomeItem.FindAsync(id);
            if (itm != null)
            {
                await context.Entry(itm).Reference(r => r.budgetCategory).LoadAsync();
            }
            return itm;
        }

        public async Task<ICollection<IncomeItem>> GetIncomeItemsForBudgetCategoryAsync(int categoryId)
        {
            List<IncomeItem> incomeItems = new List<IncomeItem>();

            if (context.IncomeItem.Any(e => e.budgetCategoryId == categoryId))
            {
                var inc = await Task.Run(() =>
                                         from x in context.IncomeItem
                                         where x.budgetCategoryId == categoryId
                                         select x
                                        );
                if (inc != null && inc.Count() > 0)
                {
                    foreach (var itm in inc)
                    {
                        await context.Entry(itm).Reference(c => c.budgetCategory).LoadAsync();
                    }
                    incomeItems = inc.ToList();
                }
            }

            return incomeItems;
        }

        public async Task<ICollection<BudgetCategory>> GetMatchingCategoriesAsync(string searchText)
        {
            List<BudgetCategory> categories = new List<BudgetCategory>();

            if (!string.IsNullOrEmpty(searchText) && context.BudgetCategory.Any(e => e.categoryName.ToLower().Contains(searchText.ToLower().Trim())))
            {
                var cats = await Task.Run(() =>
                                          from c in context.BudgetCategory
                                          where c.categoryName.ToLower().Contains(searchText.ToLower().Trim())
                                          select c
                                        );
                if (cats != null && cats.Count() > 0)
                {
                    categories = cats.ToList();
                }
            }

            return categories;
        }

        public async Task<SavingsAccount> GetSavingsAccountAsync(int id)
        {
            return await context.SavingsAccount.FindAsync(id);
        }

        public async Task<SavingsDeposit> GetSavingsDepositAsync(int id)
        {
            return await context.SavingsDeposit.FindAsync(id);
        }

        public async Task<ICollection<SavingsDeposit>> GetSavingsDepositsByDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            List<SavingsDeposit> deposits = new List<SavingsDeposit>();

            if (context.SavingsDeposit.Any(d => d.transactionDate >= fromDate && d.transactionDate <= toDate))
            {
                var dps = await Task.Run(() =>
                                         from d in this.context.SavingsDeposit
                                         where d.transactionDate >= fromDate && d.transactionDate <= toDate && 
                                         d.savingsAccountId == accountId
                                         select d
                                        );
                if (dps != null && dps.Count() > 0)
                {
                    deposits = dps.ToList();
                }
            }

            return deposits;
        }

        public async Task<SavingsWithdrawal> GetSavingsWithdrawalAsync(int id)
        {
            return await context.SavingsWithdrawal.FindAsync(id);
        }

        public async Task<ICollection<SavingsWithdrawal>> GetSavingsWithdrawalsByDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            List<SavingsWithdrawal> withdrawals = new List<SavingsWithdrawal>();

            if (context.SavingsWithdrawal.Any(d => d.transactionDate >= fromDate && d.transactionDate <= toDate))
            {
                var wds = await Task.Run(() =>
                                         from d in this.context.SavingsWithdrawal
                                         where d.transactionDate >= fromDate && d.transactionDate <= toDate && 
                                         d.savingsAccountId == accountId
                                         select d
                                        );
                if (wds != null && wds.Count() > 0)
                {
                    withdrawals = wds.ToList();
                }
            }

            return withdrawals;
        }

        public async Task UpdateBudgetCategoryAsync(BudgetCategory category)
        {
            if (context.BudgetCategory.Any(c => c.categoryName == category.categoryName) && 
                context.BudgetCategory.FirstOrDefault(c => c.categoryName == category.categoryName).id != category.id)
            {
                // Different id, same name:
                throw new Exception("An existing category already exists with the same name");
            }
            if (context.BudgetCategory.Any(c => c.id == category.id))
            {
                await Task.Run(() => context.BudgetCategory.Attach(category));
                await Task.Run(() => context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
        }

        public async Task UpdateCheckingAccountAsync(CheckingAccount account)
        {
            if (context.CheckingAccount.Any(c => c.id == account.id))
            {
                await Task.Run(() => context.CheckingAccount.Attach(account));
                await Task.Run(() => context.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
            else
            {
                throw new Exception("Unable to locate existing Checking Account record with provided Primary Key value");
            }
        }

        public async Task UpdateCheckingDepositAsync(CheckingDeposit deposit)
        {
            if (context.CheckingDeposit.Any(c => c.id == deposit.id))
            {
                await Task.Run(() => context.CheckingDeposit.Attach(deposit));
                await Task.Run(() => context.Entry(deposit).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
                if (deposit.checkingAccount != null)
                    await Task.Run(() => context.Entry(deposit.checkingAccount).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
            else
            {
                throw new Exception("Unable to locate existing Checking Deposit record with provided Primary Key value");
            }
        }

        public async Task UpdateCheckingWithdrawalAsync(CheckingWithdrawal withdrawal)
        {
            if (context.CheckingWithdrawal.Any(c => c.id == withdrawal.id))
            {
                await Task.Run(() => context.CheckingWithdrawal.Attach(withdrawal));
                await Task.Run(() => context.Entry(withdrawal).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
                if (withdrawal.checkingAccount != null)
                    await Task.Run(() => context.Entry(withdrawal.checkingAccount).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
                
            }
            else
            {
                throw new Exception("Unable to locate existing Checking Withdrawal record with provided Primary Key value");
            }
        }

        public async Task UpdateExpenseItemAsync(ExpenseItem expense)
        {
            if (context.ExpenseItem.Any(c => c.id == expense.id))
            {
                await Task.Run(() => context.ExpenseItem.Attach(expense));
                await Task.Run(() => context.Entry(expense).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
                if (expense.budgetCategory != null)
                    await Task.Run(() => context.Entry(expense.budgetCategory).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
            else
            {
                throw new Exception("Unable to locate existing Expense Item record with provided Primary Key value");
            }
        }

        public async Task UpdateIncomeItemAsync(IncomeItem income)
        {
            if (context.IncomeItem.Any(c => c.id == income.id))
            {
                await Task.Run(() => context.IncomeItem.Attach(income));
                await Task.Run(() => context.Entry(income).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
                if (income.budgetCategory != null)
                    await Task.Run(() => context.Entry(income.budgetCategory).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
            else
            {
                throw new Exception("Unable to locate existing Income Item record with provided Primary Key value");
            }
        }

        public async Task UpdateSavingsAccountAsync(SavingsAccount account)
        {
            if (context.SavingsAccount.Any(c => c.id == account.id))
            {
                await Task.Run(() => context.SavingsAccount.Attach(account));
                await Task.Run(() => context.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
            else
            {
                throw new Exception("Unable to locate existing Savings Account record with provided Primary Key value");
            }
        }

        public async Task<BankAccountFundsTransfer> GetBankAccountFundsTransfer(int transferId)
        {
            BankAccountFundsTransfer transfer = await context.BankAccountFundsTransfer.FindAsync(transferId);

            if (transfer != null)
            {
                switch (transfer.sourceAccountType)
                {
                    case BankAccountType.Checking:
                        transfer.sourceAccount = await context.CheckingAccount.FindAsync(transfer.sourceAccountId);
                        break;
                    case BankAccountType.Savings:
                        transfer.sourceAccount = await context.SavingsAccount.FindAsync(transfer.sourceAccountId);
                        break;
                }
                switch (transfer.destinationAccountType)
                {
                    case BankAccountType.Checking:
                        transfer.destinationAccount = await context.CheckingAccount.FindAsync(transfer.destinationAccountId);
                        break;
                    case BankAccountType.Savings:
                        transfer.destinationAccount = await context.SavingsAccount.FindAsync(transfer.destinationAccountId);
                        break;
                }
            }

            return transfer;
        }

        public async Task UpdateSavingsDepositAsync(SavingsDeposit deposit)
        {
            if (context.SavingsDeposit.Any(c => c.id == deposit.id))
            {
                await Task.Run(() => context.SavingsDeposit.Attach(deposit));
                await Task.Run(() => context.Entry(deposit).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
                if (deposit.savingsAccount != null)
                    await Task.Run(() => context.Entry(deposit.savingsAccount).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
            else
            {
                throw new Exception("Unable to locate existing Savings Deposit record with provided Primary Key value");
            }
        }

        public async Task UpdateSavingsWithdrawalAsync(SavingsWithdrawal withdrawal)
        {
            if (context.SavingsWithdrawal.Any(c => c.id == withdrawal.id))
            {
                await Task.Run(() => context.SavingsWithdrawal.Attach(withdrawal));
                await Task.Run(() => context.Entry(withdrawal).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
                if (withdrawal.savingsAccount != null)
                    await Task.Run(() => context.Entry(withdrawal.savingsAccount).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
            else
            {
                throw new Exception("Unable to locate existing Savings Withdrawal record with provided Primary Key value");
            }
        }

        public async Task AddBankAccountFundsTransferAsync(BankAccountFundsTransfer fundsTransfer)
        {
            if (!context.BankAccountFundsTransfer.Any(t => t.id == fundsTransfer.id))
            {
                await Task.Run(() => context.BankAccountFundsTransfer.Add(fundsTransfer));
            }
            else
            {
                throw new Exception("A Funds Transfer record already exists with the same Primary Key value");
            }
        }

        public async Task UpdateBankAccountFundsTransferAsync(BankAccountFundsTransfer fundsTransfer)
        {
            if (context.BankAccountFundsTransfer.Any(c => c.id == fundsTransfer.id))
            {
                await Task.Run(() => context.BankAccountFundsTransfer.Attach(fundsTransfer));
                await Task.Run(() => context.Entry(fundsTransfer).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
            }
            else
            {
                throw new Exception("Unable to locate existing Funds Transfer record with provided Primary Key value");
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync();
        }

        public void Dispose()
        {
            //try
            //{
            //    connection.Close();
            //}
            //catch (Exception ex)
            //{
            //    string err = ex.Message;
            //}
            this.context?.Dispose();
        }
    }
}
