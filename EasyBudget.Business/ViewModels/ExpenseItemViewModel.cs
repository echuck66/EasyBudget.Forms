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
using System.Threading.Tasks;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class ExpenseItemViewModel : BaseViewModel
    {

        ExpenseItem ExpenseItem;

        public decimal BudgetedAmount { get; set; }

        public string Description { get; set; }

        public string Notation { get; set; }

        public bool Recurring { get; set; }

        public ExpenseItemViewModel(string dbFilePath)
            : base(dbFilePath)
        {

        }

        internal async Task LoadExpenseItemAsync(int itemId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetExpenseItemAsync(itemId);
                if (_results.Successful)
                {

                    this.ExpenseItem = _results.Results;
                    this.ExpenseItem.ItemType = BudgetItemType.Expense;
                    this.BudgetedAmount = this.ExpenseItem?.BudgetedAmount ?? 0;
                    this.Description = this.ExpenseItem?.description ?? string.Empty;
                    this.Notation = this.ExpenseItem?.notation ?? string.Empty;
                    this.Recurring = this.ExpenseItem?.recurring ?? false;

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
                        WriteErrorCondition("An unknown error has occurred loading Expense Item");
                    }
                }
            }
        }

        internal void CrateExpenseItem(int categoryId)
        {
            this.ExpenseItem = new ExpenseItem();
            this.ExpenseItem.budgetCategoryId = categoryId;
            this.ExpenseItem.IsNew = true;
        }
    }

}
