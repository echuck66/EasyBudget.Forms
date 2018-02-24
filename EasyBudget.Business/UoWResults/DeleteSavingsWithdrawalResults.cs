using System;
namespace EasyBudget.Business.UoWResults
{
    public class DeleteSavingsWithdrawalResults : UnitOfWorkResults<bool>
    {
        public int WithdrawalId { get; set; }

        public DeleteSavingsWithdrawalResults()
        {
        }
    }
}
