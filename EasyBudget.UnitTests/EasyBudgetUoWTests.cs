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
using EasyBudget.Data;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;
using EasyBudget.Business;
using EasyBudget.Business.UoWResults;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EasyBudget.UnitTests
{
    [TestClass]
    public class EasyBudgetUoWTests
    {
        Mock<IEasyBudgetRepository> repositoryMock;

        [TestInitialize]
        public void InitializeRepository()
        {
            repositoryMock = new Mock<IEasyBudgetRepository>();
        }

        [TestCleanup]
        public void CleanupResources() 
        {
            repositoryMock = null;
        }

        [TestMethod]
        public async Task GetBudgetCategoryTests()
        {

            int id = 1;

            // Set up test data
            BudgetCategory category = new BudgetCategory();
            category.id = id;
            category.categoryName = "Dummy Budget Category";
            category.dateCreated = DateTime.Now;
            category.description = "Budget Category for Unit Testing";
            
            // Happy Path test
            repositoryMock.Setup(r => r.GetBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(category);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                UnitOfWorkResults<BudgetCategory> uowResults = await uow.GetBudgetCategoryAsync(id);
                Assert.IsNotNull(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
            }

            // Empty int Test
            id = 0;
            category = null;
            repositoryMock.Setup(r => r.GetBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(category);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                UnitOfWorkResults<BudgetCategory> uowResults = await uow.GetBudgetCategoryAsync(id);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsNull(uowResults.WorkException);
            }

            // Exception Test
            id = 0;
            category = null;
            Exception ex = new Exception();
            repositoryMock.Setup(r => r.GetBudgetCategoryAsync(It.IsAny<int>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                UnitOfWorkResults<BudgetCategory> uowResults = await uow.GetBudgetCategoryAsync(id);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsNotNull(uowResults.WorkException);
            }

        }

        [TestMethod]
        public async Task GetMatchingBudgetCategoriesTests()
        {
            string searchText = "dummy text";

            // Set up test data
            List<BudgetCategory> categories = new List<BudgetCategory>();
            categories.Add(new BudgetCategory() {
                categoryName = "Household Expenses",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            });
            categories.Add(new BudgetCategory(){
                categoryName = "Utilities",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            });
            categories.Add(new BudgetCategory(){
                categoryName = "Entertainment",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            });


            // Happy Path test
            repositoryMock.Setup(r => r.GetMatchingCategoriesAsync(It.IsAny<string>())).ReturnsAsync(categories);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                BudgetCategoriesResults _results = await uow.GetMatchingBudgetCategoriesAsync(searchText);
                Assert.IsNotNull(_results.Results);
                Assert.AreEqual(_results.Results.Count, categories.Count);
                Assert.IsNull(_results.WorkException);
                Assert.IsTrue(_results.Successful);
            }

            // Empty search text test
            searchText = string.Empty;
            List<BudgetCategory> emptyCategoryList = new List<BudgetCategory>();
            Exception exEmptyText = new Exception("Valid search text must be provided");
            repositoryMock.Setup(r => r.GetMatchingCategoriesAsync(It.IsAny<string>())).ReturnsAsync(emptyCategoryList);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                BudgetCategoriesResults _results = await uow.GetMatchingBudgetCategoriesAsync(searchText);
                Assert.IsNull(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
            }

            // Null search text test
            searchText = null;
            Exception exNullText = new Exception("Valid search text must be provided");
            repositoryMock.Setup(r => r.GetMatchingCategoriesAsync(It.IsAny<string>())).ReturnsAsync(emptyCategoryList);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                BudgetCategoriesResults _results = await uow.GetMatchingBudgetCategoriesAsync(searchText);
                Assert.IsNull(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
            }
        }

        [TestMethod]
        public async Task AddBudgetCategoryTests()
        {
            // Set up test data
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Household Expenses",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            // Happy Path test
            repositoryMock.Setup(r => r.AddBudgetCategoryAsync(It.IsAny<BudgetCategory>())).ReturnsAsync(category);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                BudgetCategoryResults _results = await uow.AddBudgetCategoryAsync(category);
                Assert.IsNotNull(_results.Results);
                Assert.IsNull(_results.WorkException);
                Assert.IsTrue(_results.Successful);
            }

            // Duplicate Category Name test
            Exception exDuplicateName = new Exception("An existing category already exists with the same name");
            repositoryMock.Setup(r => r.AddBudgetCategoryAsync(It.IsAny<BudgetCategory>())).ThrowsAsync(exDuplicateName);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                BudgetCategoryResults _results = await uow.AddBudgetCategoryAsync(category);
                Assert.IsNull(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.ToLower().Contains("an existing category already exists with the same name"));
            }

            // Duplicate id test
            Exception exDupePrimaryKey = new Exception("A category already exists with the same Primary Key value");
            repositoryMock.Setup(r => r.AddBudgetCategoryAsync(It.IsAny<BudgetCategory>())).ThrowsAsync(exDupePrimaryKey);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                BudgetCategoryResults _results = await uow.AddBudgetCategoryAsync(category);
                Assert.IsNull(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.ToLower().Contains("a category already exists with the same primary key value"));
            }
        }

        [TestMethod]
        public async Task UpdateBudgetCategoryTests()
        {
            // Set up test data
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Household Expenses",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            // Happy Path test
            repositoryMock.Setup(r => r.UpdateBudgetCategoryAsync(It.IsAny<BudgetCategory>())).Returns(Task.CompletedTask);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                BudgetCategoryResults _results = await uow.UpdateBudgetCategoryAsync(category);
                Assert.IsNotNull(_results.Results);
                Assert.IsNull(_results.WorkException);
                Assert.IsTrue(_results.Successful);
            }

            // Exception Handling test
            Exception ex = new Exception();
            repositoryMock.Setup(r => r.UpdateBudgetCategoryAsync(It.IsAny<BudgetCategory>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                BudgetCategoryResults _results = await uow.UpdateBudgetCategoryAsync(category);
                Assert.IsNull(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
            }

            // Duplicate Category Name Exists test
            Exception exDuplicateName = new Exception("An existing category already exists with the same name");
            repositoryMock.Setup(r => r.AddBudgetCategoryAsync(It.IsAny<BudgetCategory>())).ThrowsAsync(exDuplicateName);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                BudgetCategoryResults _results = await uow.AddBudgetCategoryAsync(category);
                Assert.IsNull(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.ToLower().Contains("an existing category already exists with the same name"));
            }

        }

        [TestMethod]
        public async Task DeleteBudgetCategoryTests()
        {
            // Set up test data
            BudgetCategory expCategory = new BudgetCategory()
            {
                categoryName = "Household Expenses",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                categoryType = BudgetCategoryType.Expense
            };

            ExpenseItem expItm = new ExpenseItem()
            {
                budgetCategoryId = expCategory.id,
                budgetCategory = expCategory,
                BudgetedAmount = 250,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                description = "Dummy Expense Item"
            };

            BudgetCategory incCategory = new BudgetCategory()
            {
                categoryName = "Paycheck",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                categoryType = BudgetCategoryType.Income
            };

            IncomeItem incItm = new IncomeItem()
            {
                budgetCategory = incCategory,
                budgetCategoryId = incCategory.id,
                BudgetedAmount = 1250,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                description = "Dummy Income Item"
            };

            ICollection<ExpenseItem> expItems = new List<ExpenseItem>();
            ICollection<IncomeItem> incItems = new List<IncomeItem>();

            // Happy Path test
            repositoryMock.Setup(r => r.GetIncomeItemsForBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(incItems);
            repositoryMock.Setup(r => r.GetExpenseItemsForBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(expItems);
            repositoryMock.Setup(r => r.DeleteBudgetCategoryAsync(It.IsAny<BudgetCategory>())).Returns(Task.CompletedTask);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteBudgetCategoryResults _results = await uow.DeleteBudgetCategoryAsync(expCategory);
                Assert.IsTrue(_results.Results);
                Assert.IsNull(_results.WorkException);
                Assert.IsTrue(_results.Successful);
            }

            // Existing Income Items test
            expItems.Clear();
            incItems.Add(incItm);
            Exception exExistingIncomeItems = new Exception("You must first remove all Income and Expense Items from this Category before deleting it.");
            repositoryMock.Setup(r => r.GetIncomeItemsForBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(incItems);
            repositoryMock.Setup(r => r.GetExpenseItemsForBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(expItems);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteBudgetCategoryResults _results = await uow.DeleteBudgetCategoryAsync(expCategory);
                Assert.IsFalse(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.AreEqual(_results.WorkException.Message, exExistingIncomeItems.Message);
                Assert.IsFalse(_results.Successful);
            }

            // Existing Expense Items test
            incItems.Clear();
            expItems.Add(expItm);
            incItems.Add(incItm);
            Exception exExistingExpenseItems = new Exception("You must first remove all Income and Expense Items from this Category before deleting it.");
            repositoryMock.Setup(r => r.GetIncomeItemsForBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(incItems);
            repositoryMock.Setup(r => r.GetExpenseItemsForBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(expItems);
            repositoryMock.Setup(r => r.GetExpenseItemsForBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(expItems);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteBudgetCategoryResults _results = await uow.DeleteBudgetCategoryAsync(expCategory);
                Assert.IsFalse(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.AreEqual(_results.WorkException.Message, exExistingExpenseItems.Message);
                Assert.IsFalse(_results.Successful);
            }

            // Exception Handling test
            incItems.Clear();
            expItems.Clear();
            Exception ex = new Exception();
            repositoryMock.Setup(r => r.GetIncomeItemsForBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(incItems);
            repositoryMock.Setup(r => r.GetExpenseItemsForBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(expItems);
            repositoryMock.Setup(r => r.DeleteBudgetCategoryAsync(It.IsAny<BudgetCategory>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteBudgetCategoryResults _results = await uow.DeleteBudgetCategoryAsync(expCategory);
                Assert.IsFalse(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
            }
        }

        [TestMethod]
        public async Task GetExpenseItemTests()
        {
            // Set up test data
            BudgetCategory category = new BudgetCategory()
            {
                categoryIcon = AppIcon.None,
                categoryName = "Category",
                systemCategory = false,
                budgetAmount = 0
            };

            ExpenseItem expenseItem = new ExpenseItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                notation = "Test item",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetExpenseItemAsync(It.IsAny<int>())).ReturnsAsync(expenseItem);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                ExpenseItemResults uowResults = await uow.GetExpenseItemAsync(expenseItem.id);
                Assert.IsNotNull(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
                Assert.IsNotNull(uowResults.Results.budgetCategory);
                Assert.AreEqual(uowResults.Results.budgetCategoryId, category.id);
            }

            // Not Found test
            repositoryMock.Setup(r => r.GetExpenseItemAsync(It.IsAny<int>())).ReturnsAsync((ExpenseItem)null);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                ExpenseItemResults uowResults = await uow.GetExpenseItemAsync(expenseItem.id);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsTrue(uowResults.Message.ToLower() == "no matching expenseitem found");
            }

            // Excaption Handling test
            Exception ex = new Exception("dummy exception");
            repositoryMock.Setup(r => r.GetExpenseItemAsync(It.IsAny<int>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                ExpenseItemResults uowResults = await uow.GetExpenseItemAsync(expenseItem.id);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsNotNull(uowResults.WorkException);
            }
        }

        [TestMethod]
        public async Task GetIncomeItemTests()
        {
            // Set up test data
            BudgetCategory category = new BudgetCategory()
            {
                categoryIcon = AppIcon.None,
                categoryName = "Category",
                systemCategory = false,
                budgetAmount = 0
            };

            IncomeItem incomeItem = new IncomeItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                notation = "Test item",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetIncomeItemAsync(It.IsAny<int>())).ReturnsAsync(incomeItem);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                IncomeItemResults uowResults = await uow.GetIncomeItemAsync(incomeItem.id);
                Assert.IsNotNull(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
                Assert.IsNotNull(uowResults.Results.budgetCategory);
                Assert.AreEqual(uowResults.Results.budgetCategoryId, category.id);
            }

            // Not Found test
            repositoryMock.Setup(r => r.GetIncomeItemAsync(It.IsAny<int>())).ReturnsAsync((IncomeItem)null);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                IncomeItemResults uowResults = await uow.GetIncomeItemAsync(incomeItem.id);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsTrue(uowResults.Message.ToLower() == "no matching incomeitem found");
            }

            // Excaption Handling test
            Exception ex = new Exception("dummy exception");
            repositoryMock.Setup(r => r.GetIncomeItemAsync(It.IsAny<int>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                IncomeItemResults uowResults = await uow.GetIncomeItemAsync(incomeItem.id);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsNotNull(uowResults.WorkException);
            }
        }

        [TestMethod]
        public async Task GetCategoryExpenseItemsTests()
        {
            // Set up test data
            BudgetCategory category = new BudgetCategory()
            {
                categoryIcon = AppIcon.None,
                categoryName = "Category",
                systemCategory = false,
                budgetAmount = 0
            };

            List<ExpenseItem> items = new List<ExpenseItem>() {
                new ExpenseItem()
                {
                    budgetCategory = category,
                    budgetCategoryId = category.id,
                    notation = "Test item 1",
                    dateCreated = DateTime.Now,
                    dateModified = DateTime.Now
                },
                new ExpenseItem()
                {
                    budgetCategory = category,
                    budgetCategoryId = category.id,
                    notation = "Test item 2",
                    dateCreated = DateTime.Now,
                    dateModified = DateTime.Now
                }
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetExpenseItemsForBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(items);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                ExpenseItemsResults uowResults = await uow.GetCategoryExpenseItemsAsync(category);
                Assert.IsNotNull(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
                foreach (ExpenseItem itm in uowResults.Results)
                {
                    Assert.IsNotNull(itm.budgetCategory);
                    Assert.AreEqual(itm.budgetCategoryId, category.id);
                }
                Assert.IsNull(uowResults.WorkException);
            }

            // No Items Found test
            items = new List<ExpenseItem>();
            repositoryMock.Setup(r => r.GetExpenseItemsForBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(items);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                ExpenseItemsResults uowResults = await uow.GetCategoryExpenseItemsAsync(category);
                Assert.IsNotNull(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
                Assert.IsTrue(uowResults.Results.Count == 0);
                Assert.IsNull(uowResults.WorkException);
                Assert.AreEqual(uowResults.Message, "No Expense Items found for Budget Category " + category.categoryName);
            }

            // Exception Handling test
            Exception ex = new Exception();
            repositoryMock.Setup(r => r.GetExpenseItemsForBudgetCategoryAsync(It.IsAny<int>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                ExpenseItemsResults uowResults = await uow.GetCategoryExpenseItemsAsync(category);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsNotNull(uowResults.WorkException);
            }
        }

        [TestMethod]
        public async Task GetCategoryIncomeItemsTests()
        {
            // Set up test data
            BudgetCategory category = new BudgetCategory()
            {
                categoryIcon = AppIcon.None,
                categoryName = "Category",
                systemCategory = false,
                budgetAmount = 0
            };

            List<IncomeItem> items = new List<IncomeItem>() {
                new IncomeItem()
                {
                    budgetCategory = category,
                    budgetCategoryId = category.id,
                    notation = "Test item 1",
                    dateCreated = DateTime.Now,
                    dateModified = DateTime.Now
                },
                new IncomeItem()
                {
                    budgetCategory = category,
                    budgetCategoryId = category.id,
                    notation = "Test item 2",
                    dateCreated = DateTime.Now,
                    dateModified = DateTime.Now
                }
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetIncomeItemsForBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(items);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                IncomeItemsResults uowResults = await uow.GetCategoryIncomeItemsAsync(category);
                Assert.IsNotNull(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
                foreach (IncomeItem itm in uowResults.Results)
                {
                    Assert.IsNotNull(itm.budgetCategory);
                    Assert.AreEqual(itm.budgetCategoryId, category.id);
                }
                Assert.IsNull(uowResults.WorkException);
            }

            // No Items Found test
            items = new List<IncomeItem>();
            repositoryMock.Setup(r => r.GetIncomeItemsForBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(items);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                IncomeItemsResults uowResults = await uow.GetCategoryIncomeItemsAsync(category);
                Assert.IsNotNull(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
                Assert.IsTrue(uowResults.Results.Count == 0);
                Assert.IsNull(uowResults.WorkException);
                Assert.AreEqual(uowResults.Message, "No Income Items found for Budget Category " + category.categoryName);
            }

            // Exception Handling test
            Exception ex = new Exception();
            repositoryMock.Setup(r => r.GetIncomeItemsForBudgetCategoryAsync(It.IsAny<int>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                IncomeItemsResults uowResults = await uow.GetCategoryIncomeItemsAsync(category);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsNotNull(uowResults.WorkException);
            }
        }

        [TestMethod]
        public async Task AddExpenseItemTests()
        {
            // Set up test data
            BudgetCategory category = new BudgetCategory()
            {
                categoryIcon = AppIcon.None,
                categoryName = "Category",
                systemCategory = false,
                budgetAmount = 0
            };

            ExpenseItem expenseItem = new ExpenseItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                notation = "Test item",
                BudgetedAmount = 200,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(category);
            decimal expectedPreviousCategoryBudgetAmount = category.budgetAmount;
            decimal expectedNewCategoryBudgetAmount = category.budgetAmount + expenseItem.BudgetedAmount;
            repositoryMock.Setup(r => r.AddExpenseItemAsync(It.IsAny<ExpenseItem>())).ReturnsAsync(expenseItem);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                ExpenseItemResults _results = await uow.AddExpenseItemAsync(expenseItem);
                Assert.IsNotNull(_results);
                Assert.AreEqual(expectedNewCategoryBudgetAmount, _results.NewBudgetCategoryAmount);
                Assert.IsNull(_results.WorkException);
                Assert.IsTrue(_results.Successful);
            }

            // NULL argument test
            Exception exNULLArgument = new Exception("Expense Item cannot be NULL");
            repositoryMock.Setup(r => r.AddExpenseItemAsync(It.IsAny<ExpenseItem>())).ThrowsAsync(exNULLArgument);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                ExpenseItemResults _results = await uow.AddExpenseItemAsync(null);
                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("Expense Item cannot be NULL"));
            }

            // Duplicate id test
            Exception exDupePrimaryKey = new Exception("An Expense Item already exists with the same Primary Key value");
            repositoryMock.Setup(r => r.AddExpenseItemAsync(It.IsAny<ExpenseItem>())).ThrowsAsync(exDupePrimaryKey);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                ExpenseItemResults _results = await uow.AddExpenseItemAsync(expenseItem);
                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("An Expense Item already exists with the same Primary Key value"));
            }
        }

        [TestMethod]
        public async Task AddIncomeItemTests()
        {
            // Set up test data
            BudgetCategory category = new BudgetCategory()
            {
                categoryIcon = AppIcon.None,
                categoryName = "Category",
                systemCategory = false,
                budgetAmount = 0
            };

            IncomeItem incomeItem = new IncomeItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                notation = "Test item",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                BudgetedAmount = 200
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(category);
            repositoryMock.Setup(r => r.AddIncomeItemAsync(It.IsAny<IncomeItem>())).ReturnsAsync(incomeItem);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            decimal expectedCategoryBudgetAmount = category.budgetAmount + incomeItem.BudgetedAmount;

            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                IncomeItemResults _results = await uow.AddIncomeItemAsync(incomeItem);
                Assert.AreEqual(expectedCategoryBudgetAmount, _results.NewBudgetCategoryAmount);
                Assert.IsNull(_results.WorkException);
                Assert.IsTrue(_results.Successful);
            }

            // NULL argument test
            Exception exNULLArgument = new Exception("Income Item cannot be NULL");
            repositoryMock.Setup(r => r.AddIncomeItemAsync(It.IsAny<IncomeItem>())).ThrowsAsync(exNULLArgument);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                IncomeItemResults _results = await uow.AddIncomeItemAsync(null);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("Income Item cannot be NULL"));
            }

            // Duplicate id test
            Exception exDupePrimaryKey = new Exception("An Income Item already exists with the same Primary Key value");
            repositoryMock.Setup(r => r.AddIncomeItemAsync(It.IsAny<IncomeItem>())).ThrowsAsync(exDupePrimaryKey);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                IncomeItemResults _results = await uow.AddIncomeItemAsync(incomeItem);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("An Income Item already exists with the same Primary Key value"));
            }
        }

        [TestMethod]
        public async Task UpdateExpenseItemTests()
        {
            // Set up test data
            BudgetCategory category = new BudgetCategory()
            {
                categoryIcon = AppIcon.None,
                categoryName = "Category",
                systemCategory = false,
                budgetAmount = 200
            };

            ExpenseItem expenseItem = new ExpenseItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                notation = "Test item",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                BudgetedAmount = 250
            };

            ExpenseItem existingExpenseItem = new ExpenseItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                notation = "Test item",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                BudgetedAmount = 200
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(category);
            decimal expectedPreviousCategoryBudgetAmount = category.budgetAmount;
            decimal expectedNewCategoryBudgetAmount = category.budgetAmount - existingExpenseItem.BudgetedAmount + expenseItem.BudgetedAmount;

            repositoryMock.Setup(r => r.GetExpenseItemAsync(It.IsAny<int>())).ReturnsAsync(existingExpenseItem);
            repositoryMock.Setup(r => r.UpdateExpenseItemAsync(It.IsAny<ExpenseItem>())).Returns(Task.CompletedTask);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                ExpenseItemResults _results = await uow.UpdateExpenseItemAsync(expenseItem);
                Assert.AreEqual(_results.PreviousBudgetCategoryAmount, 200);
                Assert.AreEqual(expectedNewCategoryBudgetAmount, _results.NewBudgetCategoryAmount);
                Assert.IsNull(_results.WorkException);
                Assert.IsTrue(_results.Successful);
            }

            // NULL argument test
            Exception exNULLArgument = new Exception("Expense Item cannot be NULL");
            repositoryMock.Setup(r => r.UpdateExpenseItemAsync(It.IsAny<ExpenseItem>())).ThrowsAsync(exNULLArgument);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                ExpenseItemResults _results = await uow.UpdateExpenseItemAsync(null);
                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("Expense Item cannot be NULL"));
            }

            // Duplicate id test
            Exception exDupePrimaryKey = new Exception();
            repositoryMock.Setup(r => r.UpdateExpenseItemAsync(It.IsAny<ExpenseItem>())).ThrowsAsync(exDupePrimaryKey);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                ExpenseItemResults _results = await uow.UpdateExpenseItemAsync(expenseItem);
                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
            }
        }

        [TestMethod]
        public async Task UpdateIncomeItemTests()
        {
            // Set up test data
            BudgetCategory category = new BudgetCategory()
            {
                categoryIcon = AppIcon.None,
                categoryName = "Category",
                systemCategory = false,
                budgetAmount = 200
            };

            IncomeItem incomeItem = new IncomeItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                notation = "Test item",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                BudgetedAmount = 250
            };

            IncomeItem existingExpenseItem = new IncomeItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                notation = "Test item",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                BudgetedAmount = 200
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(category);
            decimal expectedPreviousCategoryBudgetAmount = category.budgetAmount;
            decimal expectedNewCategoryBudgetAmount = category.budgetAmount - existingExpenseItem.BudgetedAmount + incomeItem.BudgetedAmount;

            repositoryMock.Setup(r => r.GetIncomeItemAsync(It.IsAny<int>())).ReturnsAsync(existingExpenseItem);
            repositoryMock.Setup(r => r.UpdateIncomeItemAsync(It.IsAny<IncomeItem>())).Returns(Task.CompletedTask);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                IncomeItemResults _results = await uow.UpdateIncomeItemAsync(incomeItem);
                Assert.IsNotNull(_results);
                Assert.AreEqual(expectedNewCategoryBudgetAmount, _results.NewBudgetCategoryAmount);
                Assert.IsNull(_results.WorkException);
                Assert.IsTrue(_results.Successful);
            }

            // NULL argument test
            Exception exNULLArgument = new Exception("Income Item cannot be NULL");
            repositoryMock.Setup(r => r.UpdateIncomeItemAsync(It.IsAny<IncomeItem>())).ThrowsAsync(exNULLArgument);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                IncomeItemResults _results = await uow.UpdateIncomeItemAsync(null);
                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("Income Item cannot be NULL"));
            }

            // Exception Handling test
            Exception exDupePrimaryKey = new Exception();
            repositoryMock.Setup(r => r.UpdateIncomeItemAsync(It.IsAny<IncomeItem>())).ThrowsAsync(exDupePrimaryKey);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                IncomeItemResults _results = await uow.UpdateIncomeItemAsync(incomeItem);
                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
            }
        }

        [TestMethod]
        public async Task DeleteExpenseItemTests()
        {
            // Set up test data
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Household Expenses",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                budgetAmount = 250
            };

            ExpenseItem expenseItem = new ExpenseItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                notation = "Test item",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now, 
                BudgetedAmount = 200
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(category);
            decimal expectedPreviousCategoryBudgetAmount = category.budgetAmount;
            decimal expectedNewCategoryBudgetAmount = category.budgetAmount - expenseItem.BudgetedAmount;
            repositoryMock.Setup(r => r.DeleteExpenseItemAsync(It.IsAny<ExpenseItem>())).Returns(Task.CompletedTask);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteExpenseItemResults _results = await uow.DeleteExpenseItemAsync(expenseItem);
                Assert.IsNotNull(_results);
                Assert.AreEqual(expectedNewCategoryBudgetAmount, _results.NewBudgetCategoryAmount);
                Assert.IsNull(_results.WorkException);
                Assert.IsTrue(_results.Successful);
            }

            // NULL argument test
            Exception exNULLArgument = new Exception("Expense Item cannot be NULL");
            repositoryMock.Setup(r => r.DeleteExpenseItemAsync(It.IsAny<ExpenseItem>())).ThrowsAsync(exNULLArgument);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteExpenseItemResults _results = await uow.DeleteExpenseItemAsync(null);
                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("Expense Item cannot be NULL"));
            }

            // Exception Handling test
            Exception ex = new Exception();
            repositoryMock.Setup(r => r.DeleteExpenseItemAsync(It.IsAny<ExpenseItem>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteExpenseItemResults _results = await uow.DeleteExpenseItemAsync(expenseItem);
                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
            }
        }

        [TestMethod]
        public async Task DeleteIncomeItemTests()
        {
            // Set up test data
            BudgetCategory category = new BudgetCategory()
            {
                categoryName = "Household Income",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            IncomeItem incomeItem = new IncomeItem()
            {
                budgetCategory = category,
                budgetCategoryId = category.id,
                notation = "Test item",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetBudgetCategoryAsync(It.IsAny<int>())).ReturnsAsync(category);
            decimal expectedPreviousCategoryBudgetAmount = category.budgetAmount;
            decimal expectedNewCategoryBudgetAmount = category.budgetAmount - incomeItem.BudgetedAmount;
            repositoryMock.Setup(r => r.DeleteIncomeItemAsync(It.IsAny<IncomeItem>())).Returns(Task.CompletedTask);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteIncomeItemResults _results = await uow.DeleteIncomeItemAsync(incomeItem);
                Assert.IsNotNull(_results);
                Assert.AreEqual(expectedNewCategoryBudgetAmount, _results.NewBudgetCategoryAmount);
                Assert.IsNull(_results.WorkException);
                Assert.IsTrue(_results.Successful);
            }

            // NULL argument test
            Exception exNULLArgument = new Exception("Income Item cannot be NULL");
            repositoryMock.Setup(r => r.DeleteIncomeItemAsync(It.IsAny<IncomeItem>())).ThrowsAsync(exNULLArgument);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteIncomeItemResults _results = await uow.DeleteIncomeItemAsync(null);
                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("Income Item cannot be NULL"));
            }

            // Exception Handling test
            Exception ex = new Exception();
            repositoryMock.Setup(r => r.DeleteIncomeItemAsync(It.IsAny<IncomeItem>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteIncomeItemResults _results = await uow.DeleteIncomeItemAsync(incomeItem);
                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
            }
        }

        [TestMethod]
        public async Task GetCheckingAccountTests()
        {
            // Set up test data
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111111",
                accountNumber = "222222",
                bankName = "National Bank of Nowhere",
                accountNickname = "NBN",
                currentBalance = 100,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetCheckingAccountAsync(It.IsAny<int>())).ReturnsAsync(account);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                UnitOfWorkResults<CheckingAccount> uowResults = await uow.GetCheckingAccountAsync(account.id);
                Assert.IsNotNull(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
            }

            // Not Found test
            repositoryMock.Setup(r => r.GetCheckingAccountAsync(It.IsAny<int>())).ReturnsAsync((CheckingAccount)null);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountResults uowResults = await uow.GetCheckingAccountAsync(account.id);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsTrue(uowResults.Message == "No matching Checking Account found");
            }

            // Excaption Handling test
            Exception ex = new Exception("dummy exception");
            repositoryMock.Setup(r => r.GetCheckingAccountAsync(It.IsAny<int>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountResults uowResults = await uow.GetCheckingAccountAsync(account.id);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsNotNull(uowResults.WorkException);
            }
        }

        [TestMethod]
        public async Task AddCheckingAccountTests()
        {
            // Set up test data
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111111",
                accountNumber = "222222",
                bankName = "National Bank of Nowhere",
                accountNickname = "NBN",
                currentBalance = 100,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            // Happy Path test
            repositoryMock.Setup(r => r.AddCheckingAccountAsync(It.IsAny<CheckingAccount>())).ReturnsAsync(account);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountResults uowResults = await uow.AddCheckingAccountAsync(account);
                Assert.IsNotNull(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
            }

            // Duplicate Primary Key test
            Exception exDupePrimaryKey = new Exception("A Checking Account already exists with the same Primary Key value");
            repositoryMock.Setup(r => r.AddCheckingAccountAsync(It.IsAny<CheckingAccount>())).ThrowsAsync(exDupePrimaryKey);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountResults _results = await uow.AddCheckingAccountAsync(account);
                Assert.IsNull(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("A Checking Account already exists with the same Primary Key value"));
            }

            // NULL argument test
            Exception exNULLArgument = new Exception("Checking Account cannot be NULL");
            repositoryMock.Setup(r => r.AddCheckingAccountAsync(It.IsAny<CheckingAccount>())).ThrowsAsync(exNULLArgument);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountResults _results = await uow.AddCheckingAccountAsync(null);
                Assert.IsNull(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("Checking Account cannot be NULL"));
            }

            // Excaption Handling test
            Exception ex = new Exception("dummy exception");
            repositoryMock.Setup(r => r.AddCheckingAccountAsync(It.IsAny<CheckingAccount>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountResults uowResults = await uow.AddCheckingAccountAsync(account);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsNotNull(uowResults.WorkException);
            }
        }

        [TestMethod]
        public async Task UpdateCheckingAccountTests()
        {
            // Set up test data
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111111",
                accountNumber = "222222",
                bankName = "National Bank of Nowhere",
                accountNickname = "NBN",
                currentBalance = 100,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            // Happy Path test
            repositoryMock.Setup(r => r.UpdateCheckingAccountAsync(It.IsAny<CheckingAccount>())).Returns(Task.CompletedTask);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountResults uowResults = await uow.UpdateCheckingAccountAsync(account);
                Assert.IsNotNull(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
            }

            // NULL argument test
            Exception exNULLArgument = new Exception("Checking Account cannot be NULL");
            repositoryMock.Setup(r => r.UpdateCheckingAccountAsync(It.IsAny<CheckingAccount>())).ThrowsAsync(exNULLArgument);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountResults _results = await uow.UpdateCheckingAccountAsync(null);
                Assert.IsNull(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("Checking Account cannot be NULL"));
            }

            // Excaption Handling test
            Exception ex = new Exception("dummy exception");
            repositoryMock.Setup(r => r.UpdateCheckingAccountAsync(It.IsAny<CheckingAccount>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountResults uowResults = await uow.UpdateCheckingAccountAsync(account);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsNotNull(uowResults.WorkException);
            }
        }

        [TestMethod]
        public async Task DeleteCheckingAccountTests()
        {
            // Set up test data
            CheckingAccount account = new CheckingAccount()
            {
                routingNumber = "111111111",
                accountNumber = "222222",
                bankName = "National Bank of Nowhere",
                accountNickname = "NBN",
                currentBalance = 100,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            // Happy Path test
            repositoryMock.Setup(r => r.DeleteCheckingAccountAsync(It.IsAny<CheckingAccount>())).Returns(Task.CompletedTask);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteCheckingAccountResults uowResults = await uow.DeleteCheckingAccountAsync(account);
                Assert.IsTrue(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
            }

            // NULL argument test
            Exception exNULLArgument = new Exception("Checking Account cannot be NULL");
            repositoryMock.Setup(r => r.DeleteCheckingAccountAsync(It.IsAny<CheckingAccount>())).ThrowsAsync(exNULLArgument);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteCheckingAccountResults _results = await uow.DeleteCheckingAccountAsync(null);
                Assert.IsFalse(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("Checking Account cannot be NULL"));
            }

            // Excaption Handling test
            Exception ex = new Exception("dummy exception");
            repositoryMock.Setup(r => r.DeleteCheckingAccountAsync(It.IsAny<CheckingAccount>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteCheckingAccountResults uowResults = await uow.DeleteCheckingAccountAsync(account);
                Assert.IsFalse(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsNotNull(uowResults.WorkException);
            }
        }

        [TestMethod]
        public async Task GetSavingsAccountTests()
        {
            // Set up test data
            SavingsAccount account = new SavingsAccount()
            {
                routingNumber = "111111111",
                accountNumber = "222222",
                bankName = "National Bank of Nowhere",
                accountNickname = "NBN",
                currentBalance = 100,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetSavingsAccountAsync(It.IsAny<int>())).ReturnsAsync(account);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountResults uowResults = await uow.GetSavingsAccountAsync(account.id);
                Assert.IsNotNull(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
            }

            // Not Found test
            repositoryMock.Setup(r => r.GetSavingsAccountAsync(It.IsAny<int>())).ReturnsAsync((SavingsAccount)null);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountResults uowResults = await uow.GetSavingsAccountAsync(account.id);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsTrue(uowResults.Message == "No matching Savings Account found");
            }

            // Excaption Handling test
            Exception ex = new Exception("dummy exception");
            repositoryMock.Setup(r => r.GetSavingsAccountAsync(It.IsAny<int>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountResults uowResults = await uow.GetSavingsAccountAsync(account.id);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsNotNull(uowResults.WorkException);
            }
        }

        [TestMethod]
        public async Task AddSavingsAccountTests()
        {
            // Set up test data
            SavingsAccount  account = new SavingsAccount()
            {
                routingNumber = "111111111",
                accountNumber = "222222",
                bankName = "National Bank of Nowhere",
                accountNickname = "NBN",
                currentBalance = 100,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            // Happy Path test
            repositoryMock.Setup(r => r.AddSavingsAccountAsync(It.IsAny<SavingsAccount>())).ReturnsAsync(account);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountResults uowResults = await uow.AddSavingsAccountAsync(account);
                Assert.IsNotNull(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
            }

            // Duplicate Primary Key test
            Exception exDupePrimaryKey = new Exception("A Savings Account already exists with the same Primary Key value");
            repositoryMock.Setup(r => r.AddSavingsAccountAsync(It.IsAny<SavingsAccount>())).ThrowsAsync(exDupePrimaryKey);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountResults _results = await uow.AddSavingsAccountAsync(account);
                Assert.IsNull(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("A Savings Account already exists with the same Primary Key value"));
            }

            // NULL argument test
            Exception exNULLArgument = new Exception("Savings Account cannot be NULL");
            repositoryMock.Setup(r => r.AddSavingsAccountAsync(It.IsAny<SavingsAccount>())).ThrowsAsync(exNULLArgument);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountResults _results = await uow.AddSavingsAccountAsync(null);
                Assert.IsNull(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("Savings Account cannot be NULL"));
            }

            // Excaption Handling test
            Exception ex = new Exception("dummy exception");
            repositoryMock.Setup(r => r.AddSavingsAccountAsync(It.IsAny<SavingsAccount>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountResults uowResults = await uow.AddSavingsAccountAsync(account);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsNotNull(uowResults.WorkException);
            }
        }

        [TestMethod]
        public async Task UpdateSavingsAccountTests()
        {
            // Set up test data
            SavingsAccount account = new SavingsAccount()
            {
                routingNumber = "111111111",
                accountNumber = "222222",
                bankName = "National Bank of Nowhere",
                accountNickname = "NBN",
                currentBalance = 100,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            // Happy Path test
            repositoryMock.Setup(r => r.UpdateSavingsAccountAsync(It.IsAny<SavingsAccount>())).Returns(Task.CompletedTask);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountResults uowResults = await uow.UpdateSavingsAccountAsync(account);
                Assert.IsNotNull(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
            }

            // NULL argument test
            Exception exNULLArgument = new Exception("Savings Account cannot be NULL");
            repositoryMock.Setup(r => r.UpdateSavingsAccountAsync(It.IsAny<SavingsAccount>())).ThrowsAsync(exNULLArgument);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountResults _results = await uow.UpdateSavingsAccountAsync(null);
                Assert.IsNull(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("Savings Account cannot be NULL"));
            }

            // Excaption Handling test
            Exception ex = new Exception("dummy exception");
            repositoryMock.Setup(r => r.UpdateSavingsAccountAsync(It.IsAny<SavingsAccount>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountResults uowResults = await uow.UpdateSavingsAccountAsync(account);
                Assert.IsNull(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsNotNull(uowResults.WorkException);
            }
        }

        [TestMethod]
        public async Task DeleteSavingsAccountTests()
        {
            // Set up test data
            SavingsAccount account = new SavingsAccount()
            {
                routingNumber = "111111111",
                accountNumber = "222222",
                bankName = "National Bank of Nowhere",
                accountNickname = "NBN",
                currentBalance = 100,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            // Happy Path test
            repositoryMock.Setup(r => r.DeleteSavingsAccountAsync(It.IsAny<SavingsAccount>())).Returns(Task.CompletedTask);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteSavingsAccountResults uowResults = await uow.DeleteSavingsAccountAsync(account);
                Assert.IsTrue(uowResults.Results);
                Assert.IsTrue(uowResults.Successful);
            }

            // Duplicate Primary Key test
            Exception exDupePrimaryKey = new Exception("A Savings Account already exists with the same Primary Key value");
            repositoryMock.Setup(r => r.DeleteSavingsAccountAsync(It.IsAny<SavingsAccount>())).ThrowsAsync(exDupePrimaryKey);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteSavingsAccountResults _results = await uow.DeleteSavingsAccountAsync(account);
                Assert.IsFalse(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("A Savings Account already exists with the same Primary Key value"));
            }

            // NULL argument test
            Exception exNULLArgument = new Exception("Savings Account cannot be NULL");
            repositoryMock.Setup(r => r.DeleteSavingsAccountAsync(It.IsAny<SavingsAccount>())).ThrowsAsync(exNULLArgument);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteSavingsAccountResults _results = await uow.DeleteSavingsAccountAsync(null);
                Assert.IsFalse(_results.Results);
                Assert.IsNotNull(_results.WorkException);
                Assert.IsFalse(_results.Successful);
                Assert.IsTrue(_results.WorkException.Message.Contains("Savings Account cannot be NULL"));
            }

            // Excaption Handling test
            Exception ex = new Exception("dummy exception");
            repositoryMock.Setup(r => r.DeleteSavingsAccountAsync(It.IsAny<SavingsAccount>())).ThrowsAsync(ex);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                DeleteSavingsAccountResults uowResults = await uow.DeleteSavingsAccountAsync(account);
                Assert.IsFalse(uowResults.Results);
                Assert.IsFalse(uowResults.Successful);
                Assert.IsNotNull(uowResults.WorkException);
            }
        }

        [TestMethod]
        public async Task SpendCheckingTests()
        {
            // Set up test data
            CheckingAccount account = new CheckingAccount()
            {
                id = 101,
                accountNumber = "111111",
                routingNumber = "222222",
                currentBalance = 1000
            };

            CheckingWithdrawal withdrawal = new CheckingWithdrawal()
            {
                checkingAccount = account,
                checkingAccountId = account.id,
                memo = "Test Withdrawal",
                payToTheOrderOf = "Test Payee",
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetCheckingAccountAsync(It.IsAny<int>())).ReturnsAsync(account);
            decimal expectedBalance = account.currentBalance - withdrawal.transactionAmount;
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountWithdrawalResults _results = await uow.SpendMoneyCheckingAsync(withdrawal);

                Assert.IsNotNull(_results);
                Assert.AreEqual(_results.EndingAccountBalance, expectedBalance);
                Assert.AreEqual(_results.TransactionAmount, withdrawal.transactionAmount);
                Assert.AreEqual(_results.AccountId, account.id);
            }

            // checkingAccountId == 0 test
            withdrawal.checkingAccount = null;
            withdrawal.checkingAccountId = 0;
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountWithdrawalResults _results = await uow.SpendMoneyCheckingAsync(withdrawal);

                Assert.IsNotNull(_results.WorkException);
                Assert.AreEqual(_results.WorkException.Message, "The Checking Account ID is required with all transactions.");
            }

            // Checking Account NOT FOUND test
            withdrawal.checkingAccountId = 10;
            repositoryMock.Setup(r => r.GetCheckingAccountAsync(It.IsAny<int>())).ReturnsAsync((CheckingAccount)null);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountWithdrawalResults _results = await uow.SpendMoneyCheckingAsync(withdrawal);

                Assert.IsNotNull(_results.WorkException);
                Assert.AreEqual(_results.WorkException.Message, "Unable to locate the Checking Account associated with this record.");
            }

        }

        [TestMethod]
        public async Task DepositCheckingTests()
        {
            // Set up test data
            CheckingAccount account = new CheckingAccount()
            {
                id = 102,
                routingNumber = "111111111",
                accountNumber = "222222",
                bankName = "National Bank of Nowhere",
                accountNickname = "NBN",
                currentBalance = 100,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            CheckingDeposit deposit = new CheckingDeposit()
            {
                checkingAccount = account,
                checkingAccountId = account.id,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetCheckingAccountAsync(It.IsAny<int>())).ReturnsAsync(account);
            decimal expectedBalance = account.currentBalance + deposit.transactionAmount;
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountDepositResults _results = await uow.DepositMoneyCheckingAsync(deposit);

                Assert.IsNotNull(_results);
                Assert.AreEqual(_results.EndingAccountBalance, expectedBalance);
                Assert.AreEqual(_results.TransactionAmount, deposit.transactionAmount);
                Assert.AreEqual(_results.AccountId, account.id);
            }

            // checkingAccountId == 0 test
            deposit.checkingAccount = null;
            deposit.checkingAccountId = 0;
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountDepositResults _results = await uow.DepositMoneyCheckingAsync(deposit);

                Assert.IsNotNull(_results.WorkException);
                Assert.AreEqual(_results.WorkException.Message, "The Checking Account ID is required with all transactions.");
            }

            // Checking Account NOT FOUND test
            deposit.checkingAccountId = 101;
            repositoryMock.Setup(r => r.GetCheckingAccountAsync(It.IsAny<int>())).ReturnsAsync((CheckingAccount)null);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                CheckingAccountDepositResults _results = await uow.DepositMoneyCheckingAsync(deposit);

                Assert.IsNotNull(_results.WorkException);
                Assert.AreEqual(_results.WorkException.Message, "Unable to locate the Checking Account associated with this record.");
            }

        }

        [TestMethod]
        public async Task SpendSavingsTests()
        {
            // Set up test data
            SavingsAccount account = new SavingsAccount()
            {
                id = 101,
                routingNumber = "111111111",
                accountNumber = "222222",
                bankName = "National Bank of Nowhere",
                accountNickname = "NBN",
                currentBalance = 100,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            SavingsWithdrawal withdrawal = new SavingsWithdrawal()
            {
                savingsAccount = account,
                savingsAccountId = account.id,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetSavingsAccountAsync(It.IsAny<int>())).ReturnsAsync(account);
            decimal expectedBalance = account.currentBalance - withdrawal.transactionAmount;
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountWithdrawalResults _results = await uow.SpendMoneySavingsAsync(withdrawal);

                Assert.IsNotNull(_results);
                Assert.AreEqual(_results.EndingAccountBalance, expectedBalance);
                Assert.AreEqual(_results.TransactionAmount, withdrawal.transactionAmount);
                Assert.AreEqual(_results.AccountId, account.id);
            }

            // savingsAccountId == 0 test
            withdrawal.savingsAccount = null;
            withdrawal.savingsAccountId = 0;
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountWithdrawalResults _results = await uow.SpendMoneySavingsAsync(withdrawal);

                Assert.IsNotNull(_results.WorkException);
                Assert.AreEqual(_results.WorkException.Message, "The Savings Account ID is required with all transactions.");
            }

            // Savings Account NOT FOUND test
            withdrawal.savingsAccountId = 10;
            repositoryMock.Setup(r => r.GetSavingsAccountAsync(It.IsAny<int>())).ReturnsAsync((SavingsAccount)null);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountWithdrawalResults _results = await uow.SpendMoneySavingsAsync(withdrawal);

                Assert.IsNotNull(_results.WorkException);
                Assert.AreEqual(_results.WorkException.Message, "Unable to locate the Savings Account associated with this record.");
            }
        }

        [TestMethod]
        public async Task DepositSavingsTests()
        {
            // Set up test data
            SavingsAccount account = new SavingsAccount()
            {
                id = 101,
                routingNumber = "111111111",
                accountNumber = "222222",
                bankName = "National Bank of Nowhere",
                accountNickname = "NBN",
                currentBalance = 100,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            SavingsDeposit deposit = new SavingsDeposit()
            {
                savingsAccount = account,
                savingsAccountId = account.id,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                transactionDate = DateTime.Now,
                transactionAmount = 100
            };

            // Happy Path test
            repositoryMock.Setup(r => r.GetSavingsAccountAsync(It.IsAny<int>())).ReturnsAsync(account);
            decimal expectedBalance = account.currentBalance + deposit.transactionAmount;
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountDepositResults _results = await uow.DepositMoneySavingsAsync(deposit);

                Assert.IsNotNull(_results);
                Assert.AreEqual(_results.EndingAccountBalance, expectedBalance);
                Assert.AreEqual(_results.TransactionAmount, deposit.transactionAmount);
                Assert.AreEqual(_results.AccountId, account.id);
            }

            // savingsAccountId == 0 test
            deposit.savingsAccount = null;
            deposit.savingsAccountId = 0;
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountDepositResults _results = await uow.DepositMoneySavingsAsync(deposit);

                Assert.IsNotNull(_results.WorkException);
                Assert.AreEqual(_results.WorkException.Message, "The Savings Account ID is required with all transactions.");
            }

            // Savings Account NOT FOUND test
            deposit.savingsAccountId = 10;
            repositoryMock.Setup(r => r.GetSavingsAccountAsync(It.IsAny<int>())).ReturnsAsync((SavingsAccount)null);
            using (UnitOfWork uow = new UnitOfWork(repositoryMock.Object))
            {
                SavingsAccountDepositResults _results = await uow.DepositMoneySavingsAsync(deposit);

                Assert.IsNotNull(_results.WorkException);
                Assert.AreEqual(_results.WorkException.Message, "Unable to locate the Savings Account associated with this record.");
            }
        }

        [TestMethod]
        [Ignore]
        public void TransferMoneyTests()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        [Ignore]
        public void VoidFundsTransferTests()
        {
            Assert.IsTrue(false);
        }
    }
}
