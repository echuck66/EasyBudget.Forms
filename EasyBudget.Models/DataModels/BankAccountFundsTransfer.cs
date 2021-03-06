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
using SQLite;

namespace EasyBudget.Models.DataModels
{
    [Table("BankAccountTransfer")]
    public class BankAccountFundsTransfer : BaseObject
    {
        [Ignore]
        public BankAccount sourceAccount { get; set; }

        public int sourceAccountId { get; set; }

        public BankAccountType sourceAccountType { get; set; }

        [Ignore]
        public BankAccount destinationAccount { get; set; }

        public int destinationAccountId { get; set; }

        public BankAccountType destinationAccountType { get; set; }

        public decimal transactionAmount { get; set; }

        public DateTime transactionDate { get; set; }

        public decimal sourceAccountBeginningBalance { get; set; }

        public decimal sourceAccountEndingBalance { get; set; }

        public decimal destinationAccountBeginningBalance { get; set; }

        public decimal destinationAccountEndingBalance { get; set; }

        public bool voided { get; set; }

        public BankAccountFundsTransfer()
        {
        }
    }
}
