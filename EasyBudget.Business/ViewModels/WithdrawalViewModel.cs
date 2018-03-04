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
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public abstract class WithdrawalViewModel : AccountRegisterItemViewModel
    {
        internal BankAccount accountModel { get; set; }

        public string BankName
        {
            get
            {
                return accountModel.bankName;
            }
        }

        public string RoutingNumber
        {
            get
            {
                return accountModel.routingNumber;
            }
        }

        public string AccountNumber
        {
            get
            {
                return accountModel.accountNumber;
            }
        }

        public int accountId
        {
            get
            {
                return accountModel.id;
            }
        }

        public BankAccountType accountType
        {
            get
            {
                return accountModel.accountType;
            }
        }

        public ObservableCollection<BudgetCategory> BudgetCategories { get; set; }

        public ObservableCollection<BudgetItem> BudgetItems { get; set; }

        internal WithdrawalViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.BudgetItems = new ObservableCollection<BudgetItem>();
            this.BudgetCategories = new ObservableCollection<BudgetCategory>();
        }

        public abstract Task<bool> SaveChangesAsync();

        public abstract Task<bool> DeleteAsync();
    }
}
