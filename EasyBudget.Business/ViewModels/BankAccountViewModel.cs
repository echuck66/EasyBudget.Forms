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
using System.Linq;
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

        public ObservableCollection<AccountRegisterItemViewModel> AccountRegister { get; set; }
        public ObservableCollection<Grouping<string, AccountRegisterItemViewModel>> AccountRegisteredGrouped { get; set; }

        public void GroupAccountItems()
        {
            var grouped = from regItm in this.AccountRegister
                          orderby regItm.ItemDate descending
                          group regItm by regItm.ItemType into Group
                          select new Grouping<string, AccountRegisterItemViewModel>(Group.Key.ToString(), Group);
            this.AccountRegisteredGrouped = new ObservableCollection<Grouping<string, AccountRegisterItemViewModel>>(grouped.OrderBy(g => g.Key));
        }

        public async Task GroupAccountItemsAsync()
        {
            var grouped = from regItm in this.AccountRegister
                          orderby regItm.ItemDate descending
                          group regItm by regItm.ItemType into Group
                          select new Grouping<string, AccountRegisterItemViewModel>(Group.Key.ToString(), Group);

            this.AccountRegisteredGrouped = await Task.Run(() => new ObservableCollection<Grouping<string, AccountRegisterItemViewModel>>(grouped.OrderBy(g => g.Key)));
        }

        internal BankAccountViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.AccountRegister = new ObservableCollection<AccountRegisterItemViewModel>();
            this.AccountRegisteredGrouped = new ObservableCollection<Grouping<string, AccountRegisterItemViewModel>>();
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        internal async Task PopulateVMAsync(BankAccount account)
        {
            this.model = account;

            await LoadDepositsAsync(false);
            await LoadWithdrawalsAsync(false);

        }

        public async Task AddDepositAsync()
        {
            switch (this.AccountType)
            {
                case BankAccountType.Checking:
                    await AddCheckingDepositAsync();
                    break;
                case BankAccountType.Savings:
                    await AddSavingsDepositAsync();
                    break;
            }
        }

        public async Task AddWithdrawalAsync()
        {
            switch (this.AccountType)
            {
                case BankAccountType.Checking:
                    await AddCheckingWithdrawalAsync();
                    break;
                case BankAccountType.Savings:
                    await AddSavingsWithdrawalAsync();
                    break;
            }
        }

        async Task AddCheckingDepositAsync()
        {
            CheckingDepositViewModel vm = new CheckingDepositViewModel(this.dbFilePath);
            vm.IsNew = true;
            vm.CanEdit = true;
            vm.CanDelete = false;
            vm.ItemType = AccountRegisterItemViewModel.AccountItemType.Deposit;

            CheckingDeposit deposit = new CheckingDeposit();
            deposit.checkingAccount = model as CheckingAccount;
            deposit.checkingAccountId = model.id;

            await vm.PopulateVMAsync(deposit);
            
            this.AccountRegister.Add(vm);

        }

        async Task AddSavingsDepositAsync()
        {
            SavingsDepositViewModel vm = new SavingsDepositViewModel(this.dbFilePath);
            vm.IsNew = true;
            vm.CanEdit = true;
            vm.CanDelete = false;
            vm.ItemType = AccountRegisterItemViewModel.AccountItemType.Deposit;

            SavingsDeposit deposit = new SavingsDeposit();
            deposit.savingsAccount = model as SavingsAccount;
            deposit.savingsAccountId = model.id;

            await vm.PopulateVMAsync(deposit);

            this.AccountRegister.Add(vm);

        }

        async Task AddCheckingWithdrawalAsync()
        {
            CheckingWithdrawalViewModel vm = new CheckingWithdrawalViewModel(this.dbFilePath);
            vm.IsNew = true;
            vm.CanEdit = true;
            vm.CanDelete = false;
            vm.ItemType = AccountRegisterItemViewModel.AccountItemType.Withdrawal;

            CheckingWithdrawal withdrawal = new CheckingWithdrawal();
            withdrawal.checkingAccount = model as CheckingAccount;
            withdrawal.checkingAccountId = model.id;

            await vm.PopulateVMAsync(withdrawal);

            this.AccountRegister.Add(vm);
        }

        async Task AddSavingsWithdrawalAsync()
        {
            SavingsWithdrawalViewModel vm = new SavingsWithdrawalViewModel(this.dbFilePath);
            vm.IsNew = true;
            vm.CanEdit = true;
            vm.CanDelete = false;
            vm.ItemType = AccountRegisterItemViewModel.AccountItemType.Withdrawal;

            SavingsWithdrawal withdrawal = new SavingsWithdrawal();
            withdrawal.savingsAccount = model as SavingsAccount;
            withdrawal.savingsAccountId = model.id;

            await vm.PopulateVMAsync(withdrawal);

            this.AccountRegister.Add(vm);
        }

        internal async Task LoadVMAsync(int accountId, BankAccountType accountType)
        {
            switch (accountType)
            {
                case BankAccountType.Checking:
                    await LoadCheckingAccountAsync(accountId);
                    break;
                case BankAccountType.Savings:
                    await LoadSavingsAccountAsync(accountId);
                    break;
            }
        }

        public async Task LoadDepositsAsync(bool getReconciled = false)
        {
            switch (this.AccountType)
            {
                case BankAccountType.Checking:
                    await LoadCheckingDepositsAsync(getReconciled);
                    break;
                case BankAccountType.Savings:
                    await LoadSavingsDepositsAsync(getReconciled);
                    break;
            }
        }

        public async Task LoadWithdrawalsAsync(bool getReconciled = false)
        {
            switch (this.AccountType)
            {
                case BankAccountType.Checking:
                    await LoadCheckingWithdrawalsAsync(getReconciled);
                    break;
                case BankAccountType.Savings:
                    await LoadSavingsWithdrawalsAsync(getReconciled);
                    break;
            }
        }

        async Task LoadCheckingAccountAsync(int accountId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetCheckingAccountAsync(accountId);
                if (_results.Successful)
                {
                    await PopulateVMAsync(_results.Results);
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
                        WriteErrorCondition("An unknown error has occurred loading bank account records");
                    }
                }
            }
        }

        async Task LoadSavingsAccountAsync(int accountId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetSavingsAccountAsync(accountId);
                if (_results.Successful)
                {
                    await PopulateVMAsync(_results.Results);
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
                        WriteErrorCondition("An unknown error has occurred loading bank account records");
                    }
                }
            }
        }

        async Task LoadCheckingDepositsAsync(bool getReconciled = false)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetCheckingDepositsAsync(model.id, getReconciled);
                if (_results.Successful)
                {
                    foreach(var deposit in _results.Results)
                    {
                        deposit.checkingAccount = model as CheckingAccount;

                        CheckingDepositViewModel vm = new CheckingDepositViewModel(this.dbFilePath);
                        vm.IsNew = false;
                        vm.CanEdit = true;
                        vm.CanDelete = true;
                        await vm.PopulateVMAsync(deposit);

                        this.AccountRegister.Add(vm);
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
                        WriteErrorCondition("An unknown error has occurred loading deposit records");
                    }
                }
            }
        }

        async Task LoadCheckingWithdrawalsAsync(bool getReconciled = false)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetCheckingWithdrawalsAsync(model.id, getReconciled);
                if (_results.Successful)
                {
                    foreach (var withdrawal in _results.Results)
                    {
                        withdrawal.checkingAccount = model as CheckingAccount;

                        CheckingWithdrawalViewModel vm = new CheckingWithdrawalViewModel(this.dbFilePath);
                        vm.IsNew = false;
                        vm.CanEdit = true;
                        vm.CanDelete = true;
                        await vm.PopulateVMAsync(withdrawal);

                        this.AccountRegister.Add(vm);
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
                        WriteErrorCondition("An unknown error has occurred loading withdrawal records");
                    }
                }
            }
        }

        async Task LoadSavingsDepositsAsync(bool getReconciled = false)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetSavingsDepositsAsync(model.id, getReconciled);
                if (_results.Successful)
                {
                    foreach (var deposit in _results.Results)
                    {
                        deposit.savingsAccount = model as SavingsAccount;

                        SavingsDepositViewModel vm = new SavingsDepositViewModel(this.dbFilePath);
                        vm.IsNew = false;
                        vm.CanEdit = true;
                        vm.CanDelete = true;
                        await vm.PopulateVMAsync(deposit);

                        this.AccountRegister.Add(vm);
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
                        WriteErrorCondition("An unknown error has occurred loading deposit records");
                    }
                }
            }
        }

        async Task LoadSavingsWithdrawalsAsync(bool getReconciled = false)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetSavingsWithdrawalsAsync(model.id, getReconciled);
                if (_results.Successful)
                {
                    foreach (var withdrawal in _results.Results)
                    {
                        withdrawal.savingsAccount = model as SavingsAccount;

                        SavingsWithdrawalViewModel vm = new SavingsWithdrawalViewModel(this.dbFilePath);
                        vm.IsNew = false;
                        vm.CanEdit = true;
                        vm.CanDelete = true;
                        await vm.PopulateVMAsync(withdrawal);

                        this.AccountRegister.Add(vm);
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
                        WriteErrorCondition("An unknown error has occurred loading withdrawal records");
                    }
                }
            }
        }

        public async Task SaveChangesAsync()
        {
            bool _saveOk = true;
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                switch (this.AccountType)
                {
                    case BankAccountType.Checking:
                        if (this.IsNew)
                        {
                            var _resultsAddChecking = await uow.AddCheckingAccountAsync(model as CheckingAccount);
                            _saveOk = _resultsAddChecking.Successful;
                            if (_saveOk)
                            {
                                this.IsDirty = false;
                                this.IsNew = false;
                                this.CanEdit = true;
                                this.CanDelete = true;
                                model = _resultsAddChecking.Results;
                            }
                            else
                            {
                                if (_resultsAddChecking.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsAddChecking.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsAddChecking.Message))
                                {
                                    WriteErrorCondition(_resultsAddChecking.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred saving account record");
                                }
                            }
                        }
                        else
                        {
                            var _resultsUpdateChecking = await uow.UpdateCheckingAccountAsync(model as CheckingAccount);
                            _saveOk = _resultsUpdateChecking.Successful;
                            if (_saveOk)
                            {
                                this.IsDirty = false;
                                this.IsNew = false;
                                this.CanEdit = true;
                                this.CanDelete = true;
                                model = _resultsUpdateChecking.Results;
                            }
                            else
                            {
                                if (_resultsUpdateChecking.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsUpdateChecking.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsUpdateChecking.Message))
                                {
                                    WriteErrorCondition(_resultsUpdateChecking.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred saving account record");
                                }
                            }
                        }
                        break;
                    case BankAccountType.Savings:
                        if (this.IsNew)
                        {
                            var _resultsAddSavings = await uow.AddSavingsAccountAsync(model as SavingsAccount);
                            _saveOk = _resultsAddSavings.Successful;
                            if (_saveOk)
                            {
                                this.IsDirty = false;
                                this.IsNew = false;
                                this.CanEdit = true;
                                this.CanDelete = true;
                                model = _resultsAddSavings.Results;
                            }
                            else
                            {
                                if (_resultsAddSavings.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsAddSavings.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsAddSavings.Message))
                                {
                                    WriteErrorCondition(_resultsAddSavings.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred saving account record");
                                }
                            }
                        }
                        else
                        {
                            var _resultsUpdateSavings = await uow.UpdateSavingsAccountAsync(model as SavingsAccount);
                            _saveOk = _resultsUpdateSavings.Successful;
                            if (_saveOk)
                            {
                                this.IsDirty = false;
                                this.IsNew = false;
                                this.CanEdit = true;
                                this.CanDelete = true;
                                model = _resultsUpdateSavings.Results;
                            }
                            else
                            {
                                if (_resultsUpdateSavings.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsUpdateSavings.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsUpdateSavings.Message))
                                {
                                    WriteErrorCondition(_resultsUpdateSavings.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred saving account record");
                                }
                            }
                        }
                        break;
                }

                if (_saveOk)
                {
                    foreach (var item in this.AccountRegister)
                    { 
                        if (item.IsDirty)
                        {
                            switch (item.ItemType)
                            {
                                case AccountRegisterItemViewModel.AccountItemType.Deposit:
                                    await (item as DepositViewModel).SaveChangesAsync();

                                    break;
                                case AccountRegisterItemViewModel.AccountItemType.Withdrawal:
                                    await (item as WithdrawalViewModel).SaveChangesAsync();
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
