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
using System.ComponentModel;
using System.Threading.Tasks;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class SavingsWithdrawalViewModel : WithdrawalViewModel, INotifyPropertyChanged
    {
        SavingsWithdrawal model { get; set; }

        public DateTime transactionDate
        {
            get
            {
                return model.transactionDate;
            }
            set
            {
                if (model.transactionDate != value)
                {
                    model.transactionDate = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(transactionDate)));
                }
            }
        }

        public decimal transactionAmount
        {
            get
            {
                return model.transactionAmount;
            }
            set
            {
                if (model.transactionAmount != value)
                {
                    model.transactionAmount = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(transactionAmount)));
                }
            }
        }

        public string notation
        {
            get
            {
                return model.notation;
            }
            set
            {
                if (model.notation != value)
                {
                    model.notation = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(notation)));
                }
            }
        }


        public string description
        {
            get
            {
                return model.description;
            }
            set
            {
                if (model.description != value)
                {
                    model.description = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(description)));
                }
            }
        }

        public int? budgetExpenseId
        {
            get
            {
                return model.budgetExpenseId;
            }
            set
            {
                if (model.budgetExpenseId != value)
                {
                    model.budgetExpenseId = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(budgetExpenseId)));
                }
            }
        }

        public bool reconciled
        {
            get
            {
                return model.reconciled;
            }
            set
            {
                if (model.reconciled != value)
                {
                    model.reconciled = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(reconciled)));
                }
            }
        }

        public ICollection<BudgetCategory> BudgetCategories { get; set; }

        internal SavingsWithdrawalViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.BudgetCategories = new List<BudgetCategory>();
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public void PopulateVM(SavingsWithdrawal withdrawal)
        {
            this.model = withdrawal;
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = Task.Run(() => uow.GetAllBudgetCategoriesAsync()).Result;
                if (_results.Successful)
                {
                    foreach (BudgetCategory category in _results.Results)
                    {
                        if (category.categoryType == Models.BudgetCategoryType.Expense)
                        {
                            this.BudgetCategories.Add(category);
                        }
                    }
                }
            }
        }

        public async Task PopulateVMAsync(SavingsWithdrawal withdrawal)
        {
            this.model = withdrawal;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllBudgetCategoriesAsync();
                if (_results.Successful)
                {
                    foreach (BudgetCategory category in _results.Results)
                    {
                        if (category.categoryType == Models.BudgetCategoryType.Expense)
                        {
                            this.BudgetCategories.Add(category);
                        }
                    }
                }
            }
        }

        public async override Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }

}
