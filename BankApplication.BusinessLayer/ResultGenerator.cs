using BankApplication.DataAccess;
using BankApplication.Common;
namespace BankApplication.BusinessLayer
{
    public static class ResultGenerator
    { 
        public static void PrintAllLogTransactions()
        {
            var allTransactions = TransactionLog.GetTransactions();
            Console.WriteLine("\n******************************   Report Starts   *************************************\n");
            
            foreach (var accountTransactions in allTransactions)
            {
                Console.WriteLine("-----------------------------------------------------------------------------------------------");
                Console.WriteLine($"Account No:{accountTransactions.Key}\n");

                foreach (var transactionType in accountTransactions.Value)
                {
                    Console.WriteLine($"Transaction Type:{transactionType.Key}");
                    Console.WriteLine();

                    Console.WriteLine("ID\t\tAccount No\t\t Date\t\t\tAmount\t\tStatus\n");

                    foreach (var transaction in transactionType.Value)
                    {
                        Console.WriteLine($"{transaction.TransID}\t\t{transaction.FromAccount}\t\t{transaction.TranDate}\t\t{transaction.Amount}\t\t{transaction.Status.ToString()}");
                    }
                    Console.WriteLine();
                }
            }
            Console.WriteLine("\n******************************   Report Ends   *************************************\n");
        }

        public static void PrintAllLogTransactions(string accountId)
        {
            var accountTransactions = TransactionLog.GetTransactions(accountId);
            Console.WriteLine("\n******************************   Report Starts   *************************************\n");
            Console.WriteLine($"Account No:{accountId}\n");

            foreach (var transactionType in accountTransactions)
            {
                Console.WriteLine($"Transaction Type:{transactionType.Key}");
                Console.WriteLine();

                Console.WriteLine("ID\t\tAccount No\t\t Date\t\t\tAmount\t\tStatus\n");

                foreach (var transaction in transactionType.Value)
                {
                    Console.WriteLine($"{transaction.TransID}\t\t{transaction.FromAccount}\t\t{transaction.TranDate}\t\t{transaction.Amount}\t\t{transaction.Status.ToString()}");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n******************************   Report Ends   ****************************************\n");

        }

        public static void PrintAllLogTransactions(TransactionTypes transactionType)
        {
            var allTransactions = TransactionLog.GetTransactions();
            Console.WriteLine("\n******************************   Report Starts   *************************************\n");
            Console.WriteLine(transactionType.ToString());
            Console.WriteLine();
            Console.WriteLine("ID\t\tAccount No\t\t Date\t\t\tAmount\t\tStatus\n");
            foreach (var accountTransactions in allTransactions)
            {
                if (accountTransactions.Value.ContainsKey(transactionType))
                {
                    foreach (var transaction in accountTransactions.Value[transactionType])
                    {
                        Console.WriteLine($"{transaction.TransID}\t\t{transaction.FromAccount}\t\t{transaction.TranDate}\t\t{transaction.Amount}\t\t{transaction.Status.ToString()}");
                    }
                }
            }
            Console.WriteLine("\n******************************   Report Ends   ****************************************\n");
        }

        public static int GetTotalNoOfAccounts()
        {
            BankApplicationDbRepository bankApplicationDbRepository = new BankApplicationDbRepository();
            var allAccounts = bankApplicationDbRepository.GetAllAccounts();
            return allAccounts.Count;
        }

        public static void DisplayNoOfAccTypeWise()
        {
            BankApplicationDbRepository bankApplicationDbRepository = new BankApplicationDbRepository();
            var allAccounts = bankApplicationDbRepository.GetAllAccounts();
            var accountTypeCounts = allAccounts.GroupBy(acc => acc.GetAccType())
                                               .Select(group => new { AccType = group.Key, Count = group.Count() }) .ToList();

            Console.WriteLine("\nAccount Type\tNo Of Accounts\n");
            foreach (var accTypeCount in accountTypeCounts)
            {
                Console.WriteLine($"{accTypeCount.AccType}\t\t{accTypeCount.Count}");
            }
        }

        public static void DispTotalWorthOfBank()
        {
            BankApplicationDbRepository bankApplicationDbRepository = new BankApplicationDbRepository();
            List<IAccount> allAccounts = bankApplicationDbRepository.GetAllAccounts();
            double totalBalance = allAccounts.Sum(acc => acc.Balance);

            Console.WriteLine($"Total Worth of Asset Available in Bank is : Rs {totalBalance:N2}");
        }

        public static void DispPolicyInfo()
        {
            PolicyFactory pf = PolicyFactory.Instance;
            var policyInfo = pf.GetPolicyInfo();
            Console.WriteLine("\nPolicy Type\t  Minimum Balance\tRate Of Interest\n");
            foreach (var policy in policyInfo)
            {
                Console.WriteLine($"{policy.Key}\t\t{policy.Value.GetMinBalance()}\t\t{policy.Value.GetRateOfInterest()}");
            }
        }
        public static void PrintAllTransfers()
        {
            var allTransactions = TransactionLog.GetTransactions();
            Console.WriteLine("\nFrom\t\tTo\tDate\t\tAmount\n");

            foreach (var accountTransactions in allTransactions)
            {
                if (accountTransactions.Value.ContainsKey(TransactionTypes.TRANSFER))
                {
                    foreach (var transaction in accountTransactions.Value[TransactionTypes.TRANSFER])
                    {
                        Console.WriteLine($"{transaction.FromAccount}\t\t{"NA"}\t{transaction.TranDate.ToShortDateString()}\t{transaction.Amount}");
                    }
                }
            }
        }

        // 9. To get the list of all withdrawals done, with their account information and display the same
        public static void PrintAllWithdrawals()
        {
            var allTransactions = TransactionLog.GetTransactions();
            Console.WriteLine("\nFrom\t\tDate\t\tAmount\n");

            foreach (var accountTransactions in allTransactions)
            {
                if (accountTransactions.Value.ContainsKey(TransactionTypes.WITHDRAW))
                {
                    foreach (var transaction in accountTransactions.Value[TransactionTypes.WITHDRAW])
                    {
                        Console.WriteLine($"{transaction.FromAccount}\t\t{transaction.TranDate.ToShortDateString()}\t{transaction.Amount}");
                    }
                }
            }
        }

        // 10. To get the list of all deposits done, with their account information and display the same
        public static void PrintAllDeposits()
        {
            var allTransactions = TransactionLog.GetTransactions();
            Console.WriteLine("\nTo\t\tDate\t\tAmount\n");

            foreach (var accountTransactions in allTransactions)
            {
                if (accountTransactions.Value.ContainsKey(TransactionTypes.DEPOSIT))
                {
                    foreach (var transaction in accountTransactions.Value[TransactionTypes.DEPOSIT])
                    {
                        Console.WriteLine($"{transaction.FromAccount}\t\t{transaction.TranDate.ToShortDateString()}\t{transaction.Amount}");
                    }
                }
            }
        }

        //11. To get all the transactions done for the day from the log
        public static void PrintAllTransactionsForToday()
        {
            var allTransactions = TransactionLog.GetTransactions();
            DateTime today = DateTime.Today;
            Console.WriteLine("\nFrom\tTo\t\tDate\t  Amount\n");

            foreach (var accountTransactions in allTransactions)
            {
                foreach (var transactionTypes in accountTransactions.Value)
                {
                    foreach (var transaction in transactionTypes.Value)
                    {
                        if (transaction.TranDate.Date == today)
                        {
                            string toAccount = transactionTypes.Key == TransactionTypes.WITHDRAW ? "---" : transaction.FromAccount;
                            string fromAccount = transactionTypes.Key == TransactionTypes.DEPOSIT ? "---" : transaction.FromAccount;
                            Console.WriteLine($"{fromAccount}\t{toAccount}\t    {transaction.TranDate.ToShortDateString()}\t   {transaction.Amount}");
                        }
                    }
                }
            }
        }
    }
}
