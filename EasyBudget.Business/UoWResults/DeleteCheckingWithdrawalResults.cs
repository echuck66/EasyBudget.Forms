using System;
namespace EasyBudget.Business.UoWResults
{
    public class DeleteCheckingWithdrawalResults : UnitOfWorkResults<bool>
    {
        public int WithdrawalId { get; set; }

        public DeleteCheckingWithdrawalResults()
        {
        }
    }
}
