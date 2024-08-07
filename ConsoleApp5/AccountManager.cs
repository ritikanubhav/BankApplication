using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ConsoleApp5
{
    public class AccountManager
    {
        public IAccount CreateAccount(string name, string pin, double balance, PrivilegeType privilegeType, AccountType accType)
        {
            IAccount account =AccountFactory.CreateAccount(name, pin, balance, privilegeType, accType);

            PolicyFactory policyFactory = PolicyFactory.Instance;
            IPolicy policy;

            try
            {
                policy = policyFactory.CreatePolicy(accType.ToString(), privilegeType.ToString());
            }
            catch (InvalidPolicyTypeException)
            {
                throw new UnableToOpenAccountException("Invalid policy type.");
            }

            if (balance < policy.GetMinBalance())
            {
                throw new MinBalanceNeedsToBeMaintainedException("Minimum balance needs to be maintained.");
            }
            account.Policy = policy;
            return account;
        }

        public bool Withdraw(IAccount fromAccount, double amount, string pin)
        {
            if (fromAccount == null)
            {
                throw new AccountDoesNotExistException("Account does not exist.");
            }

            if (!fromAccount.Active)
            {
                throw new InactiveAccountException("Account is inactive.");
            }

            if (fromAccount.Pin != pin)
            {
                throw new InvalidPinException("Invalid PIN.");
            }

            if (fromAccount.Balance < amount)
            {
                throw new InsufficientBalanceException("Insufficient balance.");
            }

            fromAccount.Balance -= amount;

            TransactionLog.LogTransaction(fromAccount.AccNo, TransactionTypes.WITHDRAW, new Transaction(fromAccount, amount));

            return true;
        }

        
        public bool Deposit(IAccount toAccount, double amount)
        {
            if (toAccount == null)
            {
                throw new AccountDoesNotExistException("Account does not exist.");
            }

            if (!toAccount.Active)
            {
                throw new InactiveAccountException("Account is inactive.");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be greater than zero.");
            }

            toAccount.Balance += amount;

            TransactionLog.LogTransaction(toAccount.AccNo, TransactionTypes.DEPOSIT, new Transaction(toAccount, amount));
            
            return true;
        }

        public bool TransferFunds(Transfer transfer)
        {
            if (transfer.FromAccount == null || transfer.ToAccount == null)
            {
                throw new AccountDoesNotExistException("One or both accounts do not exist.");
            }

            if (!transfer.FromAccount.Active || !transfer.ToAccount.Active)
            {
                throw new InactiveAccountException("One or both accounts are inactive.");
            }

            if (transfer.FromAccount.Pin != transfer.Pin)
            {
                throw new InvalidPinException("Invalid PIN.");
            }

            if (transfer.FromAccount.Balance < transfer.Amount)
            {
                throw new InsufficientBalanceException("Insufficient balance.");
            }

            if (transfer.FromAccount.Balance - transfer.Amount < transfer.FromAccount.Policy.GetMinBalance())
            {
                throw new MinBalanceNeedsToBeMaintainedException("Minimum balance needs to be maintained.");

            }

            // Check if the transaction is within permissible daily limits
            double dailyLimit = AccountPrivilegeManager.GetDailyLimit(transfer.FromAccount.PrivilegeType);
            double totalTransferredAmountToday = TransactionLog.GetTotalTransferredAmountToday(transfer.FromAccount.AccNo);

            if (totalTransferredAmountToday + transfer.Amount > dailyLimit)
                throw new DailyLimitExceededException();

            transfer.FromAccount.Balance -= transfer.Amount;
            transfer.ToAccount.Balance += transfer.Amount;

            int transactionID = IDGenerator.GenerateID();

            Transaction withdrawTransaction = new Transaction(transfer.FromAccount, transfer.Amount);
            Transaction depositTransaction = new Transaction(transfer.ToAccount, transfer.Amount);

            TransactionLog.LogTransaction(transfer.FromAccount.AccNo, TransactionTypes.WITHDRAW, withdrawTransaction);
            TransactionLog.LogTransaction(transfer.ToAccount.AccNo,TransactionTypes.DEPOSIT, depositTransaction);

            withdrawTransaction.Status = TransactionStatus.CLOSE;
            depositTransaction.Status = TransactionStatus.CLOSE;

            return true;
        }

        public bool ExternalTransferFunds(ExternalTransfer externalTransfer)
        {
            if (!externalTransfer.FromAccount.Active)
                throw new InactiveAccountException("Inactive account");

            if (externalTransfer.FromAccount.Pin != externalTransfer.FromAccPin)
                throw new InvalidPinException("Invalid Pin");

            if (externalTransfer.FromAccount.Balance < externalTransfer.Amount)
                throw new InsufficientBalanceException("Insufficient Balnace");

            if (externalTransfer.FromAccount.Balance - externalTransfer.Amount < externalTransfer.FromAccount.Policy.GetMinBalance())
                throw new MinBalanceNeedsToBeMaintainedException("Minimum balance needs to be maintained.");

            // Check if the transaction is within permissible daily limits
            double dailyLimit = AccountPrivilegeManager.GetDailyLimit(externalTransfer.FromAccount.PrivilegeType);
            double totalTransferredAmountToday = TransactionLog.GetTotalTransferredAmountToday(externalTransfer.FromAccount.AccNo);

            if (totalTransferredAmountToday + externalTransfer.Amount > dailyLimit)
                throw new DailyLimitExceededException();

            externalTransfer.FromAccount.Balance -= externalTransfer.Amount;

            // Assuming success in external transfer, log transaction and update status
            TransactionLog.LogTransaction(externalTransfer.FromAccount.AccNo, TransactionTypes.EXTERNALTRANSFER, externalTransfer);
            externalTransfer.Status = TransactionStatus.OPEN;
            return true;
        }
    }

    

}
