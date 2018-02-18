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
        BankAccount model { get; set; }

        public string BankName 
        {
            get
            {
                return model.bankName;
            }
            set
            {
                if (model.bankName != value)
                {
                    model.bankName = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BankName)));
                }
            }
        }

        public BankAccountType AccountType 
        {
            get
            {
                return model.accountType;
            }
            set
            {
                if (model.accountType != value)
                {
                    model.accountType = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountType)));
                }
            }
        }

        public decimal CurrentBalance 
        {
            get
            {
                return model.currentBalance;
            }
            set
            {
                if (model.currentBalance != value)
                {
                    model.currentBalance = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentBalance)));
                }
            }
        }

        public string RoutingNumber 
        {
            get 
            {
                return model.routingNumber;
            }
            set
            {
                if (model.routingNumber != value)
                {
                    model.routingNumber = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RoutingNumber)));
                }
            }
        }

        public string AccountNumber 
        {
            get
            {
                return model.accountNumber;
            }
            set
            {
                if (model.accountNumber != value)
                {
                    model.accountNumber = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountNumber)));
                }
            }
        }

        public string Nickname 
        {
            get
            {
                return model.accountNickname;
            }
            set
            {
                if (model.accountNickname != value)
                {
                    model.accountNickname = value;
                    this.IsDirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nickname)));
                }
            }
        }

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
            await Task.Run(() => this.model = account);


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
