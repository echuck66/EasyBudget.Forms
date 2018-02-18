using System;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{
    public class CheckingDepositViewModel : DepositViewModel
    {
        CheckingDeposit Deposit { get; set; }


        public CheckingDepositViewModel(string dbFilePath)
            : base(dbFilePath)
        {
        }
    }
}
