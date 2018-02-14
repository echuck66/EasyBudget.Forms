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
using EasyBudget.Models;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class BankAccountViewModel : BaseViewModel
    {
        BankAccount Account { get; set; }

        public string BankName { get; set; }

        public BankAccountType AccountType { get; set; }

        public decimal CurrentBalance { get; set; }

        public string RoutingNumber { get; set; }

        public string AccountNumber { get; set; }

        public string Nickname { get; set; }

        public ICollection<DepositViewModel> Deposits { get; set; }

        public ICollection<WithdrawalViewModel> Withdrawals { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool IsNew { get; set; }

        internal BankAccountViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.Deposits = new List<DepositViewModel>();
            this.Withdrawals = new List<WithdrawalViewModel>();
        }

        internal async Task PopulateVMAsync(int id, BankAccountType accountType)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                switch (accountType)
                {
                    case BankAccountType.Checking:
                        var _resultsChecking = await uow.GetCheckingAccountAsync(id);
                        if (_resultsChecking.Successful)
                        {
                            this.Account = _resultsChecking.Results;
                        }
                        else
                        {
                            if (_resultsChecking.WorkException != null)
                            {
                                WriteErrorCondition(_resultsChecking.WorkException);
                            }
                            else if (!string.IsNullOrEmpty(_resultsChecking.Message))
                            {
                                WriteErrorCondition(_resultsChecking.Message);
                            }
                            else
                            {
                                WriteErrorCondition("An unknown error has occurred");
                            }
                        }
                        break;
                    case BankAccountType.Savings:
                        var _resultsSavings = await uow.GetSavingsAccountAsync(id);
                        if (_resultsSavings.Successful)
                        {
                            this.Account = _resultsSavings.Results;
                        }
                        else
                        {
                            if (_resultsSavings.WorkException != null)
                            {
                                WriteErrorCondition(_resultsSavings.WorkException);
                            }
                            else if (!string.IsNullOrEmpty(_resultsSavings.Message))
                            {
                                WriteErrorCondition(_resultsSavings.Message);
                            }
                            else
                            {
                                WriteErrorCondition("An unknown error has occurred");
                            }
                        }
                        break;
                }
            }

            if (this.Account != null)
            {
                this.BankName = this.Account.bankName;
                this.AccountNumber = this.Account.accountNumber;
                this.RoutingNumber = this.Account.routingNumber;
                this.AccountType = this.Account.accountType;
                this.CurrentBalance = this.Account.currentBalance;

            }
        }
    
    }

}
