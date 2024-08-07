using System.Transactions;

namespace ConsoleApp5
{
    public class ExternalTransfer : Transaction
    {
        public ExternalAccount ToExternalAccount { get; set; }
        public string FromAccPin { get; set; }
        public ExternalTransfer(IAccount fromAccount, double amount, ExternalAccount toExternalAccount, string fromAccPin)
            : base(fromAccount, amount)
        {
            ToExternalAccount = toExternalAccount;
            FromAccPin = fromAccPin;
            Status = TransactionStatus.OPEN;
        }
    }

}
