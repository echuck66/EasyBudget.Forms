namespace EasyBudget.Models.DataModels
{
    public abstract class BankAccount : BaseObject
    {

        public string bankName { get; set; }

        public string routingNumber { get; set; }

        public string accountNumber { get; set; }

        public string accountNickname { get; set; }

        public BankAccountType accountType { get; set; }

        public decimal currentBalance { get; set; }

        public BankAccount()
        {
        }
    }
}
