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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class BankAccountViewModel : BaseViewModel, INotifyPropertyChanged
    {
        BankAccount Account { get; set; }

        public string BankName 
        {
            get
            {
                return Account.bankName;
            }
            set
            {
                if (Account.bankName != value)
                {
                    Account.bankName = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BankName)));
                }
            }
        }

        public BankAccountType AccountType 
        {
            get
            {
                return Account.accountType;
            }
            set
            {
                if (Account.accountType != value)
                {
                    Account.accountType = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountType)));
                }
            }
        }

        public decimal CurrentBalance 
        {
            get
            {
                return Account.currentBalance;
            }
            set
            {
                if (Account.currentBalance != value)
                {
                    Account.currentBalance = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentBalance)));
                }
            }
        }

        public string RoutingNumber 
        {
            get 
            {
                return Account.routingNumber;
            }
            set
            {
                if (Account.routingNumber != value)
                {
                    Account.routingNumber = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RoutingNumber)));
                }
            }
        }

        public string AccountNumber 
        {
            get
            {
                return Account.accountNumber;
            }
            set
            {
                if (Account.accountNumber != value)
                {
                    Account.accountNumber = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountNumber)));
                }
            }
        }

        public string Nickname 
        {
            get
            {
                return Account.accountNickname;
            }
            set
            {
                if (Account.accountNickname != value)
                {
                    Account.accountNickname = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nickname)));
                }
            }
        }

        public bool IsDirty { get; set; }

        public ObservableCollection<DepositViewModel> Deposits { get; set; }

        public ObservableCollection<WithdrawalViewModel> Withdrawals { get; set; }

        internal BankAccountViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.Deposits = new ObservableCollection<DepositViewModel>();
            this.Withdrawals = new ObservableCollection<WithdrawalViewModel>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal async Task PopulateVMAsync(BankAccount account)
        {
            await Task.Run(() => this.Account = account);


            //using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            //{
            //    switch (Account.accountType)
            //    {
            //        case BankAccountType.Checking:
            //            var _resultsChecking = await uow.GetCheckingAccountAsync(id);
            //            if (_resultsChecking.Successful)
            //            {
            //                this.Account = _resultsChecking.Results;
            //            }
            //            else
            //            {
            //                if (_resultsChecking.WorkException != null)
            //                {
            //                    WriteErrorCondition(_resultsChecking.WorkException);
            //                }
            //                else if (!string.IsNullOrEmpty(_resultsChecking.Message))
            //                {
            //                    WriteErrorCondition(_resultsChecking.Message);
            //                }
            //                else
            //                {
            //                    WriteErrorCondition("An unknown error has occurred");
            //                }
            //            }
            //            break;
            //        case BankAccountType.Savings:
            //            var _resultsSavings = await uow.GetSavingsAccountAsync(id);
            //            if (_resultsSavings.Successful)
            //            {
            //                this.Account = _resultsSavings.Results;
            //            }
            //            else
            //            {
            //                if (_resultsSavings.WorkException != null)
            //                {
            //                    WriteErrorCondition(_resultsSavings.WorkException);
            //                }
            //                else if (!string.IsNullOrEmpty(_resultsSavings.Message))
            //                {
            //                    WriteErrorCondition(_resultsSavings.Message);
            //                }
            //                else
            //                {
            //                    WriteErrorCondition("An unknown error has occurred");
            //                }
            //            }
            //            break;
            //    }
            //}
        }
    }
}
