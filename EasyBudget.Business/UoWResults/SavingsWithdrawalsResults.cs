using System;
using System.Collections.Generic;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
{
    public class SavingsWithdrawalsResults : UnitOfWorkResults<ICollection<SavingsWithdrawal>>
    {
        public SavingsWithdrawalsResults()
        {
        }
    }
}
