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
using EasyBudget.Models;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class DepositViewModel : BaseViewModel
    {
        public int accountId { get; set; }

        public BankAccountType accountType { get; set; }

        public DateTime transactionDate { get; set; }

        public decimal transactionAmount { get; set; }

        public string description { get; set; }

        public string notation { get; set; }

        public int budgetItemId { get; set; }

        public bool reconciled { get; set; }

        internal DepositViewModel(string dbFilePath)
            : base(dbFilePath)
        {

        }

        internal void PopulateVM(CheckingDeposit deposit)
        {
            this.accountId = deposit.checkingAccountId;
            this.transactionDate = deposit.transactionDate;
            this.transactionAmount = deposit.transactionAmount;
            this.description = deposit.description;
            this.notation = deposit.notation;
            this.budgetItemId = deposit.budgetIncomeId != null ? (int)deposit.budgetIncomeId : 0;
            this.reconciled = deposit.reconciled;
            this.accountType = BankAccountType.Checking;
        }

        internal void PopulateVM(SavingsDeposit deposit)
        {
            this.accountId = deposit.savingsAccountId;
            this.transactionDate = deposit.transactionDate;
            this.transactionAmount = deposit.transactionAmount;
            this.description = deposit.description;
            this.notation = deposit.notation;
            this.budgetItemId = deposit.budgetIncomeId != null ? (int)deposit.budgetIncomeId : 0;
            this.reconciled = deposit.reconciled;
            this.accountType = BankAccountType.Savings;
        }
    }

}
