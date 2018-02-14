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

    public class SavingsAccountViewModel : BaseViewModel
    {
        SavingsAccount SavingsAccount { get; set; }

        public string RoutingNumber { get; set; }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public string AccountNickname { get; set; }

        public decimal CurrentBalance { get; set; }

        public ICollection<SavingsWithdrawal> Withdrawals { get; set; }

        public ICollection<SavingsDeposit> Deposits { get; set; }

        public SavingsAccountViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.Withdrawals = new List<SavingsWithdrawal>();
            this.Deposits = new List<SavingsDeposit>();
        }

        internal async Task LoadSavingsAccountDetailsAsync(int accountId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetSavingsAccountAsync(accountId);
                if (_results.Successful)
                {
                    this.SavingsAccount = _results.Results;
                    this.RoutingNumber = this.SavingsAccount?.routingNumber ?? string.Empty;
                    this.AccountNumber = this.SavingsAccount?.accountNumber ?? string.Empty;
                    this.BankName = this.SavingsAccount?.bankName ?? string.Empty;
                    this.AccountNickname = this.SavingsAccount?.accountNickname ?? string.Empty;
                    this.CurrentBalance = this.SavingsAccount?.currentBalance ?? 0;
                    if (this.SavingsAccount.withdrawals != null)
                    {
                        foreach (SavingsWithdrawal item in this.SavingsAccount.withdrawals)
                        {
                            this.Withdrawals.Add(item);
                        }
                    }
                    if (this.SavingsAccount.deposits != null)
                    {
                        foreach (SavingsDeposit item in this.SavingsAccount.deposits)
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
                        WriteErrorCondition("An unknown error has occurred loading Savings Account");
                    }
                }
            }
        }

        internal void CreateSavingsAccount()
        {
            this.SavingsAccount = new SavingsAccount();
            this.SavingsAccount.IsNew = true;
        }

        public async Task SaveChangesAsync()
        {
            bool _saveOk = true;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (this.SavingsAccount.IsNew)
                {
                    var _resultsAddAccont = await uow.AddSavingsAccountAsync(this.SavingsAccount);
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
                    var _resultsUpdateAccont = await uow.UpdateSavingsAccountAsync(this.SavingsAccount);
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
                    foreach (SavingsDeposit deposit in this.Deposits)
                    {
                        if (deposit.IsNew)
                        {
                            var _resultsAddDeposit = await uow.AddSavingsDepositAsync(deposit);
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
                            var _resultsUpdateDeposit = await uow.UpdateSavingsDepositAsync(deposit);
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
                    foreach (SavingsWithdrawal withdrawal in this.Withdrawals)
                    {
                        if (withdrawal.IsNew)
                        {
                            var _resultsAddWithdrawal = await uow.AddSavingsWithdrawalAsync(withdrawal);
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
                            var _resultsUpdateWithdrawal = await uow.UpdateSavingsWithdrawalAsync(withdrawal);
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

        public SavingsDeposit AddDeposit()
        {
            var deposit = new SavingsDeposit();
            deposit.savingsAccountId = this.SavingsAccount.id;
            deposit.IsNew = true;

            this.Deposits.Add(deposit);

            return deposit;
        }

        public SavingsWithdrawal AddWithdrawal()
        {
            var withdrawal = new SavingsWithdrawal();
            withdrawal.savingsAccountId = this.SavingsAccount.id;
            withdrawal.IsNew = true;

            this.Withdrawals.Add(withdrawal);

            return withdrawal;
        }
    }

}
