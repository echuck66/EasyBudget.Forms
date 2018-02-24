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
using System.Linq;
using System.Threading.Tasks;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class BankAccountsViewModel : BaseViewModel, INotifyPropertyChanged, IDisposable
    {

        public ObservableCollection<BankAccountViewModel> BankAccounts { get; set; }
        public ObservableCollection<Grouping<string, BankAccountViewModel>> BankAccountsGrouped { get; set; }

        BankAccountViewModel _SelectedBankAccount;
        public BankAccountViewModel SelectedBankAccount
        {
            get
            {
                return _SelectedBankAccount;
            }
            set
            {
                _SelectedBankAccount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedBankAccount)));
            }
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        internal BankAccountsViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            BankAccounts = new ObservableCollection<BankAccountViewModel>();
            BankAccountsGrouped = new ObservableCollection<Grouping<string, BankAccountViewModel>>();
        }

        internal async Task LoadVMAsync()
        {
            await LoadCheckingAccountsAsync();
            await LoadSavingsAccountsAsync();
            await GroupAccountsAsync();
        }

        public void GroupAccounts()
        {
            var grouped = from bnk in this.BankAccounts
                          orderby bnk.BankName 
                          group bnk by bnk.AccountType into Group
                          select new Grouping<string, BankAccountViewModel>(Group.Key.ToString(), Group);

            this.BankAccountsGrouped = new ObservableCollection<Grouping<string, BankAccountViewModel>>(grouped.OrderBy(g => g.Key));
        }

        public async Task GroupAccountsAsync()
        {
            var grouped = from bnk in this.BankAccounts
                          orderby bnk.BankName
                          group bnk by bnk.AccountType into Group
                          select new Grouping<string, BankAccountViewModel>(Group.Key.ToString(), Group);

            this.BankAccountsGrouped = await Task.Run(() => new ObservableCollection<Grouping<string, BankAccountViewModel>>(grouped.OrderBy(g => g.Key)));
        }

        async Task LoadCheckingAccountsAsync()
        {

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllCheckingAccountsAsync();
                if (_results.Successful)
                {
                    foreach (var account in _results.Results)
                    {
                        BankAccountViewModel vm = new BankAccountViewModel(this.dbFilePath);
                        await vm.PopulateVMAsync(account);
                        this.BankAccounts.Add(vm);
                    }
                    await GroupAccountsAsync();
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
                        WriteErrorCondition("An unknown error has occurred");
                    }
                }
            }
        }

        async Task LoadSavingsAccountsAsync()
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllSavingsAccountsAsync();
                if (_results.Successful)
                {
                    foreach (var account in _results.Results)
                    {
                        //this.BankAccountVMs.Add(account);
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
                        WriteErrorCondition("An unknown error has occurred");
                    }
                }
            }
        }

        public async Task AddCheckingAccountAsync()
        {
            BankAccountViewModel vm = new BankAccountViewModel(this.dbFilePath);
            CheckingAccount account = new CheckingAccount();
            await vm.PopulateVMAsync(account);
            vm.IsNew = true;
            vm.CanEdit = true;
            vm.CanDelete = false;
            this.BankAccounts.Add(vm);
            this.SelectedBankAccount = vm;
            await GroupAccountsAsync();
        }

        public async Task AddsavingsAccountAsync()
        {
            BankAccountViewModel vm = new BankAccountViewModel(this.dbFilePath);
            SavingsAccount account = new SavingsAccount();
            await vm.PopulateVMAsync(account);
            vm.IsNew = true;
            vm.CanEdit = true;
            vm.CanDelete = false;
            this.BankAccounts.Add(vm);
            this.SelectedBankAccount = vm;
            await GroupAccountsAsync();
        }

        public async Task<bool> DeleteBankAccountAsync(BankAccountViewModel vm)
        {
            bool deleted = false;
            var itemList = new List<BankAccountViewModel>();

            if (vm.CanDelete && this.BankAccounts.Contains(vm, new BankAccountComparer()))
            {
                deleted = await vm.DeleteAsync();
                if (deleted)
                {
                    this.BankAccounts.Remove(vm);
                    await GroupAccountsAsync();
                }
            }
            else
            {
                this.WriteErrorCondition("Unable to locate provided item in the source collection");
            }

            return deleted;
        }

        public void Dispose()
        {
            foreach(BankAccountViewModel vm in this.BankAccounts)
            {
                vm.Dispose();
            }
        }
    }

}
