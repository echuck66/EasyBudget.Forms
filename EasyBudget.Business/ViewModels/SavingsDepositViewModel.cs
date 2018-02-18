using System;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{
    public class SavingsDepositViewModel : DepositViewModel
    {
        SavingsDeposit Deposit { get; set; }

        public SavingsDepositViewModel(string dbFilePath)
            : base(dbFilePath)
        {
        }
    }
}
