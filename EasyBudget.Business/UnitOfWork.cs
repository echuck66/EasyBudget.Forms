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
using EasyBudget.Data;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;
using EasyBudget.Business.UoWResults;

namespace EasyBudget.Business
{
    public sealed class UnitOfWork : IDisposable
    {
        private IEasyBudgetRepository repository;

        public UnitOfWork(string dbFilePath)
        {
            //repository = new EasyBudgetRepository(dbFilePath);
            repository = new SQLiteRepository(dbFilePath);
        }

        public UnitOfWork(IEasyBudgetRepository repo)
        {
            repository = repo;
        }

        public void Dispose()
        {
            this.repository?.Dispose();
        }

        #region System Data

        public async Task<SystemDataInitializationResults> EnsureSystemDataItemsAsync()
        {
            SystemDataInitializationResults _results = new SystemDataInitializationResults();
            bool dataChanged = false;

            try
            {

                ICollection<BudgetCategory> _budgetCategories = await repository.GetAllCategoriesAsync();

                int _categoryCount = _budgetCategories.Count;
                int _incomeItemCount = 0;
                int _expenseItemCount = 0;
                int _checkingAccountCount = 0;
                int _savingsAccountCount = 0;

                if (!_budgetCategories.Any(c => c.categoryType == BudgetCategoryType.Income))
                {
                    BudgetCategory _defaultIncomeCategory = new BudgetCategory();
                    _defaultIncomeCategory.CanEdit = true;
                    _defaultIncomeCategory.CanDelete = false;
                    _defaultIncomeCategory.categoryName = "Default Income Category";
                    _defaultIncomeCategory.categoryType = BudgetCategoryType.Income;
                    _defaultIncomeCategory.dateCreated = DateTime.Now;
                    _defaultIncomeCategory.dateModified = DateTime.Now;
                    _defaultIncomeCategory.categoryIcon = AppIcon.None;
                    _defaultIncomeCategory.budgetAmount = 0;
                    await repository.AddBudgetCategoryAsync(_defaultIncomeCategory);
                    _categoryCount++;
                    dataChanged = true;
                    await repository.SaveChangesAsync();

                    IncomeItem _defaultIncomeItem = new IncomeItem();
                    _defaultIncomeItem.CanEdit = true;
                    _defaultIncomeItem.CanDelete = false;
                    _defaultIncomeItem.description = "Default Item";
                    _defaultIncomeItem.notation = string.Empty;
                    _defaultIncomeItem.budgetCategory = _defaultIncomeCategory;
                    _defaultIncomeItem.budgetCategoryId = _defaultIncomeCategory.id;
                    _defaultIncomeItem.BudgetedAmount = 0;
                    _defaultIncomeItem.dateCreated = DateTime.Now;
                    _defaultIncomeItem.dateModified = DateTime.Now;
                    _defaultIncomeItem.frequency = Frequency.Monthly;
                    await repository.AddIncomeItemAsync(_defaultIncomeItem);
                    _incomeItemCount++;
                    dataChanged = true;
                    await repository.SaveChangesAsync();
                }
                else
                {
                    foreach (BudgetCategory _category in _budgetCategories.Where(c => c.categoryType == BudgetCategoryType.Income))
                    {
                        ICollection<IncomeItem> _incomeItems = await repository.GetIncomeItemsForBudgetCategoryAsync(_category.id);
                        if (_incomeItems.Count == 0)
                        {
                            IncomeItem _defaultIncomeItem = new IncomeItem();
                            _defaultIncomeItem.CanEdit = true;
                            _defaultIncomeItem.CanDelete = false;
                            _defaultIncomeItem.description = "Default Item";
                            _defaultIncomeItem.notation = string.Empty;
                            _defaultIncomeItem.budgetCategory = _category;
                            _defaultIncomeItem.budgetCategoryId = _category.id;
                            _defaultIncomeItem.BudgetedAmount = 0;
                            _defaultIncomeItem.dateCreated = DateTime.Now;
                            _defaultIncomeItem.dateModified = DateTime.Now;
                            _defaultIncomeItem.frequency = Frequency.Monthly;
                            await repository.AddIncomeItemAsync(_defaultIncomeItem);
                            _incomeItemCount++;
                            dataChanged = true;
                            await repository.SaveChangesAsync();
                        }
                    }
                }

                if (!_budgetCategories.Any(c => c.categoryType == BudgetCategoryType.Expense))
                {
                    BudgetCategory _defaultExpenseCategory = new BudgetCategory();
                    _defaultExpenseCategory.CanEdit = true;
                    _defaultExpenseCategory.CanDelete = false;
                    _defaultExpenseCategory.categoryName = "Default Expense Category";
                    _defaultExpenseCategory.categoryType = BudgetCategoryType.Expense;
                    _defaultExpenseCategory.dateCreated = DateTime.Now;
                    _defaultExpenseCategory.dateModified = DateTime.Now;
                    _defaultExpenseCategory.categoryIcon = AppIcon.None;
                    _defaultExpenseCategory.budgetAmount = 0;
                    await repository.AddBudgetCategoryAsync(_defaultExpenseCategory);
                    _categoryCount++;
                    dataChanged = true;
                    await repository.SaveChangesAsync();

                    ExpenseItem _defaultExpenseItem = new ExpenseItem();
                    _defaultExpenseItem.CanEdit = true;
                    _defaultExpenseItem.CanDelete = false;
                    _defaultExpenseItem.description = "Default Item";
                    _defaultExpenseItem.notation = string.Empty;
                    _defaultExpenseItem.budgetCategory = _defaultExpenseCategory;
                    _defaultExpenseItem.budgetCategoryId = _defaultExpenseCategory.id;
                    _defaultExpenseItem.BudgetedAmount = 0;
                    _defaultExpenseItem.dateCreated = DateTime.Now;
                    _defaultExpenseItem.dateModified = DateTime.Now;
                    _defaultExpenseItem.frequency = Frequency.Monthly;
                    await repository.AddExpenseItemAsync(_defaultExpenseItem);
                    _expenseItemCount++;
                    dataChanged = true;
                    await repository.SaveChangesAsync();
                }
                else
                {
                    foreach (BudgetCategory _category in _budgetCategories.Where(c => c.categoryType == BudgetCategoryType.Expense))
                    {
                        ICollection<ExpenseItem> _incomeItems = await repository.GetExpenseItemsForBudgetCategoryAsync(_category.id);
                        if (_incomeItems.Count == 0)
                        {
                            ExpenseItem _defaultExpenseItem = new ExpenseItem();
                            _defaultExpenseItem.CanEdit = true;
                            _defaultExpenseItem.CanDelete = false;
                            _defaultExpenseItem.description = "Default Item";
                            _defaultExpenseItem.notation = string.Empty;
                            _defaultExpenseItem.budgetCategory = _category;
                            _defaultExpenseItem.budgetCategoryId = _category.id;
                            _defaultExpenseItem.BudgetedAmount = 0;
                            _defaultExpenseItem.dateCreated = DateTime.Now;
                            _defaultExpenseItem.dateModified = DateTime.Now;
                            _defaultExpenseItem.frequency = Frequency.Monthly;
                            await repository.AddExpenseItemAsync(_defaultExpenseItem);
                            _expenseItemCount++;
                            dataChanged = true;
                            await repository.SaveChangesAsync();
                        }
                    }
                }

                _results.BudgetCategoriesExist = _categoryCount > 0;
                _results.BudgetCategoriesCount = _categoryCount;

                ICollection<CheckingAccount> _checkingAccounts = await repository.GetAllCheckingAccountsAsync();
                _checkingAccountCount = _checkingAccounts.Count;
                if (_checkingAccountCount == 0)
                {
                    CheckingAccount _defaultCheckingAccount = new CheckingAccount();
                    _defaultCheckingAccount.CanEdit = true;
                    _defaultCheckingAccount.CanDelete = false;
                    _defaultCheckingAccount.accountType = BankAccountType.Checking;
                    _defaultCheckingAccount.bankName = "Default Account";
                    _defaultCheckingAccount.accountNickname = string.Empty;
                    _defaultCheckingAccount.routingNumber = string.Empty;
                    _defaultCheckingAccount.accountNumber = string.Empty;
                    _defaultCheckingAccount.dateCreated = DateTime.Now;
                    _defaultCheckingAccount.dateModified = DateTime.Now;
                    _defaultCheckingAccount.currentBalance = 0;
                    _defaultCheckingAccount.deposits = new List<CheckingDeposit>();
                    _defaultCheckingAccount.withdrawals = new List<CheckingWithdrawal>();
                    await repository.AddCheckingAccountAsync(_defaultCheckingAccount);
                    _checkingAccountCount++;
                    dataChanged = true;
                    await repository.SaveChangesAsync();
                }

                ICollection<SavingsAccount> _savingsAccounts = await repository.GetAllSavingsAccountsAsync();
                _savingsAccountCount = _savingsAccounts.Count;
                if (_savingsAccountCount == 0)
                {
                    SavingsAccount _defaultSavingsAccount = new SavingsAccount();
                    _defaultSavingsAccount.CanEdit = true;
                    _defaultSavingsAccount.CanDelete = false;
                    _defaultSavingsAccount.accountType = BankAccountType.Savings;
                    _defaultSavingsAccount.bankName = "Default Account";
                    _defaultSavingsAccount.accountNickname = string.Empty;
                    _defaultSavingsAccount.routingNumber = string.Empty;
                    _defaultSavingsAccount.accountNumber = string.Empty;
                    _defaultSavingsAccount.dateCreated = DateTime.Now;
                    _defaultSavingsAccount.dateModified = DateTime.Now;
                    _defaultSavingsAccount.currentBalance = 0;
                    _defaultSavingsAccount.deposits = new List<SavingsDeposit>();
                    _defaultSavingsAccount.withdrawals = new List<SavingsWithdrawal>();
                    await repository.AddSavingsAccountAsync(_defaultSavingsAccount);
                    _savingsAccountCount++;
                    dataChanged = true;
                    await repository.SaveChangesAsync();
                }

                if (dataChanged)
                {

                }
                _results.BudgetCategoriesCount = _categoryCount;
                _results.BudgetCategoriesExist = _categoryCount > 0;
                _results.IncomeItemsCount = _incomeItemCount;
                _results.IncomeItemsExist = _incomeItemCount > 0;
                _results.ExpenseItemsCount = _expenseItemCount;
                _results.ExpenseItemsExist = _expenseItemCount > 0;
                _results.CheckingAccountsCount = _checkingAccountCount;
                _results.CheckingAccountsExist = _checkingAccountCount > 0;
                _results.SavingsAccountsCount = _savingsAccountCount;
                _results.SavingsAccountsExist = _savingsAccountCount > 0;

                _results.Successful = true;
                _results.Results = true;
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.Results = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        #endregion

        #region Budget Categories and Items

        public async Task<BudgetCategoryResults> GetBudgetCategoryAsync(int id)
        {
            BudgetCategoryResults _results = new BudgetCategoryResults();

            try
            {
                var category = await this.repository.GetBudgetCategoryAsync(id);
                _results.Results = category;
                _results.Successful = category != null;
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<BudgetCategoriesResults> GetMatchingBudgetCategoriesAsync(string searchText)
        {
            BudgetCategoriesResults _results = new BudgetCategoriesResults();

            try
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    throw new Exception("Valid search text must be provided");
                }
                var matches = await this.repository.GetMatchingCategoriesAsync(searchText);
                _results.Results = matches;
                _results.Successful = matches != null;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<BudgetCategoryResults> AddBudgetCategoryAsync(BudgetCategory category)
        {
            BudgetCategoryResults _results = new BudgetCategoryResults();

            try
            {
                if (category == null)
                {
                    throw new NullReferenceException("category cannot be NULL");
                }
                BudgetCategory _category = await repository.AddBudgetCategoryAsync(category);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Results = _category;
                _results.Successful = objectsAdded == 1;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<BudgetCategoryResults> UpdateBudgetCategoryAsync(BudgetCategory category)
        {
            BudgetCategoryResults _results = new BudgetCategoryResults();

            try
            {
                if (category == null)
                {
                    throw new NullReferenceException("category cannot be NULL");
                }
                await repository.UpdateBudgetCategoryAsync(category);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = objectsAdded == 1;
                _results.Results = category;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<DeleteBudgetCategoryResults> DeleteBudgetCategoryAsync(BudgetCategory category)
        {
            DeleteBudgetCategoryResults _results = new DeleteBudgetCategoryResults();

            try
            {
                if (category == null)
                {
                    throw new NullReferenceException("category cannot be NULL");
                }
                var incomeItms = await repository.GetIncomeItemsForBudgetCategoryAsync(category.id);
                var expenseItms = await repository.GetExpenseItemsForBudgetCategoryAsync(category.id);
                if (incomeItms.Count > 0 || expenseItms.Count > 0)
                {
                    throw new Exception("You must first remove all Income and Expense Items from this Category before deleting it.");
                }
                await repository.DeleteBudgetCategoryAsync(category);
                int objectsRemoved = await this.repository.SaveChangesAsync();
                _results.Results = objectsRemoved == 1;
                _results.Successful = _results.Results;
            }
            catch (Exception ex)
            {
                _results.Results = false;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<ExpenseItemResults> GetExpenseItemAsync(int id)
        {
            ExpenseItemResults _results = new ExpenseItemResults();

            try
            {
                ExpenseItem expItm = await repository.GetExpenseItemAsync(id);
                _results.Results = expItm;
                _results.Successful = expItm != null;
                if (expItm == null)
                {
                    _results.Message = "No matching ExpenseItem found";
                }
                _results.WorkException = null;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<IncomeItemResults> GetIncomeItemAsync(int id)
        {
            IncomeItemResults _results = new IncomeItemResults();

            try
            {
                IncomeItem expItm = await repository.GetIncomeItemAsync(id);
                _results.Results = expItm;
                _results.Successful = expItm != null;
                if (expItm == null)
                {
                    _results.Message = "No matching IncomeItem found";
                }
                _results.WorkException = null;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<BudgetCategoriesResults> GetAllBudgetCategoriesAsync()
        {
            BudgetCategoriesResults _results = new BudgetCategoriesResults();

            try
            {
                var categories = await repository.GetAllCategoriesAsync();

                _results.Results = categories;
                _results.Successful = true;

                if (categories.Count == 0)
                {
                    _results.Message = "No Budget Categories found";
                }
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<ExpenseItemsResults> GetCategoryExpenseItemsAsync(BudgetCategory category)
        {
            ExpenseItemsResults _results = new ExpenseItemsResults();

            try
            {
                var items = await repository.GetExpenseItemsForBudgetCategoryAsync(category.id);
                _results.Results = items;
                _results.Successful = items != null;
                if (_results.Results.Count == 0)
                {
                    _results.Message = "No Expense Items found for Budget Category " + category.categoryName;
                }
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<IncomeItemsResults> GetCategoryIncomeItemsAsync(BudgetCategory category)
        {
            IncomeItemsResults _results = new IncomeItemsResults();

            try
            {
                var items = await repository.GetIncomeItemsForBudgetCategoryAsync(category.id);
                _results.Results = items;
                _results.Successful = items != null;
                if (_results.Results.Count == 0)
                {
                    _results.Message = "No Income Items found for Budget Category " + category.categoryName;
                }
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<ExpenseItemResults> AddExpenseItemAsync(ExpenseItem expItem)
        {
            ExpenseItemResults _results = new ExpenseItemResults();

            try
            {
                if (expItem == null)
                {
                    throw new NullReferenceException("Expense Item cannot be NULL");
                }
                expItem.budgetCategory = await repository.GetBudgetCategoryAsync(expItem.budgetCategoryId);
                decimal previousBudgetCategoryAmount = expItem.budgetCategory.budgetAmount;
                _results.PreviousBudgetedAmount = expItem.BudgetedAmount;
                _results.PreviousBudgetCategoryAmount = previousBudgetCategoryAmount;
                decimal newCategoryBudgetAmount = previousBudgetCategoryAmount + expItem.BudgetedAmount;
                expItem.budgetCategory.budgetAmount = newCategoryBudgetAmount;

                ExpenseItem _expItem = await repository.AddExpenseItemAsync(expItem);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = _expItem;
                _results.BudgetCategory = expItem.budgetCategory;
                _results.BudgetCategoryId = expItem.budgetCategoryId;
                _results.NewBudgetCategoryAmount = newCategoryBudgetAmount;
                _results.NewBudgetedAmount = expItem.BudgetedAmount;
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<IncomeItemResults> AddIncomeItemAsync(IncomeItem incItem)
        {
            IncomeItemResults _results = new IncomeItemResults();

            try
            {
                if (incItem == null)
                {
                    throw new NullReferenceException("Income Item cannot be NULL");
                }
                incItem.budgetCategory = await repository.GetBudgetCategoryAsync(incItem.budgetCategoryId);
                decimal previousBudgetCategoryAmount = incItem.budgetCategory.budgetAmount;
                _results.PreviousBudgetedAmount = incItem.BudgetedAmount;
                _results.PreviousBudgetCategoryAmount = previousBudgetCategoryAmount;

                incItem.budgetCategory.budgetAmount += incItem.BudgetedAmount;
                IncomeItem _incItem = await repository.AddIncomeItemAsync(incItem);
                int objectsAdded = await this.repository.SaveChangesAsync();

                _results.Results = _incItem;
                _results.BudgetCategory = incItem.budgetCategory;
                _results.BudgetCategoryId = incItem.budgetCategoryId;
                _results.NewBudgetCategoryAmount = incItem.budgetCategory.budgetAmount;
                _results.NewBudgetedAmount = incItem.BudgetedAmount;

                _results.Successful = true;
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<ExpenseItemResults> UpdateExpenseItemAsync(ExpenseItem expItem)
        {
            ExpenseItemResults _results = new ExpenseItemResults();

            try
            {
                if (expItem == null)
                {
                    throw new NullReferenceException("Expense Item cannot be NULL");
                }
                // Get the current budget category so we can adjust the budgeted amount
                expItem.budgetCategory = await repository.GetBudgetCategoryAsync(expItem.budgetCategoryId);
                // Existing (previous) category budget total
                decimal previousBudgetCategoryAmount = expItem.budgetCategory.budgetAmount;

                // Get the previous budgeted amount for this item, deduct it from the category 
                // budget amount, then add the new budgeted amount for this item to get the
                // new category total budget amount ExpenseItem existingItem = await repository.GetExpenseItemAsync(expItem.id);
                ExpenseItem existingItem = await repository.GetExpenseItemAsync(expItem.id);
                decimal previousItemBudgetedAmount = existingItem.BudgetedAmount;

                _results.PreviousBudgetedAmount = previousItemBudgetedAmount;
                _results.PreviousBudgetCategoryAmount = previousBudgetCategoryAmount;
                _results.NewBudgetCategoryAmount = previousBudgetCategoryAmount - previousItemBudgetedAmount + expItem.BudgetedAmount;
                // Set the new total amount on the budget category
                expItem.budgetCategory.budgetAmount = _results.NewBudgetCategoryAmount;
                // Update the item
                await repository.UpdateExpenseItemAsync(expItem);
                int objectsAdded = await this.repository.SaveChangesAsync();

                // Finish updating the return object
                _results.Successful = true;
                _results.Results = expItem;
                _results.BudgetCategory = expItem.budgetCategory;
                _results.BudgetCategoryId = expItem.budgetCategoryId;
                _results.NewBudgetedAmount = expItem.BudgetedAmount;

            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<IncomeItemResults> UpdateIncomeItemAsync(IncomeItem incItem)
        {
            IncomeItemResults _results = new IncomeItemResults();

            try
            {
                if (incItem == null)
                {
                    throw new NullReferenceException("Income Item cannot be NULL");
                }
                // Get the current budget category so we can adjust the budgeted amount
                incItem.budgetCategory = await repository.GetBudgetCategoryAsync(incItem.budgetCategoryId);
                // Existing (previous) category budget total
                decimal previousBudgetCategoryAmount = incItem.budgetCategory.budgetAmount;

                // Get the previous budgeted amount for this item, deduct it from the category 
                // budget amount, then add the new budgeted amount for this item to get the
                // new category total budget amount ExpenseItem existingItem = await repository.GetExpenseItemAsync(expItem.id);
                IncomeItem existingItem = await repository.GetIncomeItemAsync(incItem.id);
                decimal previousItemBudgetedAmount = existingItem.BudgetedAmount;

                _results.PreviousBudgetedAmount = previousItemBudgetedAmount;
                _results.PreviousBudgetCategoryAmount = previousBudgetCategoryAmount;
                _results.NewBudgetCategoryAmount = previousBudgetCategoryAmount - previousItemBudgetedAmount + incItem.BudgetedAmount;
                // Set the new total amount on the budget category
                incItem.budgetCategory.budgetAmount = _results.NewBudgetCategoryAmount;
                // Update the item
                await repository.UpdateIncomeItemAsync(incItem);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<DeleteExpenseItemResults> DeleteExpenseItemAsync(ExpenseItem expItem)
        {
            DeleteExpenseItemResults _results = new DeleteExpenseItemResults();

            try
            {
                if (expItem == null)
                {
                    throw new NullReferenceException("Expense Item cannot be NULL");
                }
                expItem.budgetCategory = await repository.GetBudgetCategoryAsync(expItem.budgetCategoryId);
                decimal previousBudgetCategoryAmount = expItem.budgetCategory.budgetAmount;
                _results.PreviousBudgetedAmount = expItem.BudgetedAmount;
                _results.PreviousBudgetCategoryAmount = previousBudgetCategoryAmount;
                decimal newCategoryBudgetAmount = previousBudgetCategoryAmount - expItem.BudgetedAmount;
                expItem.budgetCategory.budgetAmount = newCategoryBudgetAmount;


                await repository.DeleteExpenseItemAsync(expItem);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;

                _results.BudgetCategory = expItem.budgetCategory;
                _results.BudgetCategoryId = expItem.budgetCategoryId;
                _results.NewBudgetCategoryAmount = newCategoryBudgetAmount;
                _results.NewBudgetedAmount = expItem.BudgetedAmount;
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<DeleteIncomeItemResults> DeleteIncomeItemAsync(IncomeItem incItem)
        {
            DeleteIncomeItemResults _results = new DeleteIncomeItemResults();

            try
            {
                if (incItem == null)
                {
                    throw new NullReferenceException("Income Item cannot be NULL");
                }
                incItem.budgetCategory = await repository.GetBudgetCategoryAsync(incItem.budgetCategoryId);
                decimal previousBudgetCategoryAmount = incItem.budgetCategory.budgetAmount;
                _results.PreviousBudgetedAmount = incItem.BudgetedAmount;
                _results.PreviousBudgetCategoryAmount = previousBudgetCategoryAmount;
                decimal newCategoryBudgetAmount = previousBudgetCategoryAmount - incItem.BudgetedAmount;
                incItem.budgetCategory.budgetAmount = newCategoryBudgetAmount;


                await repository.DeleteIncomeItemAsync(incItem);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;

                _results.BudgetCategory = incItem.budgetCategory;
                _results.BudgetCategoryId = incItem.budgetCategoryId;
                _results.NewBudgetCategoryAmount = newCategoryBudgetAmount;
                _results.NewBudgetedAmount = incItem.BudgetedAmount;

                //await repository.DeleteIncomeItemAsync(incItem);
                //int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        #endregion

        #region Checking Accounts and Withdrawals / Deposits

        public async Task<CheckingAccountsResults> GetAllCheckingAccountsAsync()
        {
            CheckingAccountsResults _results = new CheckingAccountsResults();

            try
            {
                var accounts = await repository.GetAllCheckingAccountsAsync();

                _results.Results = accounts;
                _results.Successful = true;

                if (accounts.Count == 0)
                {
                    _results.Message = "No Checking Accounts found";
                }
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;

        }

        public async Task<CheckingAccountResults> GetCheckingAccountAsync(int accountId)
        {
            CheckingAccountResults _results = new CheckingAccountResults();

            try
            {
                CheckingAccount account = await repository.GetCheckingAccountAsync(accountId);
                _results.Results = account;
                _results.Successful = account != null;
                if (account == null)
                {
                    _results.Message = "No matching Checking Account found";
                }
                _results.WorkException = null;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<CheckingAccountResults> GetLoadedCheckingAccountAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            CheckingAccountResults _results = new CheckingAccountResults();

            try
            {
                CheckingAccount account = await repository.GetCheckingAccountAsync(accountId);
                _results.Results = account;
                _results.Successful = account != null;
                if (account == null)
                {
                    _results.Message = "No matching Checking Account found";
                }
                else
                {
                    // Load the deposits and withdrawals for the provided date range
                    var _withdrawals = await repository.GetCheckingWithdrawalsByDateRangeAsync(accountId, fromDate, toDate);
                    if (_withdrawals != null)
                    {
                        _results.Results.withdrawals = _withdrawals;
                    }
                    var _deposits = await repository.GetCheckingDepositsByDateRangeAsync(accountId, fromDate, toDate);
                    if (_deposits != null) 
                    {
                        _results.Results.deposits = _deposits;
                    }

                }
                _results.WorkException = null;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<CheckingAccountResults> AddCheckingAccountAsync(CheckingAccount account)
        {
            CheckingAccountResults _results = new CheckingAccountResults();

            try
            {
                if (account == null)
                {
                    throw new NullReferenceException("Checking Account cannot be NULL");
                }
                CheckingAccount _account = await repository.AddCheckingAccountAsync(account);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = _account;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<CheckingDepositResults> AddCheckingDepositAsync(CheckingDeposit deposit)
        {
            CheckingDepositResults _results = new CheckingDepositResults();

            try
            {
                if (deposit == null)
                {
                    throw new NullReferenceException("Deposit cannot be NULL");
                }
                CheckingDeposit _deposit = await repository.AddCheckingDepositAsync(deposit);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = _deposit;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<CheckingWithdrawalResults> AddCheckingWithdrawalAsync(CheckingWithdrawal withdrawal)
        {
            CheckingWithdrawalResults _results = new CheckingWithdrawalResults();

            try
            {
                if (withdrawal == null)
                {
                    throw new NullReferenceException("Withdrawal cannot be NULL");
                }
                CheckingWithdrawal _withdrawal = await repository.AddCheckingWithdrawalAsync(withdrawal);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = _withdrawal;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<SavingsDepositResults> AddSavingsDepositAsync(SavingsDeposit deposit)
        {
            SavingsDepositResults _results = new SavingsDepositResults();

            try
            {
                if (deposit == null)
                {
                    throw new NullReferenceException("Deposit cannot be NULL");
                }
                SavingsDeposit _deposit = await repository.AddSavingsDepositAsync(deposit);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = _deposit;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<SavingsWithdrawalResults> AddSavingsWithdrawalAsync(SavingsWithdrawal withdrawal)
        {
            SavingsWithdrawalResults _results = new SavingsWithdrawalResults();

            try
            {
                if (withdrawal == null)
                {
                    throw new NullReferenceException("Withdrawal cannot be NULL");
                }
                SavingsWithdrawal _withdrawal = await repository.AddSavingsWithdrawalAsync(withdrawal);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = _withdrawal;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        //***********


        public async Task<CheckingDepositResults> UpdateCheckingDepositAsync(CheckingDeposit deposit)
        {
            CheckingDepositResults _results = new CheckingDepositResults();

            try
            {
                if (deposit == null)
                {
                    throw new NullReferenceException("Deposit cannot be NULL");
                }
                await repository.UpdateCheckingDepositAsync(deposit);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = deposit;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<CheckingWithdrawalResults> UpdateCheckingWithdrawalAsync(CheckingWithdrawal withdrawal)
        {
            CheckingWithdrawalResults _results = new CheckingWithdrawalResults();

            try
            {
                if (withdrawal == null)
                {
                    throw new NullReferenceException("Withdrawal cannot be NULL");
                }
                await repository.UpdateCheckingWithdrawalAsync(withdrawal);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = withdrawal;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<SavingsDepositResults> UpdateSavingsDepositAsync(SavingsDeposit deposit)
        {
            SavingsDepositResults _results = new SavingsDepositResults();

            try
            {
                if (deposit == null)
                {
                    throw new NullReferenceException("Deposit cannot be NULL");
                }
                await repository.UpdateSavingsDepositAsync(deposit);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = deposit;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<SavingsWithdrawalResults> UpdateSavingsWithdrawalAsync(SavingsWithdrawal withdrawal)
        {
            SavingsWithdrawalResults _results = new SavingsWithdrawalResults();

            try
            {
                if (withdrawal == null)
                {
                    throw new NullReferenceException("Withdrawal cannot be NULL");
                }
                await repository.UpdateSavingsWithdrawalAsync(withdrawal);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = withdrawal;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }


        //***********

        public async Task<CheckingAccountResults> UpdateCheckingAccountAsync(CheckingAccount account)
        {
            CheckingAccountResults _results = new CheckingAccountResults();

            try
            {
                if (account == null)
                {
                    throw new NullReferenceException("Checking Account cannot be NULL");
                }
                await repository.UpdateCheckingAccountAsync(account);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = objectsAdded == 1;
                _results.Results = account;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<DeleteCheckingAccountResults> DeleteCheckingAccountAsync(CheckingAccount account)
        {
            DeleteCheckingAccountResults _results = new DeleteCheckingAccountResults();

            try
            {
                if (account == null)
                {
                    throw new NullReferenceException("Checking Account cannot be NULL");
                }
                await repository.DeleteCheckingAccountAsync(account);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Results = objectsAdded == 1;
                _results.Successful = _results.Results;
            }
            catch (Exception ex)
            {
                _results.Results = false;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<CheckingAccountWithdrawalResults> SpendMoneyCheckingAsync(CheckingWithdrawal withdrawal)
        {
            CheckingAccountWithdrawalResults _results = new CheckingAccountWithdrawalResults();

            try
            {
                if (withdrawal.checkingAccountId == 0)
                {
                    throw new Exception("The Checking Account ID is required with all transactions.");
                }
                if (withdrawal.checkingAccount == null)
                {
                    withdrawal.checkingAccount = await repository.GetCheckingAccountAsync(withdrawal.checkingAccountId);
                    if (withdrawal.checkingAccount == null)
                    {
                        throw new Exception("Unable to locate the Checking Account associated with this record.");
                    }
                }
                // Get the most current values for the related account
                withdrawal.checkingAccount = await repository.GetCheckingAccountAsync(withdrawal.checkingAccountId);
                withdrawal.checkingAccount.currentBalance = withdrawal.checkingAccount.currentBalance - withdrawal.transactionAmount;
                CheckingWithdrawal _withdrawal = await repository.AddCheckingWithdrawalAsync(withdrawal);
                await repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = _withdrawal;
                _results.EndingAccountBalance = withdrawal.checkingAccount.currentBalance;
                _results.AccountId = withdrawal.checkingAccount.id;
                _results.Account = withdrawal.checkingAccount;
                _results.TransactionAmount = withdrawal.transactionAmount;
            }
            catch (Exception ex)
            {
                _results.WorkException = ex;
                _results.Successful = false;
            }

            return _results;
        }

        public async Task<CheckingAccountDepositResults> DepositMoneyCheckingAsync(CheckingDeposit deposit)
        {
            CheckingAccountDepositResults _results = new CheckingAccountDepositResults();

            try
            {
                if (deposit.checkingAccountId == 0)
                {
                    throw new Exception("The Checking Account ID is required with all transactions.");
                }
                if (deposit.checkingAccount == null)
                {
                    deposit.checkingAccount = await repository.GetCheckingAccountAsync(deposit.checkingAccountId);
                    if (deposit.checkingAccount == null)
                    {
                        throw new Exception("Unable to locate the Checking Account associated with this record.");
                    }
                }
                // Get the most current values for the related account:
                deposit.checkingAccount = await repository.GetCheckingAccountAsync(deposit.checkingAccountId);
                deposit.checkingAccount.currentBalance = deposit.checkingAccount.currentBalance + deposit.transactionAmount;
                CheckingDeposit _deposit = await repository.AddCheckingDepositAsync(deposit);
                await repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = _deposit;
                _results.EndingAccountBalance = deposit.checkingAccount.currentBalance;
                _results.AccountId = deposit.checkingAccount.id;
                _results.Account = deposit.checkingAccount;
                _results.TransactionAmount = deposit.transactionAmount;
            }
            catch (Exception ex)
            {
                _results.WorkException = ex;
                _results.Successful = false;
            }

            return _results;
        }

        #endregion

        #region Savings Accounts and Withdrawals / Deposits

        public async Task<SavingsAccountsResults> GetAllSavingsAccountsAsync()
        {
            SavingsAccountsResults _results = new SavingsAccountsResults();

            try
            {
                var accounts = await repository.GetAllSavingsAccountsAsync();

                _results.Results = accounts;
                _results.Successful = true;

                if (accounts.Count == 0)
                {
                    _results.Message = "No Savings Accounts found";
                }
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;

        }

        public async Task<SavingsAccountResults> GetSavingsAccountAsync(int accountId)
        {
            SavingsAccountResults _results = new SavingsAccountResults();

            try
            {
                SavingsAccount account = await repository.GetSavingsAccountAsync(accountId);
                _results.Results = account;
                _results.Successful = account != null;
                if (account == null)
                {
                    _results.Message = "No matching Savings Account found";
                }
                _results.WorkException = null;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<SavingsAccountResults> GetLoadingSavingsAccountAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            SavingsAccountResults _results = new SavingsAccountResults();

            try
            {
                SavingsAccount account = await repository.GetSavingsAccountAsync(accountId);
                _results.Results = account;
                _results.Successful = account != null;
                if (account == null)
                {
                    _results.Message = "No matching Savings Account found";
                }
                else
                {// Load the deposits and withdrawals for the provided date range
                    var _withdrawals = await repository.GetSavingsWithdrawalsByDateRangeAsync(accountId, fromDate, toDate);
                    if (_withdrawals != null)
                    {
                        _results.Results.withdrawals = _withdrawals;
                    }
                    var _deposits = await repository.GetSavingsDepositsByDateRangeAsync(accountId, fromDate, toDate);
                    if (_deposits != null)
                    {
                        _results.Results.deposits = _deposits;
                    }
                }
                _results.WorkException = null;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<SavingsAccountResults> AddSavingsAccountAsync(SavingsAccount account)
        {
            SavingsAccountResults _results = new SavingsAccountResults();

            try
            {
                if (account == null)
                {
                    throw new NullReferenceException("Savings Account cannot be NULL");
                }
                SavingsAccount _account = await repository.AddSavingsAccountAsync(account);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = objectsAdded == 1;
                _results.Results = _account;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<SavingsAccountResults> UpdateSavingsAccountAsync(SavingsAccount account)
        {
            SavingsAccountResults _results = new SavingsAccountResults();

            try
            {
                if (account == null)
                {
                    throw new NullReferenceException("Savings Account cannot be NULL");
                }
                await repository.UpdateSavingsAccountAsync(account);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = objectsAdded == 1;
                _results.Results = account;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<DeleteSavingsAccountResults> DeleteSavingsAccountAsync(SavingsAccount account)
        {
            DeleteSavingsAccountResults _results = new DeleteSavingsAccountResults();

            try
            {
                if (account == null)
                {
                    throw new NullReferenceException("Savings Account cannot be NULL");
                }
                await repository.DeleteSavingsAccountAsync(account);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Results = objectsAdded == 1;
                _results.Successful = true;
            }
            catch (Exception ex)
            {
                _results.Results = false;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<SavingsAccountWithdrawalResults> SpendMoneySavingsAsync(SavingsWithdrawal withdrawal)
        {
            SavingsAccountWithdrawalResults _results = new SavingsAccountWithdrawalResults();

            try
            {
                if (withdrawal.savingsAccountId == 0)
                {
                    throw new Exception("The Savings Account ID is required with all transactions.");
                }
                if (withdrawal.savingsAccount == null)
                {
                    withdrawal.savingsAccount = await repository.GetSavingsAccountAsync(withdrawal.savingsAccountId);
                    if (withdrawal.savingsAccount == null)
                    {
                        throw new Exception("Unable to locate the Savings Account associated with this record.");
                    }
                }
                withdrawal.savingsAccount.currentBalance = withdrawal.savingsAccount.currentBalance - withdrawal.transactionAmount;
                SavingsWithdrawal _withdrawal = await repository.AddSavingsWithdrawalAsync(withdrawal);
                await repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = _withdrawal;
                _results.EndingAccountBalance = withdrawal.savingsAccount.currentBalance;
                _results.AccountId = withdrawal.savingsAccount.id;
                _results.Account = withdrawal.savingsAccount;
                _results.TransactionAmount = withdrawal.transactionAmount;
            }
            catch (Exception ex)
            {
                _results.WorkException = ex;
                _results.Successful = false;
            }

            return _results;
        }

        public async Task<SavingsAccountDepositResults> DepositMoneySavingsAsync(SavingsDeposit deposit)
        {
            SavingsAccountDepositResults _results = new SavingsAccountDepositResults();

            try
            {
                if (deposit.savingsAccountId == 0)
                {
                    throw new Exception("The Savings Account ID is required with all transactions.");
                }
                if (deposit.savingsAccount == null)
                {
                    deposit.savingsAccount = await repository.GetSavingsAccountAsync(deposit.savingsAccountId);
                    if (deposit.savingsAccount == null)
                    {
                        throw new Exception("Unable to locate the Savings Account associated with this record.");
                    }
                }
                deposit.savingsAccount.currentBalance = deposit.savingsAccount.currentBalance + deposit.transactionAmount;
                SavingsDeposit _deposit = await repository.AddSavingsDepositAsync(deposit);
                await repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = _deposit;
                _results.EndingAccountBalance = deposit.savingsAccount.currentBalance;
                _results.AccountId = deposit.savingsAccount.id;
                _results.Account = deposit.savingsAccount;
                _results.TransactionAmount = deposit.transactionAmount;
            }
            catch (Exception ex)
            {
                _results.WorkException = ex;
                _results.Successful = false;
            }

            return _results;
        }

        #endregion

        #region Money Transfers

        public async Task<FundsTransferResults> TransferMoneyAsync(BankAccountFundsTransfer fundsTransfer)
        {
            FundsTransferResults _results = new FundsTransferResults();

            try
            {
                // Validate the Funds Transfer object:
                if (fundsTransfer.sourceAccountId == 0)
                {
                    throw new Exception("The Source Account must be correctly identified");
                }
                if (fundsTransfer.destinationAccountId == 0)
                {
                    throw new Exception("The Destination Account must be correctly identified");
                }
                if (fundsTransfer.transactionAmount <= 0)
                {
                    throw new Exception("The Transaction Amount must be greater than $0.00");
                }

                // Initiate and persist the data:
                CheckingAccount sourceChecking = null;
                CheckingAccount destinationChecking = null;
                SavingsAccount sourceSavings = null;
                SavingsAccount destinationSavings = null;

                switch (fundsTransfer.sourceAccountType)
                {
                    case BankAccountType.Checking:
                        sourceChecking = await repository.GetCheckingAccountAsync(fundsTransfer.sourceAccountId);
                        if (sourceChecking == null)
                        {
                            throw new Exception("Unable to locate the Source Checking Account");
                        }
                        fundsTransfer.sourceAccountBeginningBalance = sourceChecking.currentBalance;
                        fundsTransfer.sourceAccountEndingBalance = fundsTransfer.sourceAccountBeginningBalance - fundsTransfer.transactionAmount;
                        sourceChecking.currentBalance = fundsTransfer.sourceAccountEndingBalance;
                        fundsTransfer.sourceAccount = sourceChecking;
                        await repository.UpdateCheckingAccountAsync(sourceChecking);
                        break;
                    case BankAccountType.Savings:
                        sourceSavings = await repository.GetSavingsAccountAsync(fundsTransfer.sourceAccountId);
                        if (sourceSavings == null)
                        {
                            throw new Exception("Unable to locate the Source Savings Account");
                        }
                        fundsTransfer.sourceAccountBeginningBalance = sourceSavings.currentBalance;
                        fundsTransfer.sourceAccountEndingBalance = fundsTransfer.sourceAccountBeginningBalance - fundsTransfer.transactionAmount;
                        sourceSavings.currentBalance = fundsTransfer.sourceAccountEndingBalance;
                        fundsTransfer.sourceAccount = sourceSavings;
                        await repository.UpdateSavingsAccountAsync(sourceSavings);
                        break;
                }

                switch (fundsTransfer.destinationAccountType)
                {
                    case BankAccountType.Checking:
                        destinationChecking = await repository.GetCheckingAccountAsync(fundsTransfer.destinationAccountId);
                        if (destinationChecking == null)
                        {
                            throw new Exception("Unable to locate the Destination Checking Account");
                        }
                        fundsTransfer.destinationAccountBeginningBalance = destinationChecking.currentBalance;
                        fundsTransfer.destinationAccountEndingBalance = fundsTransfer.destinationAccountBeginningBalance + fundsTransfer.transactionAmount;
                        destinationChecking.currentBalance = fundsTransfer.destinationAccountEndingBalance;
                        fundsTransfer.destinationAccount = destinationChecking;
                        await repository.UpdateCheckingAccountAsync(destinationChecking);
                        break;
                    case BankAccountType.Savings:
                        destinationSavings = await repository.GetSavingsAccountAsync(fundsTransfer.destinationAccountId);
                        if (destinationSavings == null)
                        {
                            throw new Exception("Unable to locate the Destination Savings Account");
                        }
                        fundsTransfer.destinationAccountBeginningBalance = destinationSavings.currentBalance;
                        fundsTransfer.destinationAccountEndingBalance = fundsTransfer.destinationAccountBeginningBalance + fundsTransfer.transactionAmount;
                        destinationSavings.currentBalance = fundsTransfer.destinationAccountEndingBalance;
                        fundsTransfer.destinationAccount = destinationSavings;
                        await repository.UpdateSavingsAccountAsync(destinationSavings);
                        break;
                }

                await repository.AddBankAccountFundsTransferAsync(fundsTransfer);
                await repository.SaveChangesAsync();
                _results.Successful = true;

                _results.destinationAccount = fundsTransfer.destinationAccount;
                _results.destinationAccountId = fundsTransfer.destinationAccountId;
                _results.destinationAccountType = fundsTransfer.destinationAccountType;
                _results.destinationAccountBeginningBalance = fundsTransfer.destinationAccountBeginningBalance;
                _results.destinationAccountEndingBalance = fundsTransfer.destinationAccountEndingBalance;

                _results.sourceAccount = fundsTransfer.sourceAccount;
                _results.sourceAccountId = fundsTransfer.sourceAccountId;
                _results.sourceAccountType = fundsTransfer.sourceAccountType;
                _results.sourceAccountBeginningBalance = fundsTransfer.sourceAccountBeginningBalance;
                _results.sourceAccountEndingBalance = fundsTransfer.sourceAccountEndingBalance;

            }
            catch (Exception ex)
            {
                _results.WorkException = ex;
                _results.Successful = false;
            }

            return _results;
        }

        public async Task<FundsTransferResults> VoidFundsTransferAsync(BankAccountFundsTransfer fundsTransfer)
        {
            FundsTransferResults _results = new FundsTransferResults();

            try
            {
                // Validate the Funds Transfer object:
                if (fundsTransfer.sourceAccountId == 0)
                {
                    throw new Exception("The Source Account must be correctly identified");
                }
                if (fundsTransfer.destinationAccountId == 0)
                {
                    throw new Exception("The Destination Account must be correctly identified");
                }
                if (fundsTransfer.transactionAmount <= 0)
                {
                    throw new Exception("The Transaction Amount must be greater than $0.00");
                }

                // Initiate and persist the data:
                CheckingAccount sourceChecking = null;
                CheckingAccount destinationChecking = null;
                SavingsAccount sourceSavings = null;
                SavingsAccount destinationSavings = null;

                switch (fundsTransfer.sourceAccountType)
                {
                    case BankAccountType.Checking:
                        sourceChecking = await repository.GetCheckingAccountAsync(fundsTransfer.sourceAccountId);
                        if (sourceChecking == null)
                        {
                            throw new Exception("Unable to locate the Source Checking Account");
                        }
                        fundsTransfer.sourceAccountBeginningBalance = sourceChecking.currentBalance;
                        fundsTransfer.sourceAccountEndingBalance = fundsTransfer.sourceAccountBeginningBalance + fundsTransfer.transactionAmount;
                        sourceChecking.currentBalance = fundsTransfer.sourceAccountEndingBalance;
                        fundsTransfer.sourceAccount = sourceChecking;
                        await repository.UpdateCheckingAccountAsync(sourceChecking);
                        break;
                    case BankAccountType.Savings:
                        sourceSavings = await repository.GetSavingsAccountAsync(fundsTransfer.sourceAccountId);
                        if (sourceSavings == null)
                        {
                            throw new Exception("Unable to locate the Source Savings Account");
                        }
                        fundsTransfer.sourceAccountBeginningBalance = sourceSavings.currentBalance;
                        fundsTransfer.sourceAccountEndingBalance = fundsTransfer.sourceAccountBeginningBalance + fundsTransfer.transactionAmount;
                        sourceSavings.currentBalance = fundsTransfer.sourceAccountEndingBalance;
                        fundsTransfer.sourceAccount = sourceSavings;
                        await repository.UpdateSavingsAccountAsync(sourceSavings);
                        break;
                }

                switch (fundsTransfer.destinationAccountType)
                {
                    case BankAccountType.Checking:
                        destinationChecking = await repository.GetCheckingAccountAsync(fundsTransfer.destinationAccountId);
                        if (destinationChecking == null)
                        {
                            throw new Exception("Unable to locate the Destination Checking Account");
                        }
                        fundsTransfer.destinationAccountBeginningBalance = destinationChecking.currentBalance;
                        fundsTransfer.destinationAccountEndingBalance = fundsTransfer.destinationAccountBeginningBalance - fundsTransfer.transactionAmount;
                        destinationChecking.currentBalance = fundsTransfer.destinationAccountEndingBalance;
                        fundsTransfer.destinationAccount = destinationChecking;
                        await repository.UpdateCheckingAccountAsync(destinationChecking);
                        break;
                    case BankAccountType.Savings:
                        destinationSavings = await repository.GetSavingsAccountAsync(fundsTransfer.destinationAccountId);
                        if (destinationSavings == null)
                        {
                            throw new Exception("Unable to locate the Destination Savings Account");
                        }
                        fundsTransfer.destinationAccountBeginningBalance = destinationSavings.currentBalance;
                        fundsTransfer.destinationAccountEndingBalance = fundsTransfer.destinationAccountBeginningBalance - fundsTransfer.transactionAmount;
                        destinationSavings.currentBalance = fundsTransfer.destinationAccountEndingBalance;
                        fundsTransfer.destinationAccount = destinationSavings;
                        await repository.UpdateSavingsAccountAsync(destinationSavings);
                        break;
                }

                await repository.UpdateBankAccountFundsTransferAsync(fundsTransfer);
                await repository.SaveChangesAsync();
                _results.Successful = true;

                _results.destinationAccount = fundsTransfer.destinationAccount;
                _results.destinationAccountId = fundsTransfer.destinationAccountId;
                _results.destinationAccountType = fundsTransfer.destinationAccountType;
                _results.destinationAccountBeginningBalance = fundsTransfer.destinationAccountBeginningBalance;
                _results.destinationAccountEndingBalance = fundsTransfer.destinationAccountEndingBalance;

                _results.sourceAccount = fundsTransfer.sourceAccount;
                _results.sourceAccountId = fundsTransfer.sourceAccountId;
                _results.sourceAccountType = fundsTransfer.sourceAccountType;
                _results.sourceAccountBeginningBalance = fundsTransfer.sourceAccountBeginningBalance;
                _results.sourceAccountEndingBalance = fundsTransfer.sourceAccountEndingBalance;

            }
            catch (Exception ex)
            {
                _results.WorkException = ex;
                _results.Successful = false;
            }

            return _results;
        }

        #endregion


    }
}
