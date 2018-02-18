using System;
using System.Collections.Generic;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
{
    public class SavingsDepositsResults : UnitOfWorkResults<ICollection<SavingsDeposit>>
    {
        public SavingsDepositsResults()
        {
        }
    }
}
