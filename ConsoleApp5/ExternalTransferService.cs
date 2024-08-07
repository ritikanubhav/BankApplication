using ConsoleApp5;

public class ExternalTransferService
{
    private Thread thread;
    private bool running = true;

    /*public ExternalTransferService()
    {
        thread = new Thread(new ThreadStart(Run));
    }*/

    public void Start()
    {
        thread.Start();
    }

    public void Stop()
    {
        running = false;
        thread.Join();
    }

    /*private void Run()
    {
        while (running)
        {
            // Get the list of EXTERNALTRANSFER transactions from the log
            List<ExternalTransfer> transactionLog = TransactionLog.GetTransactions(TransactionTypes.EXTERNALTRANSFER);

            foreach (ExternalTransfer transaction in transactionLog)
            {
                // If a transaction is found with status OPEN
                if (transaction.Status == TransactionStatus.OPEN)
                {
                    // Use ExternalBankServiceFactory to create an object of the Bank Class
                    IExternalBankService bankService = ExternalBankServiceFactory.Instance.GetService(transaction.ToExternalAccount.BankCode);

                    // Call the deposit method to update the table and set the status to CLOSED
                    bool success = bankService.deposit(transaction.ToExternalAccount.AccNo, transaction.Amount);

                    if (success)
                    {
                        // Update the status of the transaction to CLOSE
                        transaction.Status = TransactionStatus.CLOSE;
                        transaction.FromAccount.Balance -= transaction.Amount;

                        // Log the updated transaction

                        TransactionLog.UpdateTransaction(transaction);
                    }
                }
            }

            // Sleep for a defined period before the next iteration
            Thread.Sleep(TimeSpan.FromMinutes(10));
        }
    }*/
}
