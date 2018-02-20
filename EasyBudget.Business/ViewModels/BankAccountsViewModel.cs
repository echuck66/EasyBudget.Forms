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
using System.Threading.Tasks;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class BankAccountsViewModel : BaseViewModel
    {

        public ObservableCollection<BankAccountViewModel> BankAccounts { get; set; }

        internal BankAccountsViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            BankAccounts = new ObservableCollection<BankAccountViewModel>();
        }

        internal async Task LoadVMAsync()
        {
            await LoadCheckingAccountsAsync();
            await LoadSavingsAccountsAsync();
        }

        private async Task LoadCheckingAccountsAsync()
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

        private async Task LoadSavingsAccountsAsync()
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
        }
    }

}
