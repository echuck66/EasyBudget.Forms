using System;
using System.Collections.Generic;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
{
    public class CheckingWithdrawalsResults : UnitOfWorkResults<ICollection<CheckingWithdrawal>>
    {
        public CheckingWithdrawalsResults()
        {
        }
    }
}
