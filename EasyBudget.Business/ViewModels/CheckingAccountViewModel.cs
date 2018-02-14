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
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class CheckingAccountViewModel : BaseViewModel
    {
        CheckingAccount CheckingAccount { get; set; }

        public string RoutingNumber { get; set; }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public string AccountNickname { get; set; }

        public decimal CurrentBalance { get; set; }

        public DateTime LoadTransactionsFromDate { get; set; }

        public DateTime LoadTransactionsToDate { get; set; }

        public ICollection<CheckingWithdrawal> Withdrawals { get; set; }

        public ICollection<CheckingDeposit> Deposits { get; set; }

        public CheckingAccountViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.Withdrawals = new List<CheckingWithdrawal>();
            this.Deposits = new List<CheckingDeposit>();
        }

        internal async Task LoadCheckingAccountDetailsAsync(int accountId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetCheckingAccountAsync(accountId);
                if (_results.Successful)
                {
                    this.CheckingAccount = _results.Results;
                    this.RoutingNumber = this.CheckingAccount?.routingNumber ?? string.Empty;
                    this.AccountNumber = this.CheckingAccount?.accountNumber ?? string.Empty;
                    this.BankName = this.CheckingAccount?.bankName ?? string.Empty;
                    this.AccountNickname = this.CheckingAccount?.accountNickname ?? string.Empty;
                    this.CurrentBalance = this.CheckingAccount?.currentBalance ?? 0;
                    if (this.CheckingAccount.withdrawals != null)
                    {
                        foreach (CheckingWithdrawal item in this.CheckingAccount.withdrawals)
                        {
                            this.Withdrawals.Add(item);
                        }
                    }
                    if (this.CheckingAccount.deposits != null)
                    {
                        foreach (CheckingDeposit item in this.CheckingAccount.deposits)
                        {
                            this.Deposits.Add(item);
                        }
                    }
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
                        WriteErrorCondition("An unknown error has occurred loading Checking Account");
                    }
                }
            }
        }

        public async Task SaveChangesAsync()
        {
            bool _saveOk = true;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (this.CheckingAccount.IsNew)
                {
                    var _resultsAddAccont = await uow.AddCheckingAccountAsync(this.CheckingAccount);
                    if (_resultsAddAccont.Successful)
                    {
                        _saveOk = true;
                    }
                    else
                    {
                        _saveOk = false;
                        if (_resultsAddAccont.WorkException != null)
                        {
                            WriteErrorCondition(_resultsAddAccont.WorkException);
                        }
                        else if (!string.IsNullOrEmpty(_resultsAddAccont.Message))
                        {
                            WriteErrorCondition(_resultsAddAccont.Message);
                        }
                        else
                        {
                            WriteErrorCondition("An unknown error has occurred");
                        }
                    }
                }
                else
                {
                    var _resultsUpdateAccont = await uow.UpdateCheckingAccountAsync(this.CheckingAccount);
                    if (_resultsUpdateAccont.Successful)
                    {
                        _saveOk = true;
                    }
                    else
                    {
                        _saveOk = false;
                        if (_resultsUpdateAccont.WorkException != null)
                        {
                            WriteErrorCondition(_resultsUpdateAccont.WorkException);
                        }
                        else if (!string.IsNullOrEmpty(_resultsUpdateAccont.Message))
                        {
                            WriteErrorCondition(_resultsUpdateAccont.Message);
                        }
                        else
                        {
                            WriteErrorCondition("An unknown error has occurred");
                        }
                    }
                }
                if (_saveOk)
                {
                    foreach (CheckingDeposit deposit in this.Deposits)
                    {
                        if (deposit.IsNew)
                        {
                            var _resultsAddDeposit = await uow.AddCheckingDepositAsync(deposit);
                            if (_resultsAddDeposit.Successful)
                            {
                                _saveOk = _saveOk && true;
                            }
                            else
                            {
                                _saveOk = false;
                                if (_resultsAddDeposit.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsAddDeposit.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsAddDeposit.Message))
                                {
                                    WriteErrorCondition(_resultsAddDeposit.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred");
                                }
                            }
                        }
                        else
                        {
                            var _resultsUpdateDeposit = await uow.UpdateCheckingDepositAsync(deposit);
                            if (_resultsUpdateDeposit.Successful)
                            {
                                _saveOk = true;
                            }
                            else
                            {
                                _saveOk = false;
                                if (_resultsUpdateDeposit.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsUpdateDeposit.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsUpdateDeposit.Message))
                                {
                                    WriteErrorCondition(_resultsUpdateDeposit.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred");
                                }
                            }
                        }
                    }
                    foreach (CheckingWithdrawal withdrawal in this.Withdrawals)
                    {
                        if (withdrawal.IsNew)
                        {
                            var _resultsAddWithdrawal = await uow.AddCheckingWithdrawalAsync(withdrawal);
                            if (_resultsAddWithdrawal.Successful)
                            {
                                _saveOk = true;
                            }
                            else
                            {
                                _saveOk = false;
                                if (_resultsAddWithdrawal.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsAddWithdrawal.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsAddWithdrawal.Message))
                                {
                                    WriteErrorCondition(_resultsAddWithdrawal.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred");
                                }
                            }
                        }
                        else
                        {
                            var _resultsUpdateWithdrawal = await uow.UpdateCheckingWithdrawalAsync(withdrawal);
                            if (_resultsUpdateWithdrawal.Successful)
                            {
                                _saveOk = true;
                            }
                            else
                            {
                                _saveOk = false;
                                if (_resultsUpdateWithdrawal.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsUpdateWithdrawal.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsUpdateWithdrawal.Message))
                                {
                                    WriteErrorCondition(_resultsUpdateWithdrawal.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred");
                                }
                            }
                        }
                    }
                }
            }
        }

        internal void CreateCheckingAccount()
        {
            this.CheckingAccount = new CheckingAccount();
            this.CheckingAccount.IsNew = true;
        }

        public CheckingDeposit AddDeposit()
        {
            CheckingDeposit deposit = new CheckingDeposit();
            deposit.checkingAccountId = this.CheckingAccount.id;
            deposit.IsNew = true;

            this.Deposits.Add(deposit);

            return deposit;
        }

        public CheckingWithdrawal AddWithdrawal()
        {
            CheckingWithdrawal withdrawal = new CheckingWithdrawal();
            withdrawal.checkingAccountId = this.CheckingAccount.id;
            withdrawal.IsNew = true;

            this.Withdrawals.Add(withdrawal);

            return withdrawal;
        }
    }

}
