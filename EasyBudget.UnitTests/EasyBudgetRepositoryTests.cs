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
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SQLite;
using EasyBudget.Data;

namespace EasyBudget.UnitTests
{
    [TestClass]
    public class EasyBudgetRepositoryTests
    {
        SqliteConnection connection;
        EasyBudgetRepository repository;
        EasyBudgetRepository validationRepository;

        public EasyBudgetRepositoryTests()
        {
        }

        [TestInitialize]
        public void InitializeDataContext()
        {
            
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<EasyBudgetContext>()
                    .UseSqlite(connection)
                    .Options;

            EasyBudgetContext context = new EasyBudgetContext(options);

            repository = new EasyBudgetRepository(context);
            validationRepository = new EasyBudgetRepository(context);
        }

        [TestCleanup]
        public void CleanupDataContext()
        {
            connection.Close();
            connection.Dispose();
            repository.Dispose();
            validationRepository.Dispose();
        }

        [TestMethod]
        public async Task AddBudgetCategoryTests()
        {
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0
            };


            await repository.AddBudgetCategoryAsync(category);
            await repository.SaveChangesAsync();

            BudgetCategory testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);

            Assert.AreEqual(category.id, testCategory.id);
            Assert.AreEqual(category.categoryName, testCategory.categoryName);
            Assert.AreEqual(category.budgetAmount, testCategory.budgetAmount);

        }

        [TestMethod]
        public async Task AddCheckingAccountTests()
        {
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };


            CheckingAccount savedAccount = await repository.AddCheckingAccountAsync(account);
            await repository.SaveChangesAsync();

            CheckingAccount testAccount = await validationRepository.GetCheckingAccountAsync(account.id);

            Assert.AreEqual(savedAccount.id, testAccount.id);
            Assert.AreEqual(account.accountNumber, testAccount.accountNumber);
            Assert.AreEqual(account.routingNumber, testAccount.routingNumber);

        }

        [TestMethod]
        public async Task AddCheckingDepositTests()
        {
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            CheckingDeposit deposit = new CheckingDeposit()
            {
                checkingAccount = account,
                checkingAccountId = account.id,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };


            // Write account to db and test:
            CheckingAccount savedAccount = await repository.AddCheckingAccountAsync(account);
            await repository.SaveChangesAsync();


            CheckingAccount testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(savedAccount.id, testAccount.id);
            Assert.AreEqual(testAccount.currentBalance, account.currentBalance);

            // Adjust the balance on the account by the transaction amount on the deposit
            decimal newBalance = account.currentBalance + deposit.transactionAmount;
            deposit.checkingAccount.currentBalance = deposit.checkingAccount.currentBalance + deposit.transactionAmount;

            CheckingDeposit savedDeposit = await repository.AddCheckingDepositAsync(deposit);
            await repository.SaveChangesAsync();


            // test the changes
            testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, newBalance);

            CheckingDeposit testDeposit = await validationRepository.GetCheckingDepositAsync(deposit.id);
            Assert.AreEqual(savedDeposit.id, testDeposit.id);


        }

        [TestMethod]
        public async Task AddCheckingWithdrawalTests()
        {
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            CheckingWithdrawal withdrawal = new CheckingWithdrawal()
            {
                checkingAccount = account,
                checkingAccountId = account.id,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };


            // Write account to db and test:
            CheckingAccount savedAccount = await repository.AddCheckingAccountAsync(account);
            await repository.SaveChangesAsync();

            CheckingAccount testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, account.currentBalance);
            Assert.AreEqual(savedAccount.id, testAccount.id);

            // Adjust the balance on the account by the transaction amount on the withdrawal
            decimal newBalance = account.currentBalance - withdrawal.transactionAmount;
            withdrawal.checkingAccount.currentBalance = withdrawal.checkingAccount.currentBalance - withdrawal.transactionAmount;

            CheckingWithdrawal savedWithdrawal = await repository.AddCheckingWithdrawalAsync(withdrawal);
            await repository.SaveChangesAsync();

            // test the changes
            testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, newBalance);

            CheckingWithdrawal testWithdrawal = await validationRepository.GetCheckingWithdrawalAsync(withdrawal.id);
            Assert.AreEqual(savedWithdrawal.id, testWithdrawal.id);


        }

        [TestMethod]
        public async Task AddExpenseItemTests()
        {
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0,
                categoryType = BudgetCategoryType.Expense
            };

            ExpenseItem expItm = new ExpenseItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                dateCreated = DateTime.Now,
                description = "Dummy Expense Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };


            await repository.AddBudgetCategoryAsync(category);
            await repository.SaveChangesAsync();

            BudgetCategory testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, category.budgetAmount);
            Assert.IsTrue(testCategory.budgetAmount == 0);

            expItm.budgetCategory.budgetAmount = expItm.budgetCategory.budgetAmount + expItm.BudgetedAmount;

            await repository.AddExpenseItemAsync(expItm);

            ExpenseItem testExpItem = await validationRepository.GetExpenseItemAsync(expItm.id);
            Assert.AreEqual(testExpItem.id, expItm.id);
            Assert.AreEqual(testExpItem.BudgetedAmount, expItm.BudgetedAmount);
            Assert.AreEqual(testExpItem.budgetCategoryId, expItm.budgetCategoryId);

            testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, expItm.budgetCategory.budgetAmount);
            Assert.AreEqual(testCategory.budgetAmount, 250);

        }

        [TestMethod]
        public async Task AddIncomeItemTests()
        {
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0,
                categoryType = BudgetCategoryType.Income
            };

            IncomeItem incItem = new IncomeItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                dateCreated = DateTime.Now,
                description = "Dummy Income Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 2450
            };


            await repository.AddBudgetCategoryAsync(category);
            await repository.SaveChangesAsync();

            BudgetCategory testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, category.budgetAmount);
            Assert.IsTrue(testCategory.budgetAmount == 0);

            incItem.budgetCategory.budgetAmount = incItem.budgetCategory.budgetAmount + incItem.BudgetedAmount;

            await repository.AddIncomeItemAsync(incItem);
            await repository.SaveChangesAsync();

            IncomeItem testIncItem = await validationRepository.GetIncomeItemAsync(incItem.id);
            Assert.AreEqual(testIncItem.id, incItem.id);
            Assert.AreEqual(testIncItem.BudgetedAmount, incItem.BudgetedAmount);
            Assert.AreEqual(testIncItem.budgetCategoryId, incItem.budgetCategoryId);

            testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, incItem.budgetCategory.budgetAmount);
            Assert.AreEqual(testCategory.budgetAmount, 2450);


        }

        [TestMethod]
        public async Task AddSavingsAccountTests()
        {
            SavingsAccount account = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };


            await repository.AddSavingsAccountAsync(account);
            await repository.SaveChangesAsync();

            SavingsAccount testAccount = await validationRepository.GetSavingsAccountAsync(account.id);

            Assert.AreEqual(account.id, testAccount.id);
            Assert.AreEqual(account.accountNumber, testAccount.accountNumber);
            Assert.AreEqual(account.routingNumber, testAccount.routingNumber);

        }

        [TestMethod]
        public async Task AddSavingsWithdrawalTests()
        {
            SavingsAccount account = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            SavingsWithdrawal withdrawal = new SavingsWithdrawal()
            {
                savingsAccount = account,
                savingsAccountId = account.id,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };


            // Write account to db and test:
            await repository.AddSavingsAccountAsync(account);
            await repository.SaveChangesAsync();

            SavingsAccount testAccount = await validationRepository.GetSavingsAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, account.currentBalance);

            // Adjust the balance on the account by the transaction amount on the withdrawal
            decimal newBalance = account.currentBalance - withdrawal.transactionAmount;
            withdrawal.savingsAccount.currentBalance = withdrawal.savingsAccount.currentBalance - withdrawal.transactionAmount;

            await repository.AddSavingsWithdrawalAsync(withdrawal);
            await repository.SaveChangesAsync();

            // test the changes
            testAccount = await validationRepository.GetSavingsAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, newBalance);

            SavingsWithdrawal testWithdrawal = await validationRepository.GetSavingsWithdrawalAsync(withdrawal.id);
            Assert.AreEqual(withdrawal.id, testWithdrawal.id);


        }

        [TestMethod]
        public async Task AddSavingsDepositTests()
        {
            SavingsAccount account = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            SavingsDeposit deposit = new SavingsDeposit()
            {
                savingsAccount = account,
                savingsAccountId = account.id,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };


            // Write account to db and test:
            await repository.AddSavingsAccountAsync(account);
            await repository.SaveChangesAsync();

            SavingsAccount testAccount = await validationRepository.GetSavingsAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, account.currentBalance);

            // Adjust the balance on the account by the transaction amount on the deposit
            decimal newBalance = account.currentBalance + deposit.transactionAmount;
            deposit.savingsAccount.currentBalance = deposit.savingsAccount.currentBalance + deposit.transactionAmount;

            await repository.AddSavingsDepositAsync(deposit);
            await repository.SaveChangesAsync();

            // test the changes
            testAccount = await validationRepository.GetSavingsAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, newBalance);

            SavingsDeposit testDeposit = await validationRepository.GetSavingsDepositAsync(deposit.id);
            Assert.AreEqual(deposit.id, testDeposit.id);


        }

        [TestMethod]
        public async Task DeleteBudgetCategoryTests()
        {
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0
            };


            await repository.AddBudgetCategoryAsync(category);
            await repository.SaveChangesAsync();

            BudgetCategory testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);

            Assert.AreEqual(category.id, testCategory.id);
            Assert.AreEqual(category.categoryName, testCategory.categoryName);
            Assert.AreEqual(category.budgetAmount, testCategory.budgetAmount);

            await repository.DeleteBudgetCategoryAsync(category);
            await repository.SaveChangesAsync();

            testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);

            Assert.IsNull(testCategory);

        }

        [TestMethod]
        public async Task DeleteCheckingAccountTests()
        {
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };


            await repository.AddCheckingAccountAsync(account);
            await repository.SaveChangesAsync();

            CheckingAccount testAccount = await validationRepository.GetCheckingAccountAsync(account.id);

            Assert.AreEqual(account.id, testAccount.id);
            Assert.AreEqual(account.accountNumber, testAccount.accountNumber);
            Assert.AreEqual(account.routingNumber, testAccount.routingNumber);

            await repository.DeleteCheckingAccountAsync(account);
            await repository.SaveChangesAsync();

            testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.IsNull(testAccount);

        }

        [TestMethod]
        public async Task DeleteCheckingDepositTests()
        {
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            CheckingDeposit deposit = new CheckingDeposit()
            {
                checkingAccount = account,
                checkingAccountId = account.id,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };


            // Write account to db and test:
            await repository.AddCheckingAccountAsync(account);
            await repository.SaveChangesAsync();

            CheckingAccount testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, account.currentBalance);

            // Adjust the balance on the account by the transaction amount on the deposit
            decimal newBalance = account.currentBalance + deposit.transactionAmount;
            deposit.checkingAccount.currentBalance = deposit.checkingAccount.currentBalance + deposit.transactionAmount;

            await repository.AddCheckingDepositAsync(deposit);
            await repository.SaveChangesAsync();

            // test the changes
            testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, newBalance);

            CheckingDeposit testDeposit = await validationRepository.GetCheckingDepositAsync(deposit.id);
            Assert.AreEqual(deposit.id, testDeposit.id);

            // Now we remove the deposit and ensure the account balance updated accordingly
            decimal beginningBalance = testAccount.currentBalance;
            decimal endingBalance = beginningBalance - deposit.transactionAmount;

            account.currentBalance = beginningBalance;

            deposit.checkingAccount = account;
            deposit.checkingAccount.currentBalance = deposit.checkingAccount.currentBalance - deposit.transactionAmount;

            await repository.DeleteCheckingDepositAsync(deposit);
            await repository.SaveChangesAsync();

            testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            testDeposit = await validationRepository.GetCheckingDepositAsync(deposit.id);

            Assert.IsNull(testDeposit);
            Assert.AreEqual(testAccount.currentBalance, endingBalance);

        }

        [TestMethod]
        public async Task DeleteCheckingWithdrawalTests()
        {
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            CheckingWithdrawal withdrawal = new CheckingWithdrawal()
            {
                checkingAccount = account,
                checkingAccountId = account.id,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };

            // Write account to db and test:
            await repository.AddCheckingAccountAsync(account);
            await repository.SaveChangesAsync();

            CheckingAccount testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, account.currentBalance);

            // Adjust the balance on the account by the transaction amount on the withdrawal
            decimal newBalance = account.currentBalance - withdrawal.transactionAmount;
            withdrawal.checkingAccount.currentBalance = withdrawal.checkingAccount.currentBalance - withdrawal.transactionAmount;

            await repository.AddCheckingWithdrawalAsync(withdrawal);
            await repository.SaveChangesAsync();

            // test the changes
            testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, newBalance);

            CheckingWithdrawal testWithdrawal = await validationRepository.GetCheckingWithdrawalAsync(withdrawal.id);
            Assert.AreEqual(withdrawal.id, testWithdrawal.id);

            // Now we remove the withdrawal and ensure the account balance updated accordingly
            decimal beginningBalance = testAccount.currentBalance;
            decimal endingBalance = beginningBalance - withdrawal.transactionAmount;

            account.currentBalance = beginningBalance;

            withdrawal.checkingAccount = account;
            withdrawal.checkingAccount.currentBalance = withdrawal.checkingAccount.currentBalance - withdrawal.transactionAmount;

            await repository.DeleteCheckingWithdrawalAsync(withdrawal);
            await repository.SaveChangesAsync();

            testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            testWithdrawal = await validationRepository.GetCheckingWithdrawalAsync(withdrawal.id);

            Assert.IsNull(testWithdrawal);
            Assert.AreEqual(testAccount.currentBalance, endingBalance);

        }

        [TestMethod]
        public async Task DeleteExpenseItemTests()
        {
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0,
                categoryType = BudgetCategoryType.Expense
            };

            ExpenseItem expItm = new ExpenseItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                dateCreated = DateTime.Now,
                description = "Dummy Expense Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };


            await repository.AddBudgetCategoryAsync(category);
            await repository.SaveChangesAsync();

            BudgetCategory testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, category.budgetAmount);
            Assert.IsTrue(testCategory.budgetAmount == 0);

            expItm.budgetCategory.budgetAmount = expItm.budgetCategory.budgetAmount + expItm.BudgetedAmount;
            await repository.AddExpenseItemAsync(expItm);
            await repository.SaveChangesAsync();

            ExpenseItem testExpItem = await validationRepository.GetExpenseItemAsync(expItm.id);
            Assert.AreEqual(testExpItem.id, expItm.id);
            Assert.AreEqual(testExpItem.BudgetedAmount, expItm.BudgetedAmount);
            Assert.AreEqual(testExpItem.budgetCategoryId, expItm.budgetCategoryId);

            testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, expItm.budgetCategory.budgetAmount);
            Assert.AreEqual(testCategory.budgetAmount, 250);

            expItm.budgetCategory.budgetAmount = expItm.budgetCategory.budgetAmount - expItm.BudgetedAmount;
            await repository.DeleteExpenseItemAsync(expItm);
            await repository.SaveChangesAsync();

            testExpItem = await validationRepository.GetExpenseItemAsync(expItm.id);
            Assert.IsNull(testExpItem);

            testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.budgetAmount, 0);
        }

        [TestMethod]
        public async Task DeleteIncomeItemTests()
        {
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0,
                categoryType = BudgetCategoryType.Income
            };

            IncomeItem incItem = new IncomeItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                dateCreated = DateTime.Now,
                description = "Dummy Income Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 2450
            };


            await repository.AddBudgetCategoryAsync(category);
            await repository.SaveChangesAsync();

            BudgetCategory testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, category.budgetAmount);
            Assert.IsTrue(testCategory.budgetAmount == 0);

            incItem.budgetCategory.budgetAmount = incItem.budgetCategory.budgetAmount + incItem.BudgetedAmount;

            await repository.AddIncomeItemAsync(incItem);
            await repository.SaveChangesAsync();

            IncomeItem testIncItem = await validationRepository.GetIncomeItemAsync(incItem.id);
            Assert.AreEqual(testIncItem.id, incItem.id);
            Assert.AreEqual(testIncItem.BudgetedAmount, incItem.BudgetedAmount);
            Assert.AreEqual(testIncItem.budgetCategoryId, incItem.budgetCategoryId);

            testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, incItem.budgetCategory.budgetAmount);
            Assert.AreEqual(testCategory.budgetAmount, 2450);

            incItem.budgetCategory.budgetAmount = incItem.budgetCategory.budgetAmount - incItem.BudgetedAmount;
            await repository.DeleteIncomeItemAsync(incItem);
            await repository.SaveChangesAsync();

            testIncItem = await validationRepository.GetIncomeItemAsync(incItem.id);
            Assert.IsNull(testIncItem);

            testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.budgetAmount, 0);
        }

        [TestMethod]
        public async Task DeleteSavingsAccountTests()
        {
            SavingsAccount account = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };


            await repository.AddSavingsAccountAsync(account);
            await repository.SaveChangesAsync();

            SavingsAccount testAccount = await validationRepository.GetSavingsAccountAsync(account.id);

            Assert.AreEqual(account.id, testAccount.id);
            Assert.AreEqual(account.accountNumber, testAccount.accountNumber);
            Assert.AreEqual(account.routingNumber, testAccount.routingNumber);

            await repository.DeleteSavingsAccountAsync(account);
            await repository.SaveChangesAsync();

            testAccount = await validationRepository.GetSavingsAccountAsync(account.id);
            Assert.IsNull(testAccount);
        }

        [TestMethod]
        public async Task DeleteSavingsDepositTests()
        {
            SavingsAccount account = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            SavingsDeposit deposit = new SavingsDeposit()
            {
                savingsAccount = account,
                savingsAccountId = account.id,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };


            // Write account to db and test:
            await repository.AddSavingsAccountAsync(account);
            await repository.SaveChangesAsync();

            SavingsAccount testAccount = await validationRepository.GetSavingsAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, account.currentBalance);

            // Adjust the balance on the account by the transaction amount on the deposit
            decimal newBalance = account.currentBalance + deposit.transactionAmount;
            deposit.savingsAccount.currentBalance = deposit.savingsAccount.currentBalance + deposit.transactionAmount;

            await repository.AddSavingsDepositAsync(deposit);
            await repository.SaveChangesAsync();

            // test the changes
            testAccount = await validationRepository.GetSavingsAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, newBalance);

            SavingsDeposit testDeposit = await validationRepository.GetSavingsDepositAsync(deposit.id);
            Assert.AreEqual(deposit.id, testDeposit.id);

            // Now we remove the deposit and ensure the account balance updated accordingly
            decimal beginningBalance = testAccount.currentBalance;
            decimal endingBalance = beginningBalance - deposit.transactionAmount;

            account.currentBalance = beginningBalance;

            deposit.savingsAccount = account;
            deposit.savingsAccount.currentBalance = deposit.savingsAccount.currentBalance - deposit.transactionAmount;

            await repository.DeleteSavingsDepositAsync(deposit);
            await repository.SaveChangesAsync();

            testAccount = await validationRepository.GetSavingsAccountAsync(account.id);
            testDeposit = await validationRepository.GetSavingsDepositAsync(deposit.id);

            Assert.IsNull(testDeposit);
            Assert.AreEqual(testAccount.currentBalance, endingBalance);



        }

        [TestMethod]
        public async Task GetAllCategoriesTests()
        {
            BudgetCategory category1 = new BudgetCategory()
            {
                categoryName = "Test Category 1",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0
            };
            BudgetCategory category2 = new BudgetCategory()
            {
                categoryName = "Test Category 2",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0
            };
            BudgetCategory category3 = new BudgetCategory()
            {
                categoryName = "Test Category 3",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0
            };

            List<BudgetCategory> categories = new List<BudgetCategory>();
            categories.Add(category1);
            categories.Add(category2);
            categories.Add(category3);


            foreach (BudgetCategory category in categories)
            {
                await repository.AddBudgetCategoryAsync(category);
            }
            await repository.SaveChangesAsync();

            List<BudgetCategory> testCategories = new List<BudgetCategory>();
            testCategories.AddRange(await validationRepository.GetAllCategoriesAsync());

            Assert.IsTrue(testCategories.Count == categories.Count);

        }

        [TestMethod]
        public async Task GetAllCheckingAccountsTests()
        {
            CheckingAccount account1 = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };
            CheckingAccount account2 = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "333333",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };
            CheckingAccount account3 = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "444444",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            List<CheckingAccount> accounts = new List<CheckingAccount>();
            accounts.Add(account1);
            accounts.Add(account2);
            accounts.Add(account3);


            foreach (CheckingAccount account in accounts)
            {
                await repository.AddCheckingAccountAsync(account);
            }
            await repository.SaveChangesAsync();

            List<CheckingAccount> testAccounts = new List<CheckingAccount>();
            testAccounts.AddRange(await validationRepository.GetAllCheckingAccountsAsync());

            Assert.IsTrue(testAccounts.Count == accounts.Count);

        }

        [TestMethod]
        public async Task GetAllSavingsAccountsTests()
        {
            SavingsAccount account1 = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };
            SavingsAccount account2 = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "333333",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };
            SavingsAccount account3 = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "444444",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            List<SavingsAccount> accounts = new List<SavingsAccount>();
            accounts.Add(account1);
            accounts.Add(account2);
            accounts.Add(account3);


            foreach (SavingsAccount account in accounts)
            {
                await repository.AddSavingsAccountAsync(account);
            }
            await repository.SaveChangesAsync();

            List<SavingsAccount> testAccounts = new List<SavingsAccount>();
            testAccounts.AddRange(await validationRepository.GetAllSavingsAccountsAsync());

            Assert.IsTrue(testAccounts.Count == accounts.Count);
        }

        [TestMethod]
        public async Task GetCategoryTests()
        {
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0
            };


            await repository.AddBudgetCategoryAsync(category);
            await repository.SaveChangesAsync();

            BudgetCategory testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);

            Assert.AreEqual(category.id, testCategory.id);
            Assert.AreEqual(category.categoryName, testCategory.categoryName);
            Assert.AreEqual(category.budgetAmount, testCategory.budgetAmount);
        }

        [TestMethod]
        public async Task GetCheckingAccountTests()
        {
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };


            await repository.AddCheckingAccountAsync(account);
            await repository.SaveChangesAsync();

            CheckingAccount testAccount = await validationRepository.GetCheckingAccountAsync(account.id);

            Assert.AreEqual(account.id, testAccount.id);
            Assert.AreEqual(account.accountNumber, testAccount.accountNumber);
            Assert.AreEqual(account.routingNumber, testAccount.routingNumber);
        }

        [TestMethod]
        public async Task GetCheckingDepositTests()
        {
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            CheckingDeposit deposit = new CheckingDeposit()
            {
                checkingAccount = account,
                checkingAccountId = account.id,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };

            // Write account to db and test:
            await repository.AddCheckingAccountAsync(account);
            await repository.SaveChangesAsync();

            CheckingAccount testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, account.currentBalance);

            // Adjust the balance on the account by the transaction amount on the deposit
            decimal newBalance = account.currentBalance + deposit.transactionAmount;
            deposit.checkingAccount.currentBalance = deposit.checkingAccount.currentBalance + deposit.transactionAmount;

            await repository.AddCheckingDepositAsync(deposit);
            await repository.SaveChangesAsync();

            // test the changes
            testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, newBalance);

            CheckingDeposit testDeposit = await validationRepository.GetCheckingDepositAsync(deposit.id);
            Assert.AreEqual(deposit.id, testDeposit.id);
            Assert.AreEqual(deposit.transactionAmount, testDeposit.transactionAmount);
            Assert.AreEqual(deposit.transactionDate, testDeposit.transactionDate);

        }

        [TestMethod]
        public async Task GetCheckingDepositsByDateRangeTests()
        {
            CheckingAccount account1 = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            CheckingAccount account2 = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            CheckingDeposit deposit1 = new CheckingDeposit()
            {
                checkingAccount = account1,
                checkingAccountId = account1.id,
                transactionDate = DateTime.Parse("1/19/2018"),
                transactionAmount = 102.48M
            };

            CheckingDeposit deposit2 = new CheckingDeposit()
            {
                checkingAccount = account2,
                checkingAccountId = account2.id,
                transactionDate = DateTime.Parse("12/5/2017"),
                transactionAmount = 105.87M
            };

            CheckingDeposit deposit3 = new CheckingDeposit()
            {
                checkingAccount = account2,
                checkingAccountId = account2.id,
                transactionDate = DateTime.Parse("1/5/2018"),
                transactionAmount = 85.43M
            };

            CheckingDeposit deposit4 = new CheckingDeposit()
            {
                checkingAccount = account2,
                checkingAccountId = account2.id,
                transactionDate = DateTime.Parse("11/5/2017"),
                transactionAmount = 123.87M
            };

            List<CheckingAccount> accounts = new List<CheckingAccount>();
            accounts.Add(account1);
            accounts.Add(account2);

            foreach(CheckingAccount account in accounts)
            {
                await repository.AddCheckingAccountAsync(account);
            }
            await repository.SaveChangesAsync();

            List<CheckingDeposit> deposits = new List<CheckingDeposit>();
            deposits.Add(deposit1);
            deposits.Add(deposit2);
            deposits.Add(deposit3);
            deposits.Add(deposit4);

            decimal account1RunningBalance = account1.currentBalance;
            decimal account2RunningBalance = account2.currentBalance;

            foreach (CheckingDeposit deposit in deposits)
            {
                // Adjust the balance on the account by the transaction amount on the deposit
                if (deposit.checkingAccountId == account1.id)
                {
                    account1RunningBalance += deposit.transactionAmount;
                    deposit.checkingAccount.currentBalance = account1RunningBalance;
                }
                else
                {
                    account2RunningBalance += deposit.transactionAmount;
                    deposit.checkingAccount.currentBalance = account2RunningBalance;
                }

                await repository.AddCheckingDepositAsync(deposit);
            }

            await repository.SaveChangesAsync();

            DateTime startDate = DateTime.Parse("1/1/2018");
            DateTime endDate = DateTime.Parse("1/31/2018");

            List<CheckingDeposit> foundDeposits1 = new List<CheckingDeposit>();
            foundDeposits1.AddRange(await validationRepository.GetCheckingDepositsByDateRangeAsync(account1.id, startDate, endDate));

            foreach (CheckingDeposit deposit in foundDeposits1)
            {
                Assert.AreEqual(deposit.checkingAccountId, account1.id);
            }

            Assert.IsTrue(foundDeposits1.Count == 1);

            startDate = DateTime.Parse("11/1/2017");
            endDate = DateTime.Parse("12/31/2017");
            List<CheckingDeposit> foundDeposits2 = new List<CheckingDeposit>();
            foundDeposits2.AddRange(await validationRepository.GetCheckingDepositsByDateRangeAsync(account2.id, startDate, endDate));

            foreach (CheckingDeposit deposit in foundDeposits1)
            {
                Assert.AreEqual(deposit.checkingAccountId, account1.id);
            }

            Assert.IsTrue(foundDeposits2.Count == 2);

        }

        [TestMethod]
        public async Task GetCheckingWithdrawalsByDateRangeTests()
        {
            CheckingAccount account1 = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            CheckingAccount account2 = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            CheckingWithdrawal withdrawal1 = new CheckingWithdrawal()
            {
                checkingAccount = account1,
                checkingAccountId = account1.id,
                transactionDate = DateTime.Parse("1/19/2018"),
                transactionAmount = 102.48M
            };

            CheckingWithdrawal withdrawal2 = new CheckingWithdrawal()
            {
                checkingAccount = account2,
                checkingAccountId = account2.id,
                transactionDate = DateTime.Parse("12/5/2017"),
                transactionAmount = 105.87M
            };

            CheckingWithdrawal withdrawal3 = new CheckingWithdrawal()
            {
                checkingAccount = account2,
                checkingAccountId = account2.id,
                transactionDate = DateTime.Parse("1/5/2018"),
                transactionAmount = 85.43M
            };

            CheckingWithdrawal withdrawal4 = new CheckingWithdrawal()
            {
                checkingAccount = account2,
                checkingAccountId = account2.id,
                transactionDate = DateTime.Parse("11/5/2017"),
                transactionAmount = 123.87M
            };

            List<CheckingAccount> accounts = new List<CheckingAccount>();
            accounts.Add(account1);
            accounts.Add(account2);

            foreach (CheckingAccount account in accounts)
            {
                await repository.AddCheckingAccountAsync(account);
            }
            await repository.SaveChangesAsync();

            List<CheckingWithdrawal> withdrawals = new List<CheckingWithdrawal>();
            withdrawals.Add(withdrawal1);
            withdrawals.Add(withdrawal2);
            withdrawals.Add(withdrawal3);
            withdrawals.Add(withdrawal4);

            decimal account1RunningBalance = account1.currentBalance;
            decimal account2RunningBalance = account2.currentBalance;

            foreach (CheckingWithdrawal deposit in withdrawals)
            {
                // Adjust the balance on the account by the transaction amount on the deposit
                if (deposit.checkingAccountId == account1.id)
                {
                    account1RunningBalance -= deposit.transactionAmount;
                    deposit.checkingAccount.currentBalance = account1RunningBalance;
                }
                else
                {
                    account2RunningBalance -= deposit.transactionAmount;
                    deposit.checkingAccount.currentBalance = account2RunningBalance;
                }

                await repository.AddCheckingWithdrawalAsync(deposit);
            }


            await repository.SaveChangesAsync();

            DateTime startDate = DateTime.Parse("1/1/2018");
            DateTime endDate = DateTime.Parse("1/31/2018");

            List<CheckingWithdrawal> foundWithdrawals1 = new List<CheckingWithdrawal>();
            foundWithdrawals1.AddRange(await repository.GetCheckingWithdrawalsByDateRangeAsync(account1.id, startDate, endDate));

            foreach (CheckingWithdrawal deposit in foundWithdrawals1)
            {
                Assert.AreEqual(deposit.checkingAccountId, account1.id);
            }

            Assert.IsTrue(foundWithdrawals1.Count == 1);

            startDate = DateTime.Parse("11/1/2017");
            endDate = DateTime.Parse("12/31/2017");
            List<CheckingWithdrawal> foundDeposits2 = new List<CheckingWithdrawal>();
            foundDeposits2.AddRange(await repository.GetCheckingWithdrawalsByDateRangeAsync(account2.id, startDate, endDate));

            foreach (CheckingWithdrawal deposit in foundWithdrawals1)
            {
                Assert.AreEqual(deposit.checkingAccountId, account1.id);
            }

            Assert.IsTrue(foundDeposits2.Count == 2);

        }

        [TestMethod]
        public async Task GetExpenseItemTests()
        {
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0,
                categoryType = BudgetCategoryType.Expense
            };

            ExpenseItem expItm = new ExpenseItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                dateCreated = DateTime.Now,
                description = "Dummy Expense Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };


            await repository.AddBudgetCategoryAsync(category);
            await repository.SaveChangesAsync();

            BudgetCategory testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, category.budgetAmount);
            Assert.IsTrue(testCategory.budgetAmount == 0);

            expItm.budgetCategory.budgetAmount = expItm.budgetCategory.budgetAmount + expItm.BudgetedAmount;

            await repository.AddExpenseItemAsync(expItm);

            ExpenseItem testExpItem = await validationRepository.GetExpenseItemAsync(expItm.id);
            Assert.AreEqual(testExpItem.id, expItm.id);
            Assert.AreEqual(testExpItem.BudgetedAmount, expItm.BudgetedAmount);
            Assert.AreEqual(testExpItem.budgetCategoryId, expItm.budgetCategoryId);

            testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, expItm.budgetCategory.budgetAmount);
            Assert.AreEqual(testCategory.budgetAmount, 250);
        }

        [TestMethod]
        public async Task GetExpenseItemsForBudgetCategoryTests()
        {
            BudgetCategory category1 = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0,
                categoryType = BudgetCategoryType.Income
            };

            ExpenseItem expItm1 = new ExpenseItem()
            {
                budgetCategory = category1,
                budgetCategoryId = category1.id,
                dateCreated = DateTime.Now,
                description = "Dummy Expense Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };

            ExpenseItem expItm2 = new ExpenseItem()
            {
                budgetCategory = category1,
                budgetCategoryId = category1.id,
                dateCreated = DateTime.Now,
                description = "Dummy Expense Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };

            ExpenseItem expItm3 = new ExpenseItem()
            {
                budgetCategory = category1,
                budgetCategoryId = category1.id,
                dateCreated = DateTime.Now,
                description = "Dummy Expense Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };

            BudgetCategory category2 = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0,
                categoryType = BudgetCategoryType.Income
            };

            ExpenseItem expItm4 = new ExpenseItem()
            {
                budgetCategory = category2,
                budgetCategoryId = category2.id,
                dateCreated = DateTime.Now,
                description = "Dummy Expense Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };

            ExpenseItem expItm5 = new ExpenseItem()
            {
                budgetCategory = category2,
                budgetCategoryId = category2.id,
                dateCreated = DateTime.Now,
                description = "Dummy Expense Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };

            ExpenseItem expItm6 = new ExpenseItem()
            {
                budgetCategory = category2,
                budgetCategoryId = category2.id,
                dateCreated = DateTime.Now,
                description = "Dummy Expense Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };

            List<BudgetCategory> categories = new List<BudgetCategory>();
            categories.Add(category1);
            categories.Add(category2);

            List<ExpenseItem> expenseItems = new List<ExpenseItem>();
            expenseItems.Add(expItm1);
            expenseItems.Add(expItm2);
            expenseItems.Add(expItm3);
            expenseItems.Add(expItm4);
            expenseItems.Add(expItm5);
            expenseItems.Add(expItm6);


            foreach (BudgetCategory category in categories)
            {
                await repository.AddBudgetCategoryAsync(category);
            }
            await repository.SaveChangesAsync();

            foreach (ExpenseItem item in expenseItems)
            {
                await repository.AddExpenseItemAsync(item);
            }
            await repository.SaveChangesAsync();

            List<ExpenseItem> testItems = new List<ExpenseItem>();
            testItems.AddRange(await validationRepository.GetExpenseItemsForBudgetCategoryAsync(category1.id));

            Assert.IsTrue(testItems.Count == 3);
            foreach (ExpenseItem item in testItems)
            {
                Assert.AreEqual(item.budgetCategoryId, category1.id);
            }

        }

        [TestMethod]
        public async Task GetIncomeItemTests()
        {
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0,
                categoryType = BudgetCategoryType.Income
            };

            IncomeItem incItem = new IncomeItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                dateCreated = DateTime.Now,
                description = "Dummy Income Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 2450
            };


            await repository.AddBudgetCategoryAsync(category);
            await repository.SaveChangesAsync();

            BudgetCategory testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, category.budgetAmount);
            Assert.IsTrue(testCategory.budgetAmount == 0);

            incItem.budgetCategory.budgetAmount = incItem.budgetCategory.budgetAmount + incItem.BudgetedAmount;

            await repository.AddIncomeItemAsync(incItem);

            IncomeItem testIncItem = await validationRepository.GetIncomeItemAsync(incItem.id);
            Assert.AreEqual(testIncItem.id, incItem.id);
            Assert.AreEqual(testIncItem.BudgetedAmount, incItem.BudgetedAmount);
            Assert.AreEqual(testIncItem.budgetCategoryId, incItem.budgetCategoryId);

            testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, incItem.budgetCategory.budgetAmount);
            Assert.AreEqual(testCategory.budgetAmount, 2450);
        }

        [TestMethod]
        public async Task GetIncomeItemsForBudgetCategoryTests()
        {
            BudgetCategory category1 = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0,
                categoryType = BudgetCategoryType.Income
            };

            IncomeItem incomeItm1 = new IncomeItem()
            {
                budgetCategory = category1,
                budgetCategoryId = category1.id,
                dateCreated = DateTime.Now,
                description = "Dummy Income Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };

            IncomeItem incomeItm2 = new IncomeItem()
            {
                budgetCategory = category1,
                budgetCategoryId = category1.id,
                dateCreated = DateTime.Now,
                description = "Dummy Income Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };

            IncomeItem incomeItm3 = new IncomeItem()
            {
                budgetCategory = category1,
                budgetCategoryId = category1.id,
                dateCreated = DateTime.Now,
                description = "Dummy Income Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };

            BudgetCategory category2 = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0,
                categoryType = BudgetCategoryType.Income
            };

            IncomeItem incomeItm4 = new IncomeItem()
            {
                budgetCategory = category2,
                budgetCategoryId = category2.id,
                dateCreated = DateTime.Now,
                description = "Dummy Income Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };

            IncomeItem incomeItm5 = new IncomeItem()
            {
                budgetCategory = category2,
                budgetCategoryId = category2.id,
                dateCreated = DateTime.Now,
                description = "Dummy Income Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };

            IncomeItem incomeItm6 = new IncomeItem()
            {
                budgetCategory = category2,
                budgetCategoryId = category2.id,
                dateCreated = DateTime.Now,
                description = "Dummy Income Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };

            List<BudgetCategory> categories = new List<BudgetCategory>();
            categories.Add(category1);
            categories.Add(category2);

            List<IncomeItem> incomeItems = new List<IncomeItem>();
            incomeItems.Add(incomeItm1);
            incomeItems.Add(incomeItm2);
            incomeItems.Add(incomeItm3);
            incomeItems.Add(incomeItm4);
            incomeItems.Add(incomeItm5);
            incomeItems.Add(incomeItm6);


            foreach (BudgetCategory category in categories)
            {
                await repository.AddBudgetCategoryAsync(category);
            }
            await repository.SaveChangesAsync();

            foreach (IncomeItem item in incomeItems)
            {
                await repository.AddIncomeItemAsync(item);
            }
            await repository.SaveChangesAsync();

            List<IncomeItem> testItems = new List<IncomeItem>();
            testItems.AddRange(await validationRepository.GetIncomeItemsForBudgetCategoryAsync(category1.id));

            Assert.IsTrue(testItems.Count == 3);
            foreach (IncomeItem item in testItems)
            {
                Assert.AreEqual(item.budgetCategoryId, category1.id);
            }

        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        public void GetMatchingCategoriesTests()
        {
            
        }

        [TestMethod]
        public async Task GetSavingsAccountTests()
        {
            SavingsAccount account = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };


            await repository.AddSavingsAccountAsync(account);
            await repository.SaveChangesAsync();

            SavingsAccount testAccount = await validationRepository.GetSavingsAccountAsync(account.id);

            Assert.AreEqual(account.id, testAccount.id);
            Assert.AreEqual(account.accountNumber, testAccount.accountNumber);
            Assert.AreEqual(account.routingNumber, testAccount.routingNumber);
        }

        [TestMethod]
        public async Task GetSavingsDepositTests()
        {
            SavingsAccount account = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            SavingsDeposit deposit = new SavingsDeposit()
            {
                savingsAccount = account,
                savingsAccountId = account.id,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };


            // Write account to db and test:
            await repository.AddSavingsAccountAsync(account);
            await repository.SaveChangesAsync();

            SavingsAccount testAccount = await validationRepository.GetSavingsAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, account.currentBalance);

            // Adjust the balance on the account by the transaction amount on the deposit
            decimal newBalance = account.currentBalance + deposit.transactionAmount;
            deposit.savingsAccount.currentBalance = deposit.savingsAccount.currentBalance + deposit.transactionAmount;

            await repository.AddSavingsDepositAsync(deposit);
            await repository.SaveChangesAsync();

            // test the changes
            testAccount = await validationRepository.GetSavingsAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, newBalance);

            SavingsDeposit testDeposit = await validationRepository.GetSavingsDepositAsync(deposit.id);
            Assert.AreEqual(deposit.id, testDeposit.id);

        }

        [TestMethod]
        public async Task GetSavingsDepositsByDateRangeTests()
        {
            SavingsAccount account1 = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            SavingsAccount account2 = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            SavingsDeposit deposit1 = new SavingsDeposit()
            {
                savingsAccount = account1,
                savingsAccountId = account1.id,
                transactionDate = DateTime.Parse("1/19/2018"),
                transactionAmount = 102.48M
            };

            SavingsDeposit deposit2 = new SavingsDeposit()
            {
                savingsAccount = account2,
                savingsAccountId = account2.id,
                transactionDate = DateTime.Parse("12/5/2017"),
                transactionAmount = 105.87M
            };

            SavingsDeposit deposit3 = new SavingsDeposit()
            {
                savingsAccount = account2,
                savingsAccountId = account2.id,
                transactionDate = DateTime.Parse("1/5/2018"),
                transactionAmount = 85.43M
            };

            SavingsDeposit deposit4 = new SavingsDeposit()
            {
                savingsAccount = account2,
                savingsAccountId = account2.id,
                transactionDate = DateTime.Parse("11/5/2017"),
                transactionAmount = 123.87M
            };

            List<SavingsAccount> accounts = new List<SavingsAccount>();
            accounts.Add(account1);
            accounts.Add(account2);

            foreach (SavingsAccount account in accounts)
            {
                await repository.AddSavingsAccountAsync(account);
            }
            await repository.SaveChangesAsync();

            List<SavingsDeposit> deposits = new List<SavingsDeposit>();
            deposits.Add(deposit1);
            deposits.Add(deposit2);
            deposits.Add(deposit3);
            deposits.Add(deposit4);

            decimal account1RunningBalance = account1.currentBalance;
            decimal account2RunningBalance = account2.currentBalance;

            foreach (SavingsDeposit deposit in deposits)
            {
                // Adjust the balance on the account by the transaction amount on the deposit
                if (deposit.savingsAccountId == account1.id)
                {
                    account1RunningBalance += deposit.transactionAmount;
                    deposit.savingsAccount.currentBalance = account1RunningBalance;
                }
                else
                {
                    account2RunningBalance += deposit.transactionAmount;
                    deposit.savingsAccount.currentBalance = account2RunningBalance;
                }

                await repository.AddSavingsDepositAsync(deposit);
            }

            await repository.SaveChangesAsync();

            DateTime startDate = DateTime.Parse("1/1/2018");
            DateTime endDate = DateTime.Parse("1/31/2018");

            List<SavingsDeposit> foundDeposits1 = new List<SavingsDeposit>();
            foundDeposits1.AddRange(await validationRepository.GetSavingsDepositsByDateRangeAsync(account1.id, startDate, endDate));

            foreach (SavingsDeposit deposit in foundDeposits1)
            {
                Assert.AreEqual(deposit.savingsAccountId, account1.id);
            }

            Assert.IsTrue(foundDeposits1.Count == 1);

            startDate = DateTime.Parse("11/1/2017");
            endDate = DateTime.Parse("12/31/2017");
            List<SavingsDeposit> foundDeposits2 = new List<SavingsDeposit>();
            foundDeposits2.AddRange(await validationRepository.GetSavingsDepositsByDateRangeAsync(account2.id, startDate, endDate));

            foreach (SavingsDeposit deposit in foundDeposits2)
            {
                Assert.AreEqual(deposit.savingsAccountId, account2.id);
            }

            Assert.IsTrue(foundDeposits2.Count == 2);

        }

        [TestMethod]
        public async Task GetSavingsWithdrawalsByDateRangeTests()
        {
            SavingsAccount account1 = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            SavingsAccount account2 = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            SavingsWithdrawal withdrawal1 = new SavingsWithdrawal()
            {
                savingsAccount = account1,
                savingsAccountId = account1.id,
                transactionDate = DateTime.Parse("1/19/2018"),
                transactionAmount = 102.48M
            };

            SavingsWithdrawal withdrawal2 = new SavingsWithdrawal()
            {
                savingsAccount = account2,
                savingsAccountId = account2.id,
                transactionDate = DateTime.Parse("12/5/2017"),
                transactionAmount = 105.87M
            };

            SavingsWithdrawal withdrawal3 = new SavingsWithdrawal()
            {
                savingsAccount = account2,
                savingsAccountId = account2.id,
                transactionDate = DateTime.Parse("1/5/2018"),
                transactionAmount = 85.43M
            };

            SavingsWithdrawal withdrawal4 = new SavingsWithdrawal()
            {
                savingsAccount = account2,
                savingsAccountId = account2.id,
                transactionDate = DateTime.Parse("11/5/2017"),
                transactionAmount = 123.87M
            };

            List<SavingsAccount> accounts = new List<SavingsAccount>();
            accounts.Add(account1);
            accounts.Add(account2);

            foreach (SavingsAccount account in accounts)
            {
                await repository.AddSavingsAccountAsync(account);
            }
            await repository.SaveChangesAsync();

            List<SavingsWithdrawal> withdrawals = new List<SavingsWithdrawal>();
            withdrawals.Add(withdrawal1);
            withdrawals.Add(withdrawal2);
            withdrawals.Add(withdrawal3);
            withdrawals.Add(withdrawal4);

            decimal account1RunningBalance = account1.currentBalance;
            decimal account2RunningBalance = account2.currentBalance;

            foreach (SavingsWithdrawal deposit in withdrawals)
            {
                // Adjust the balance on the account by the transaction amount on the deposit
                if (deposit.savingsAccountId == account1.id)
                {
                    account1RunningBalance -= deposit.transactionAmount;
                    deposit.savingsAccount.currentBalance = account1RunningBalance;
                }
                else
                {
                    account2RunningBalance -= deposit.transactionAmount;
                    deposit.savingsAccount.currentBalance = account2RunningBalance;
                }

                await repository.AddSavingsWithdrawalAsync(deposit);
            }

            await repository.SaveChangesAsync();

            DateTime startDate = DateTime.Parse("1/1/2018");
            DateTime endDate = DateTime.Parse("1/31/2018");

            List<SavingsWithdrawal> foundDeposits1 = new List<SavingsWithdrawal>();
            foundDeposits1.AddRange(await validationRepository.GetSavingsWithdrawalsByDateRangeAsync(account1.id, startDate, endDate));

            foreach (SavingsWithdrawal withdrawal in foundDeposits1)
            {
                Assert.AreEqual(withdrawal.savingsAccountId, account1.id);
            }

            Assert.IsTrue(foundDeposits1.Count == 1);

            startDate = DateTime.Parse("11/1/2017");
            endDate = DateTime.Parse("12/31/2017");
            List<SavingsWithdrawal> foundDeposits2 = new List<SavingsWithdrawal>();
            foundDeposits2.AddRange(await validationRepository.GetSavingsWithdrawalsByDateRangeAsync(account2.id, startDate, endDate));

            foreach (SavingsWithdrawal withdrawal in foundDeposits2)
            {
                Assert.AreEqual(withdrawal.savingsAccountId, account2.id);
            }

            Assert.IsTrue(foundDeposits2.Count == 2);

        }

        [TestMethod]
        public async Task UpdateBudgetCategoryTests()
        {
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0
            };


            await repository.AddBudgetCategoryAsync(category);
            await repository.SaveChangesAsync();

            BudgetCategory testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);

            Assert.AreEqual(category.id, testCategory.id);
            Assert.AreEqual(category.categoryName, testCategory.categoryName);
            Assert.AreEqual(category.budgetAmount, testCategory.budgetAmount);

            category.budgetAmount = 800;
            await repository.UpdateBudgetCategoryAsync(category);
            await repository.SaveChangesAsync();

            testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.budgetAmount, 800);

        }

        [TestMethod]
        public async Task UpdateCheckingAccountTests()
        {
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };


            await repository.AddCheckingAccountAsync(account);
            await repository.SaveChangesAsync();

            CheckingAccount testAccount = await validationRepository.GetCheckingAccountAsync(account.id);

            Assert.AreEqual(account.id, testAccount.id);
            Assert.AreEqual(account.accountNumber, testAccount.accountNumber);
            Assert.AreEqual(account.routingNumber, testAccount.routingNumber);

            account.currentBalance = 0;
            await repository.UpdateCheckingAccountAsync(account);
            await repository.SaveChangesAsync();

            testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, 0);
        }

        [TestMethod]
        public async Task UpdateCheckingDepositTests()
        {
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            CheckingDeposit deposit = new CheckingDeposit()
            {
                checkingAccount = account,
                checkingAccountId = account.id,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };


            // Write account to db and test:
            await repository.AddCheckingAccountAsync(account);
            await repository.SaveChangesAsync();

            CheckingAccount testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, account.currentBalance);

            // Adjust the balance on the account by the transaction amount on the deposit
            decimal newBalance = account.currentBalance + deposit.transactionAmount;
            deposit.checkingAccount.currentBalance = deposit.checkingAccount.currentBalance + deposit.transactionAmount;

            await repository.AddCheckingDepositAsync(deposit);
            await repository.SaveChangesAsync();


            // test the changes
            testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, newBalance);

            CheckingDeposit testDeposit = await validationRepository.GetCheckingDepositAsync(deposit.id);
            Assert.AreEqual(deposit.id, testDeposit.id);

            testDeposit.transactionAmount = 50;
            testAccount.currentBalance = testAccount.currentBalance - 50;
            decimal newTestBalance = testAccount.currentBalance;

            testDeposit.checkingAccount = testAccount;
            await repository.UpdateCheckingDepositAsync(testDeposit);

            CheckingAccount updateAccount = await repository.GetCheckingAccountAsync(testDeposit.checkingAccountId);
            Assert.AreEqual(updateAccount.currentBalance, newTestBalance);

            CheckingDeposit updateTestDeposit = await repository.GetCheckingDepositAsync(testDeposit.id);
            Assert.AreEqual(updateTestDeposit.id, testDeposit.id);
            Assert.AreEqual(updateTestDeposit.transactionAmount, testDeposit.transactionAmount);
        }

        [TestMethod]
        public async Task UpdateCheckingWithdrawalTests()
        {
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            CheckingWithdrawal withdrawal = new CheckingWithdrawal()
            {
                checkingAccount = account,
                checkingAccountId = account.id,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };


            // Write account to db and test:
            await repository.AddCheckingAccountAsync(account);
            await repository.SaveChangesAsync();

            CheckingAccount testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, account.currentBalance);

            // Adjust the balance on the account by the transaction amount on the withdrawal
            decimal newBalance = account.currentBalance - withdrawal.transactionAmount;
            withdrawal.checkingAccount.currentBalance = withdrawal.checkingAccount.currentBalance - withdrawal.transactionAmount;

            await repository.AddCheckingWithdrawalAsync(withdrawal);
            await repository.SaveChangesAsync();

            // test the changes
            testAccount = await validationRepository.GetCheckingAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, newBalance);

            CheckingWithdrawal testWithdrawal = await validationRepository.GetCheckingWithdrawalAsync(withdrawal.id);
            Assert.AreEqual(withdrawal.id, testWithdrawal.id);

            testWithdrawal.transactionAmount = 50;
            testAccount.currentBalance = testAccount.currentBalance + 50;
            decimal newTestBalance = testAccount.currentBalance;

            testWithdrawal.checkingAccount = testAccount;
            await repository.UpdateCheckingWithdrawalAsync(testWithdrawal);

            CheckingAccount updateAccount = await repository.GetCheckingAccountAsync(testAccount.id);
            Assert.AreEqual(updateAccount.currentBalance, newTestBalance);

            CheckingWithdrawal updateTestWithdrawal = await repository.GetCheckingWithdrawalAsync(testWithdrawal.id);
            Assert.AreEqual(updateTestWithdrawal.id, testWithdrawal.id);
            Assert.AreEqual(updateTestWithdrawal.transactionAmount, testWithdrawal.transactionAmount);
        }

        [TestMethod]
        public async Task UpdateExpenseItemTests()
        {
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0,
                categoryType = BudgetCategoryType.Expense
            };

            ExpenseItem expItm = new ExpenseItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                dateCreated = DateTime.Now,
                description = "Dummy Expense Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 250
            };

            await repository.AddBudgetCategoryAsync(category);
            await repository.SaveChangesAsync();

            BudgetCategory testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, category.budgetAmount);
            Assert.IsTrue(testCategory.budgetAmount == 0);

            expItm.budgetCategory.budgetAmount = expItm.budgetCategory.budgetAmount + expItm.BudgetedAmount;

            await repository.AddExpenseItemAsync(expItm);
            await repository.SaveChangesAsync();

            ExpenseItem testExpItem = await validationRepository.GetExpenseItemAsync(expItm.id);
            Assert.AreEqual(testExpItem.id, expItm.id);
            Assert.AreEqual(testExpItem.BudgetedAmount, expItm.BudgetedAmount);
            Assert.AreEqual(testExpItem.budgetCategoryId, expItm.budgetCategoryId);

            testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, expItm.budgetCategory.budgetAmount);
            Assert.AreEqual(testCategory.budgetAmount, 250);

            expItm.budgetCategory.budgetAmount = 200;
            expItm.BudgetedAmount = 200;

            await repository.UpdateExpenseItemAsync(expItm);
            await repository.SaveChangesAsync();

            testExpItem = await validationRepository.GetExpenseItemAsync(expItm.id);
            Assert.AreEqual(testExpItem.id, expItm.id);
            Assert.AreEqual(testExpItem.BudgetedAmount, expItm.BudgetedAmount);
            Assert.AreEqual(testExpItem.budgetCategoryId, expItm.budgetCategoryId);

            testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, expItm.budgetCategory.budgetAmount);
            Assert.AreEqual(testCategory.budgetAmount, 200);

        }

        [TestMethod]
        public async Task UpdateIncomeItemTests()
        {
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Test Category",
                systemCategory = false,
                userSelected = true,
                budgetAmount = 0,
                categoryType = BudgetCategoryType.Income
            };

            IncomeItem incItem = new IncomeItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                dateCreated = DateTime.Now,
                description = "Dummy Income Item",
                notation = "Used for testing purposes",
                recurring = false,
                BudgetedAmount = 2450
            };


            await repository.AddBudgetCategoryAsync(category);
            await repository.SaveChangesAsync();

            BudgetCategory testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, category.budgetAmount);
            Assert.IsTrue(testCategory.budgetAmount == 0);

            incItem.budgetCategory.budgetAmount = incItem.budgetCategory.budgetAmount + incItem.BudgetedAmount;

            await repository.AddIncomeItemAsync(incItem);
            await repository.SaveChangesAsync();

            IncomeItem testIncItem = await validationRepository.GetIncomeItemAsync(incItem.id);
            Assert.AreEqual(testIncItem.id, incItem.id);
            Assert.AreEqual(testIncItem.BudgetedAmount, incItem.BudgetedAmount);
            Assert.AreEqual(testIncItem.budgetCategoryId, incItem.budgetCategoryId);

            testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, incItem.budgetCategory.budgetAmount);
            Assert.AreEqual(testCategory.budgetAmount, 2450);

            incItem.budgetCategory.budgetAmount = 200;
            incItem.BudgetedAmount = 200;

            await repository.UpdateIncomeItemAsync(incItem);
            await repository.SaveChangesAsync();

            testIncItem = await validationRepository.GetIncomeItemAsync(incItem.id);
            Assert.AreEqual(testIncItem.id, incItem.id);
            Assert.AreEqual(testIncItem.BudgetedAmount, incItem.BudgetedAmount);
            Assert.AreEqual(testIncItem.budgetCategoryId, incItem.budgetCategoryId);

            testCategory = await validationRepository.GetBudgetCategoryAsync(category.id);
            Assert.AreEqual(testCategory.id, category.id);
            Assert.AreEqual(testCategory.budgetAmount, incItem.budgetCategory.budgetAmount);
            Assert.AreEqual(testCategory.budgetAmount, 200);
        }

        [TestMethod]
        public async Task UpdateSavingsAccountTests()
        {
            SavingsAccount account = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };


            await repository.AddSavingsAccountAsync(account);
            await repository.SaveChangesAsync();

            SavingsAccount testAccount = await validationRepository.GetSavingsAccountAsync(account.id);

            Assert.AreEqual(account.id, testAccount.id);
            Assert.AreEqual(account.accountNumber, testAccount.accountNumber);
            Assert.AreEqual(account.routingNumber, testAccount.routingNumber);

            // Change the savings account info
            account.accountNumber = "888888";
            await repository.UpdateSavingsAccountAsync(account);
            await repository.SaveChangesAsync();

            // test the changes
            testAccount = await validationRepository.GetSavingsAccountAsync(account.id);
            Assert.AreEqual(account.id, testAccount.id);
            Assert.AreEqual(account.accountNumber, testAccount.accountNumber);
            Assert.AreEqual(account.routingNumber, testAccount.routingNumber);

        }

        [TestMethod]
        public async Task UpdateSavingsDepositTests()
        {
            SavingsAccount account = new SavingsAccount()
            {
                routingNumber = "111111",
                accountNumber = "222222",
                bankName = "Bank of Nowhere",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                currentBalance = 1000
            };

            SavingsDeposit deposit = new SavingsDeposit()
            {
                savingsAccount = account,
                savingsAccountId = account.id,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };


            // Write account to db and test:
            await repository.AddSavingsAccountAsync(account);
            await repository.SaveChangesAsync();

            SavingsAccount testAccount = await validationRepository.GetSavingsAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, account.currentBalance);

            // Adjust the balance on the account by the transaction amount on the deposit
            decimal newBalance = account.currentBalance + deposit.transactionAmount;
            deposit.savingsAccount.currentBalance = deposit.savingsAccount.currentBalance + deposit.transactionAmount;

            await repository.AddSavingsDepositAsync(deposit);
            await repository.SaveChangesAsync();

            // test the changes
            testAccount = await validationRepository.GetSavingsAccountAsync(account.id);
            Assert.AreEqual(testAccount.currentBalance, newBalance);

            SavingsDeposit testDeposit = await validationRepository.GetSavingsDepositAsync(deposit.id);
            Assert.AreEqual(deposit.id, testDeposit.id);

            // Change the deposit object and update
            testDeposit.transactionAmount = 50;
            testAccount.currentBalance = testAccount.currentBalance - 50;
            decimal newTestBalance = testAccount.currentBalance;

            testDeposit.savingsAccount = testAccount;
            await repository.UpdateSavingsDepositAsync(testDeposit);
            await repository.SaveChangesAsync();

            // test the updates
            SavingsAccount updateAccount = await validationRepository.GetSavingsAccountAsync(testDeposit.savingsAccountId);
            Assert.AreEqual(updateAccount.currentBalance, newTestBalance);

            SavingsDeposit updateTestDeposit = await validationRepository.GetSavingsDepositAsync(testDeposit.id);
            Assert.AreEqual(updateTestDeposit.id, testDeposit.id);
            Assert.AreEqual(updateTestDeposit.transactionAmount, testDeposit.transactionAmount);


        }

        [TestMethod]
        public async Task GetBankAccountFundsTransferTests()
        {
            BankAccountFundsTransfer transfer = new BankAccountFundsTransfer();
            transfer.sourceAccountId = 0;
            transfer.destinationAccountId = 0;
            transfer.sourceAccountBeginningBalance = 100;
            transfer.sourceAccountEndingBalance = 200;
            transfer.destinationAccountBeginningBalance = 200;
            transfer.destinationAccountEndingBalance = 100;
            transfer.transactionAmount = 100;
            transfer.transactionDate = DateTime.Now;
            transfer.dateCreated = DateTime.Now;
            transfer.dateModified = DateTime.Now;

            await repository.AddBankAccountFundsTransferAsync(transfer);
            await repository.SaveChangesAsync();

            BankAccountFundsTransfer testTransfer = await validationRepository.GetBankAccountFundsTransfer(transfer.id);
            Assert.AreEqual(transfer.id, testTransfer.id);
            Assert.AreEqual(transfer.sourceAccountId, testTransfer.sourceAccountId);
            Assert.AreEqual(transfer.destinationAccountId, testTransfer.destinationAccountId);
            Assert.AreEqual(transfer.transactionDate, testTransfer.transactionDate);
            Assert.AreEqual(transfer.transactionAmount, testTransfer.transactionAmount);
            Assert.AreEqual(transfer.destinationAccountBeginningBalance, testTransfer.destinationAccountBeginningBalance);
            Assert.AreEqual(transfer.destinationAccountEndingBalance, testTransfer.destinationAccountEndingBalance);
            Assert.AreEqual(transfer.sourceAccountBeginningBalance, testTransfer.sourceAccountBeginningBalance);
            Assert.AreEqual(transfer.sourceAccountEndingBalance, testTransfer.sourceAccountEndingBalance);
        }

        [TestMethod]
        public async Task AddBankAccountFundsTransferTests()
        {

            BankAccountFundsTransfer transfer = new BankAccountFundsTransfer();
            transfer.sourceAccountId = 0;
            transfer.destinationAccountId = 0;
            transfer.sourceAccountBeginningBalance = 100;
            transfer.sourceAccountEndingBalance = 200;
            transfer.destinationAccountBeginningBalance = 200;
            transfer.destinationAccountEndingBalance = 100;
            transfer.transactionAmount = 100;
            transfer.transactionDate = DateTime.Now;
            transfer.dateCreated = DateTime.Now;
            transfer.dateModified = DateTime.Now;

            await repository.AddBankAccountFundsTransferAsync(transfer);
            await repository.SaveChangesAsync();

            BankAccountFundsTransfer testTransfer = await validationRepository.GetBankAccountFundsTransfer(transfer.id);
            Assert.AreEqual(transfer.id, testTransfer.id);
            Assert.AreEqual(transfer.sourceAccountId, testTransfer.sourceAccountId);
            Assert.AreEqual(transfer.destinationAccountId, testTransfer.destinationAccountId);
            Assert.AreEqual(transfer.transactionDate, testTransfer.transactionDate);
            Assert.AreEqual(transfer.transactionAmount, testTransfer.transactionAmount);
            Assert.AreEqual(transfer.destinationAccountBeginningBalance, testTransfer.destinationAccountBeginningBalance);
            Assert.AreEqual(transfer.destinationAccountEndingBalance, testTransfer.destinationAccountEndingBalance);
            Assert.AreEqual(transfer.sourceAccountBeginningBalance, testTransfer.sourceAccountBeginningBalance);
            Assert.AreEqual(transfer.sourceAccountEndingBalance, testTransfer.sourceAccountEndingBalance);
        }

        [TestMethod]
        public async Task UpdateBankAccountFundsTransferTests()
        {
            BankAccountFundsTransfer transfer = new BankAccountFundsTransfer();
            transfer.sourceAccountId = 0;
            transfer.destinationAccountId = 0;
            transfer.sourceAccountBeginningBalance = 100;
            transfer.sourceAccountEndingBalance = 200;
            transfer.destinationAccountBeginningBalance = 200;
            transfer.destinationAccountEndingBalance = 100;
            transfer.transactionAmount = 100;
            transfer.transactionDate = DateTime.Now;
            transfer.dateCreated = DateTime.Now;
            transfer.dateModified = DateTime.Now;

            await repository.AddBankAccountFundsTransferAsync(transfer);
            await repository.SaveChangesAsync();

            BankAccountFundsTransfer testTransfer = await validationRepository.GetBankAccountFundsTransfer(transfer.id);
            Assert.AreEqual(transfer.id, testTransfer.id);
            Assert.AreEqual(transfer.sourceAccountId, testTransfer.sourceAccountId);
            Assert.AreEqual(transfer.destinationAccountId, testTransfer.destinationAccountId);
            Assert.AreEqual(transfer.transactionDate, testTransfer.transactionDate);
            Assert.AreEqual(transfer.transactionAmount, testTransfer.transactionAmount);
            Assert.AreEqual(transfer.destinationAccountBeginningBalance, testTransfer.destinationAccountBeginningBalance);
            Assert.AreEqual(transfer.destinationAccountEndingBalance, testTransfer.destinationAccountEndingBalance);
            Assert.AreEqual(transfer.sourceAccountBeginningBalance, testTransfer.sourceAccountBeginningBalance);
            Assert.AreEqual(transfer.sourceAccountEndingBalance, testTransfer.sourceAccountEndingBalance);

            transfer.voided = true;

            await repository.UpdateBankAccountFundsTransferAsync(transfer);
            await repository.SaveChangesAsync();

            testTransfer = await validationRepository.GetBankAccountFundsTransfer(transfer.id);
            Assert.AreEqual(transfer.id, testTransfer.id);
            Assert.AreEqual(transfer.sourceAccountId, testTransfer.sourceAccountId);
            Assert.AreEqual(transfer.destinationAccountId, testTransfer.destinationAccountId);
            Assert.AreEqual(transfer.transactionDate, testTransfer.transactionDate);
            Assert.AreEqual(transfer.transactionAmount, testTransfer.transactionAmount);
            Assert.AreEqual(transfer.destinationAccountBeginningBalance, testTransfer.destinationAccountBeginningBalance);
            Assert.AreEqual(transfer.destinationAccountEndingBalance, testTransfer.destinationAccountEndingBalance);
            Assert.AreEqual(transfer.sourceAccountBeginningBalance, testTransfer.sourceAccountBeginningBalance);
            Assert.AreEqual(transfer.sourceAccountEndingBalance, testTransfer.sourceAccountEndingBalance);
            Assert.IsTrue(testTransfer.voided);

        }
    }
}
