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
using System.ComponentModel.DataAnnotations;

namespace EasyBudget.Models.DataModels
{
    public abstract class BudgetItem : BaseObject
    {
        public int budgetCategoryId { get; set; }

        [SQLite.Ignore]
        public virtual BudgetCategory budgetCategory { get; set; }

        public BudgetItemType ItemType { get; set; }

        public decimal BudgetedAmount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(250)]
        public string description { get; set; }

        [MaxLength(250)]
        public string notation { get; set; }

        public bool recurring { get; set; }

        public Frequency frequency { get; set; }

        public BudgetItem()
        {
        }
    }
}
