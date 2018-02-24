using System;
namespace EasyBudget.Business.UoWResults
{
    public class DeleteSavingsDepositResults : UnitOfWorkResults<bool>
    {
        public int DepositId { get; set; }

        public DeleteSavingsDepositResults()
        {
        }
    }
}
