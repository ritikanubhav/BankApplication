using BankApplication.DataAccess;

using BankApplication.Common;
namespace BankApplication.BusinessLayer
{
    public static class ResultGenerator
    {
        public static void PrintAllLogTransactions()
        {
            var allTransactions = TransactionLog.GetTransactions();
            foreach (var accountTransactions in allTransactions)
            {
                foreach (var transactionType in accountTransactions.Value)
                {
                    foreach (var transaction in transactionType.Value)
                    {
                        Console.WriteLine(transaction);
                    }
                }
            }
        }

        public static void PrintAllLogTransactions(string accountId)
        {
            var accountTransactions = TransactionLog.GetTransactions(accountId);
            foreach (var transactionType in accountTransactions)
            {
                foreach (var transaction in transactionType.Value)
                {
                    Console.WriteLine(transaction);
                }
            }
        }

        public static void PrintAllLogTransactions(TransactionTypes transactionType)
        {
            var allTransactions = TransactionLog.GetTransactions();
            foreach (var accountTransactions in allTransactions)
            {
                if (accountTransactions.Value.ContainsKey(transactionType))
                {
                    foreach (var transaction in accountTransactions.Value[transactionType])
                    {
                        Console.WriteLine(transaction);
                    }
                }
            }
        }

        /*public static int GetTotalNoOfAccounts()
        {
            var allAccounts = AccountManager.GetAllAccounts();
            return allAccounts.Count;
        }
*/
        /*public static void DisplayNoOfAccTypeWise()
        {
            var allAccounts = AccountManager.GetAllAccounts();
            var accountTypeCounts = allAccounts.GroupBy(acc => acc.AccType)
                                               .Select(group => new { AccType = group.Key, Count = group.Count() })
                                               .ToList();

            Console.WriteLine("Account Type\tNo Of Accounts");
            foreach (var accTypeCount in accountTypeCounts)
            {
                Console.WriteLine($"{accTypeCount.AccType}\t{accTypeCount.Count}");
            }
        }*/

        /*public static void DispTotalWorthOfBank()
        {
            var allAccounts = AccountManager.GetAllAccounts();
            double totalBalance = allAccounts.Sum(acc => acc.Balance);

            Console.WriteLine($"Total balance available : Rs {totalBalance:N2}");
        }

        public static void DispPolicyInfo()
        {
            var policyInfo = PolicyFactory.GetPolicyInfo();
            Console.WriteLine("Policy Type\tMinimum Balance\tRateOfInterest");
            foreach (var policy in policyInfo)
            {
                Console.WriteLine($"{policy.Key}\t{policy.Value.MinBalance}\t{policy.Value.RateOfInterest}");
            }
        }*/
        /*public static void PrintAllTransfers()
        {
            var allTransactions = TransactionLog.GetTransactions();
            Console.WriteLine("From\tTo\tDate\tAmount");

            foreach (var accountTransactions in allTransactions)
            {
                if (accountTransactions.Value.ContainsKey(TransactionType.TRANSFER))
                {
                    foreach (var transaction in accountTransactions.Value[TransactionType.TRANSFER])
                    {
                        Console.WriteLine($"{transaction.FromAccount.AccNo}\t{transaction.ToAccount.AccNo}\t{transaction.TranDate.ToShortDateString()}\t{transaction.Amount}");
                    }
                }
            }
        }*/

        // 9. To get the list of all withdrawals done, with their account information and display the same
        public static void PrintAllWithdrawals()
        {
            var allTransactions = TransactionLog.GetTransactions();
            Console.WriteLine("From\tDate\tAmount");

            foreach (var accountTransactions in allTransactions)
            {
                if (accountTransactions.Value.ContainsKey(TransactionTypes.WITHDRAW))
                {
                    foreach (var transaction in accountTransactions.Value[TransactionTypes.WITHDRAW])
                    {
                        Console.WriteLine($"{transaction.FromAccount.AccNo}\t{transaction.TranDate.ToShortDateString()}\t{transaction.Amount}");
                    }
                }
            }
        }

        // 10. To get the list of all deposits done, with their account information and display the same
        public static void PrintAllDeposits()
        {
            var allTransactions = TransactionLog.GetTransactions();
            Console.WriteLine("To\tDate\tAmount");

            foreach (var accountTransactions in allTransactions)
            {
                if (accountTransactions.Value.ContainsKey(TransactionTypes.DEPOSIT))
                {
                    foreach (var transaction in accountTransactions.Value[TransactionTypes.DEPOSIT])
                    {
                        Console.WriteLine($"{transaction.FromAccount.AccNo}\t{transaction.TranDate.ToShortDateString()}\t{transaction.Amount}");
                    }
                }
            }
        }

        // 11. To get all the transactions done for the day from the log
        /*public static void PrintAllTransactionsForToday()
        {
            var allTransactions = TransactionLog.GetTransactions();
            DateTime today = DateTime.Today;
            Console.WriteLine("From\tTo\tDate\tAmount");

            foreach (var accountTransactions in allTransactions)
            {
                foreach (var transactionTypes in accountTransactions.Value)
                {
                    foreach (var transaction in transactionTypes.Value)
                    {
                        if (transaction.TranDate.Date == today)
                        {
                            string toAccount = transaction.TransactionType == TransactionTypes.WITHDRAW ? "" : transaction.ToAccount?.AccNo;
                            Console.WriteLine($"{transaction.FromAccount.AccNo}\t{toAccount}\t{transaction.TranDate.ToShortDateString()}\t{transaction.Amount}");
                        }
                    }
                }
            }
        }*/
    }
}
