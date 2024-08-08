using System.Transactions;

using BankApplication.Common;
namespace BankApplication.BusinessLayer
{
    public static class TransactionLog
    {
        private static readonly Dictionary<string, Dictionary<TransactionTypes, List<Transaction>>> transactionLog
            = new Dictionary<string, Dictionary<TransactionTypes, List<Transaction>>>();

        public static void LogTransaction(string accNo, TransactionTypes type, Transaction transaction)
        {
            if (!transactionLog.ContainsKey(accNo))
            {
                transactionLog[accNo] = new Dictionary<TransactionTypes, List<Transaction>>();
            }

            if (!transactionLog[accNo].ContainsKey(type))
            {
                transactionLog[accNo][type] = new List<Transaction>();
            }

            transactionLog[accNo][type].Add(transaction);
        }
        public static Dictionary<string, Dictionary<TransactionTypes, List<Transaction>>> GetTransactions()
        {
            return transactionLog;
        }

        public static Dictionary<TransactionTypes, List<Transaction>> GetTransactions(string accNo)
        {
            if (!transactionLog.ContainsKey(accNo))
                throw new TransactionNotFoundException();

            return transactionLog[accNo];
        }

        public static List<Transaction> GetTransactions(string accNo, TransactionTypes type)
        {
            if (!transactionLog.ContainsKey(accNo) || !transactionLog[accNo].ContainsKey(type))
                throw new InvalidTransactionTypeException();

            return transactionLog[accNo][type];
        }

        public static double GetTotalTransferredAmountToday(string accNo)
        {
            if (!transactionLog.ContainsKey(accNo) || !transactionLog[accNo].ContainsKey(TransactionTypes.TRANSFER))
                return 0;

            DateTime today = DateTime.Today;
            double totalAmount = 0;
            foreach (var transaction in transactionLog[accNo][TransactionTypes.TRANSFER])
            {
                if (transaction.TranDate.Date == today)
                {
                    totalAmount += transaction.Amount;
                }
            }
            return totalAmount;
        }

    }
}
