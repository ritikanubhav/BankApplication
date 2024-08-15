

using BankApplication.Common;
namespace BankApplication.BusinessLayer
{
    public class ExternalTransfer : Transaction
    {
        public ExternalAccount ToExternalAccount { get; set; }
        public string FromAccPin { get; set; }
        public ExternalTransfer(string fromAccount, double amount, ExternalAccount toExternalAccount, string fromAccPin)
            : base(fromAccount, amount)
        {
            ToExternalAccount = toExternalAccount;
            FromAccPin = fromAccPin;
            Status = TransactionStatus.OPEN;
        }
    }

}
