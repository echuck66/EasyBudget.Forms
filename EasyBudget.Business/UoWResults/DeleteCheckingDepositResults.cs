using System;
namespace EasyBudget.Business.UoWResults
{
    public class DeleteCheckingDepositResults : UnitOfWorkResults<bool>
    {
        public int DepositId { get; set; }

        public DeleteCheckingDepositResults()
        {
        }
    }
}
