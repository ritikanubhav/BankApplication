
namespace BankApplication.Common
{
    public class Transaction
    {
        public int TransID { get; set; }
        public IAccount FromAccount { get; set; }
        public DateTime TranDate { get; set; }
        public double Amount { get; set; }
        public TransactionStatus Status { get; set; }

        public Transaction(IAccount fromAccount, double amount)
        {
            TransID = IDGenerator.GenerateID();
            FromAccount = fromAccount;
            TranDate = DateTime.Now;
            Amount = amount;
            Status = TransactionStatus.CLOSE; // Default status for normal Transfer
        }
    }
    public enum TransactionStatus
    {
        OPEN,
        CLOSE
    }
}
