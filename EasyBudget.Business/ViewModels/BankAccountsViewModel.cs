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
using EasyBudget.Business.ChartModels;
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
                        vm.CanEdit = true;
                        vm.CanDelete = true;
                        vm.IsNew = false;
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
                        BankAccountViewModel vm = new BankAccountViewModel(this.dbFilePath);
                        await vm.PopulateVMAsync(account);
                        vm.CanEdit = true;
                        vm.CanDelete = true;
                        vm.IsNew = false;
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

        public async Task AddCheckingAccountAsync()
        {
            BankAccountViewModel vm = new BankAccountViewModel(this.dbFilePath);
            CheckingAccount account = new CheckingAccount();
            await vm.PopulateVMAsync(account);
            vm.AccountType = Models.BankAccountType.Checking;
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
            vm.AccountType = Models.BankAccountType.Savings;
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

        //IList<AccountRegisterItemViewModel> GetAllTransactions(int accountId)
        //{
        //    List<AccountRegisterItemViewModel> _transactions = new List<AccountRegisterItemViewModel>();

        //    using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
        //    {
        //        var _resultsDeposits = Task.Run(() => uow.ge)
        //    }

        //    return _transactions;
        //}

        public override IChartDataPack GetChartData()
        {
            ChartDataPack chartPack = new ChartDataPack();


            ChartDataGroup allCategorizedGroup = new ChartDataGroup();
            allCategorizedGroup.ChartDisplayType = ChartType.Bar;
            allCategorizedGroup.ChartDisplayOrder = 0;

            //ChartDataGroup incomeCategorizedGroup = new ChartDataGroup();
            //incomeCategorizedGroup.ChartDisplayType = ChartType.Bar;
            //incomeCategorizedGroup.ChartDisplayOrder = 1;

            //ChartDataGroup spendingCategorizedGroup = new ChartDataGroup();
            //spendingCategorizedGroup.ChartDisplayType = ChartType.Bar;
            //spendingCategorizedGroup.ChartDisplayOrder = 2;

            List<List<AccountRegisterItemViewModel>> _registerVMsByCategory = new List<List<AccountRegisterItemViewModel>>();
            List<AccountRegisterItemViewModel> _allDepositTransactions = new List<AccountRegisterItemViewModel>();
            List<AccountRegisterItemViewModel> _allWithdrawalTransactions = new List<AccountRegisterItemViewModel>();
            
            decimal _depositSum = _allDepositTransactions.Sum(t => t.ItemAmount);
            decimal _withdrawalSum = _allWithdrawalTransactions.Sum(t => t.ItemAmount);

            foreach(var _accountVM in this.BankAccounts)
            {
                _allDepositTransactions.AddRange(_accountVM.AccountRegister.Where(r => r.ItemType == AccountRegisterItemViewModel.AccountItemType.Deposits));
                _allWithdrawalTransactions.AddRange(_accountVM.AccountRegister.Where(r => r.ItemType == AccountRegisterItemViewModel.AccountItemType.Withdrawals));

            }

            string _colorCode = string.Empty;
            List<AccountRegisterItemViewModel> _tempVMList = new List<AccountRegisterItemViewModel>();
            // Start with Deposits
            _registerVMsByCategory = new List<List<AccountRegisterItemViewModel>>();
            foreach(var _regVM in _allDepositTransactions.OrderBy(t => t.ObjectColorCode))
            {
                if (_regVM.ObjectColorCode != _colorCode)
                {
                    if (_tempVMList.Count > 0)
                    {
                        _registerVMsByCategory.Add(_tempVMList);
                    }
                    _colorCode = _regVM.ObjectColorCode;
                    _tempVMList = new List<AccountRegisterItemViewModel>();
                }
                _tempVMList.Add(_regVM);
            }
            if (_tempVMList.Count > 0)
            {
                _registerVMsByCategory.Add(_tempVMList);
            }
            foreach(var _list in _registerVMsByCategory)
            {
                foreach (var _listItm in _list)
                {
                    decimal _itmValue = _listItm.ItemAmount;
                    ChartDataEntry _entry = new ChartDataEntry();
                    _entry.FltValue = (float)(_itmValue);
                    _entry.Label = "Item Value";
                    _entry.ValueLabel = _itmValue.ToString("C");
                    _entry.ColorCode = _listItm.ObjectColorCode;
                    allCategorizedGroup.ChartDataItems.Add(_entry);
                }
                //decimal _catValue = _list.Sum(r => r.ItemAmount);
                //ChartDataEntry _entry = new ChartDataEntry();
                //_entry.FltValue = (float)_catValue;
                //_entry.Label = "Category Total";
                //_entry.ColorCode = _list.First().ObjectColorCode;
                ////incomeCategorizedGroup.ChartDataItems.Add(_entry);
                //allCategorizedGroup.ChartDataItems.Add(_entry);
            }

            // and Withdrawals
            _registerVMsByCategory = new List<List<AccountRegisterItemViewModel>>();
            foreach(var _regVM in _allWithdrawalTransactions.OrderBy(t => t.ObjectColorCode))
            {
                if (_regVM.ObjectColorCode != _colorCode)
                {
                    if (_tempVMList.Count > 0)
                    {
                        _registerVMsByCategory.Add(_tempVMList);
                    }
                    _colorCode = _regVM.ObjectColorCode;
                    _tempVMList = new List<AccountRegisterItemViewModel>();
                }
                _tempVMList.Add(_regVM);
            }
            if (_tempVMList.Count > 0)
            {
                _registerVMsByCategory.Add(_tempVMList);
            }
            foreach (var _list in _registerVMsByCategory)
            {
                foreach (var _listItm in _list)
                {
                    decimal _itmValue = _listItm.ItemAmount;
                    ChartDataEntry _entry = new ChartDataEntry();
                    _entry.FltValue = (float)(-1 * _itmValue);
                    _entry.Label = "Item Value";
                    _entry.ValueLabel = _itmValue.ToString("C");
                    _entry.ColorCode = _listItm.ObjectColorCode;
                    allCategorizedGroup.ChartDataItems.Add(_entry);
                }
                //decimal _catValue = _list.Sum(r => r.ItemAmount);
                //ChartDataEntry _entry = new ChartDataEntry();
                //_entry.FltValue = (float)(-1 * _catValue);
                //_entry.Label = "Category Total";
                //_entry.ColorCode = _list.First().ObjectColorCode;
                ////spendingCategorizedGroup.ChartDataItems.Add(_entry);
                //allCategorizedGroup.ChartDataItems.Add(_entry);
            }

            chartPack.Charts.Add(allCategorizedGroup);
            //chartPack.Charts.Add(incomeCategorizedGroup);
            //chartPack.Charts.Add(spendingCategorizedGroup);

            //ChartDataGroup accountBalanceGroup = new ChartDataGroup();
            //accountBalanceGroup.ChartDisplayType = ChartType.Line;
            //accountBalanceGroup.ChartDisplayOrder = 0;

            //int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            //// Initialize the time-series chart with an entry for each day in the month
            //List<ChartDataEntry> _chartData = new List<ChartDataEntry>();
            //for (int i = 0; i < daysInMonth; i++)
            //{
            //    ChartDataEntry _chartEntry = new ChartDataEntry();
            //    _chartEntry.ColorCode = "#4CAF50";
            //    _chartEntry.Label = DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + (i + 1).ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString();
            //    _chartData.Add(_chartEntry);
            //}

            //// Find the data for the charts. We want the sum of balances for each day in the graph up to and including
            //// the current date.
            //List<AccountRegisterItemViewModel> _masterRegister = new List<AccountRegisterItemViewModel>();
            //foreach(BankAccountViewModel _accountVM in this.BankAccounts)
            //{
            //    _masterRegister.AddRange(_accountVM.AccountRegister);
            //}

            //foreach(ChartDataEntry _chartEntry in _chartData)
            //{
            //    int idx = _chartData.IndexOf(_chartEntry);
            //    int day = idx + 1;
            //    string dateToParse = DateTime.Now.Month.ToString() + "/" + day.ToString() + "/" + DateTime.Now.Year.ToString();
            //    DateTime _testDate = DateTime.Parse(dateToParse);
            //    if (_masterRegister.Any(itm => itm.ItemDate.Date ==_testDate.Date))
            //    {
            //        decimal _maxEndingBalance = _masterRegister.Where(itm => itm.ItemDate.Date == _testDate.Date).Max(r => r.EndingBalance);
            //        _chartEntry.FltValue = (float)_maxEndingBalance;
            //        _chartEntry.ValueLabel = _maxEndingBalance.ToString("C");
            //    }
            //    else
            //    {
            //        if (_masterRegister.Count() == 0)
            //        {
            //            _chartEntry.FltValue = (float)this.BankAccounts.Max(b => b.CurrentBalance);
            //            _chartEntry.ValueLabel = _chartEntry.FltValue.ToString("C");
            //        }
            //        else
            //        {
            //            //_chartEntry.FltValue = (float)0M;
            //            //_chartEntry.ValueLabel = string.Format("{0:C}", 0);
            //            if (day <= DateTime.Now.Day)
            //            {
            //                _chartEntry.FltValue = idx > 0 ? _chartData[idx - 1].FltValue : (float)0M;
            //                _chartEntry.ValueLabel = _chartEntry.FltValue.ToString("C");
            //            }
            //            else
            //            {
            //                _chartEntry.FltValue = (float)0M;
            //                _chartEntry.ValueLabel = _chartEntry.FltValue.ToString("C");
            //            }
            //        }
            //    }
            //    accountBalanceGroup.ChartDataItems.Add(_chartEntry);
            //}

            //chartPack.Charts.Add(accountBalanceGroup);

            return chartPack;
        }
    }

}
