using System;
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

        public decimal SelectedMonthBudgetedExpenseAmount { get; set; }

        public decimal SelectedMonthBudgetedIncomeAmount { get; set; }

        public decimal SelectedMonthIncomeTotal { get; set; }

        public decimal SelectedMonthExpenseTotal { get; set; }

        public EasyBudgetStatusViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task LoadStatusAsync()
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _resultsChecking = await uow.GetAllCheckingAccountsAsync();
                var _resultsSavings = await uow.GetAllSavingsAccountsAsync();
                var _resultsCategories = await uow.GetAllBudgetCategoriesAsync();

                if (_resultsChecking.Successful && _resultsSavings.Successful && _resultsCategories.Successful)
                {
                    
                }
            }
        }
    }
}
