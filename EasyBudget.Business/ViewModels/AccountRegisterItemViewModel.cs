using System;
using System.Collections.Generic;
using System.ComponentModel;
using EasyBudget.Business.ChartModels;

namespace EasyBudget.Business.ViewModels
{
    public abstract class AccountRegisterItemViewModel: BaseViewModel, INotifyPropertyChanged
    {
        int _ItemId;
        public int ItemId
        {
            get
            {
                return _ItemId;
            }
            set
            {
                if (_ItemId != value)
                {
                    _ItemId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemId)));
                }
            }
        }


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

        decimal _EndingBalance;
        public decimal EndingBalance
        {
            get
            {
                return _EndingBalance;
            }
            set
            {
                if (_EndingBalance != value)
                {
                    _EndingBalance = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndingBalance)));
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

        string _ObjectColorCode;
        public string ObjectColorCode
        {
            get
            {
                return _ObjectColorCode;
            }
            set
            {
                if (_ObjectColorCode != value)
                {
                    _ObjectColorCode = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ObjectColorCode)));
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
            Deposits,
            Withdrawals
        }

		public override IChartDataPack GetChartData()
		{
			throw new NotImplementedException();
		}
	}

    public class AccountRegisterItemViewModelComparer : IEqualityComparer<AccountRegisterItemViewModel>
    {
        public bool Equals(AccountRegisterItemViewModel x, AccountRegisterItemViewModel y)
        {
            return x.ItemId == y.ItemId;
        }

        public int GetHashCode(AccountRegisterItemViewModel obj)
        {
            return obj.ItemId.GetHashCode() + obj.ItemDate.GetHashCode() + obj.ItemDescription.GetHashCode();
        }
    }
}
