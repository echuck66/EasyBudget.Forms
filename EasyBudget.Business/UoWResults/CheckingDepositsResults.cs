using System;
using System.Collections.Generic;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
{
    public class CheckingDepositsResults : UnitOfWorkResults<ICollection<CheckingDeposit>>
    {
        public CheckingDepositsResults()
        {
        }
    }
}
