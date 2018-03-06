using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;

namespace EasyBudget.Business.ViewModels
{
    public class EasyBudgetStatusViewModel : BaseViewModel, INotifyPropertyChanged
    {
        int _SelectedMonth;
        public int SelectedMonth 
        {
            get 
            {
                return _SelectedMonth;
            }
            set
            {
                if (_SelectedMonth != value)
                {
                    _SelectedMonth = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMonth)));
                }
            }
        }

        BankAccountsViewModel _vmAccounts;
        public BankAccountsViewModel vmAccounts 
        {
            get
            {
                return _vmAccounts;
            }
            set
            {
                _vmAccounts = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(vmAccounts)));
            }
        }

        BudgetCategoriesViewModel _vmCategories;
        public BudgetCategoriesViewModel vmCategories
        {
            get
            {
                return _vmCategories;
            }
            set
            {
                _vmCategories = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(vmCategories)));
            }
        }

        BudgetCategoryViewModel _SelectedCategory;
        public BudgetCategoryViewModel SelectedCategory
        {
            get
            {
                return _SelectedCategory;
            }
            set
            {
                _SelectedCategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCategory)));
            }

        }



        public EasyBudgetStatusViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        public async Task LoadVMAsync()
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _resultsBanking = await EasyBudgetDataService.Instance.GetBankAccountsViewModelAsync();
                var _resultsCategories = await EasyBudgetDataService.Instance.GetBudgetCategoriesViewModelAsync();

                if (_resultsCategories != null)
                {
                    this.vmCategories = _resultsCategories;
                }
                else
                {
                    // TODO Something went wrong with Categories
                }

                if (_resultsBanking != null){
                    this.vmAccounts = _resultsBanking;
                }
                else
                {
                    // TODO Something went wrong with Banking
                }
            }
        }
    }
}
