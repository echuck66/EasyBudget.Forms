using System;
using System.ComponentModel;

namespace EasyBudget.Business.ViewModels
{
    public abstract class AccountRegisterItemViewModel: BaseViewModel, INotifyPropertyChanged
    {
        string _ItemDescription;
        public string ItemDescription
        {
            get
            {
                return _ItemDescription;
            }
            set
            {
                if (_ItemDescription != value)
                {
                    _ItemDescription = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemDescription)));
                }
            }
        }

        decimal _ItemAmount;
        public decimal ItemAmount
        {
            get
            {
                return _ItemAmount;
            }
            set
            {
                if (_ItemAmount != value)
                {
                    _ItemAmount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemAmount)));
                }
            }
        }

        DateTime _ItemDate;
        public DateTime ItemDate
        {
            get
            {
                return _ItemDate;
            }
            set
            {
                if (_ItemDate != value)
                {
                    _ItemDate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemDate)));
                }
            }
        }

        AccountItemType _ItemType;
        public AccountItemType ItemType
        {
            get
            {
                return _ItemType;
            }
            set
            {
                if (_ItemType != value)
                {
                    _ItemType = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemType)));
                }
            }
        }

        public AccountRegisterItemViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        public enum AccountItemType
        {
            Deposit,
            Withdrawal
        }
    }
}
