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
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace EasyBudget.Models.DataModels
{
    [SQLite.Table("CheckingDeposit")]
    public class CheckingDeposit : BaseObject
    {
        public int checkingAccountId { get; set; }

        [SQLite.Ignore]
        public virtual CheckingAccount checkingAccount { get; set; }

        public DateTime transactionDate { get; set; }

        public decimal transactionAmount { get; set; }

        [MaxLength(250)]
        public string description { get; set; }

        [MaxLength(250)]
        public string notation { get; set; }

        public int? budgetIncomeId { get; set; }

        [SQLite.Ignore]
        public virtual IncomeItem budgetIncome { get; set; }

        public bool reconciled { get; set; }

        public CheckingDeposit()
        {
        }
    }
}
