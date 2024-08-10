using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using BankApplication.DataAccess;

using BankApplication.Common;
using System.Security.AccessControl;
namespace BankApplication.BusinessLayer
{
    public class AccountManager
    {
        //creating accountrepo instance to use its methods for manipulating the accounts table in database
        BankApplicationDbRepository accountRepo = new BankApplicationDbRepository();

        public IAccount CreateAccount(string name, string pin, double balance, PrivilegeType privilegeType, AccountType accType)
        {
            //calling accountfactory createaccount method 
            IAccount account =AccountFactory.CreateAccount(name, pin, balance, privilegeType, accType);

            //creating policy for the account using policyfactory instance
            PolicyFactory policyFactory = PolicyFactory.Instance;
            IPolicy policy;
            try
            {
                policy = policyFactory.CreatePolicy(accType.ToString(), privilegeType.ToString());
            }
            catch (InvalidPolicyTypeException e)
            {
                throw e;
            }

            //validating minimum balance requirement for the specific policy
            if (balance < policy.GetMinBalance())
            {
                throw new MinBalanceNeedsToBeMaintainedException($"Minimum Balance needs to be maintained .\nMin balnce should be: {policy.GetMinBalance()}");
            }

            // assigning polict to the account
            account.Policy = policy;

            // activating the account and assigning account no
            account.Open();

            //sending account data to accounts table in our database
            accountRepo.Create(account);

            return account;
        }
        
        public bool Deposit(string  AccNo, double amount)
        {
            //getting account details from accounts table in database
            IAccount toAccount=accountRepo.GetAccountByAccNo(AccNo);

            //validating details otherwise throwing errors
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

            //update the balance
            toAccount.Balance += amount;

            //logging transaction
            TransactionLog.LogTransaction(toAccount.AccNo, TransactionTypes.DEPOSIT, new Transaction(toAccount, amount));

            //Updating account  balance data to accounts table in our database
            accountRepo.Update(toAccount.AccNo, toAccount.Balance);

            return true;
        }


        public bool Withdraw(string AccNo, double amount, string pin)
        {
            //getting account details from database
            IAccount fromAccount = accountRepo.GetAccountByAccNo(AccNo);

            //validating details otherwise throw error
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

            if (fromAccount.Balance - amount < fromAccount.Policy.GetMinBalance())
            {
                throw new MinBalanceNeedsToBeMaintainedException($"Minimum balance needs to be maintained.\nMin balnce should be: {fromAccount.Policy.GetMinBalance()}");

            }

            // update the balance
            fromAccount.Balance -= amount;

            //Updating account  balance data to accounts table in our database
            accountRepo.Update(fromAccount.AccNo, fromAccount.Balance);

            //logging transaction
            TransactionLog.LogTransaction(fromAccount.AccNo, TransactionTypes.WITHDRAW, new Transaction(fromAccount, amount));

            return true;
        }

        public bool TransferFunds(string fromAccNo,string toAccNo,string pin,double amount)
        {
            //getting accounts details from database of from and to accounts
            IAccount fromAccount = accountRepo.GetAccountByAccNo(fromAccNo);
            IAccount toAccount = accountRepo.GetAccountByAccNo(toAccNo);

            Transfer transfer = new Transfer
            {
                FromAccount = fromAccount,
                ToAccount = toAccount,
                Pin = pin,
                Amount = amount
            };

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
                throw new MinBalanceNeedsToBeMaintainedException($"Minimum balance needs to be maintained.\nMin balance should be: {transfer.FromAccount.Policy.GetMinBalance()}");

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

            //Updating account  balance data to accounts table in our database after transfer
            if(accountRepo.FundTransfer(fromAccNo, toAccNo, amount))
                return true;
            else return false;
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

            //Updating account  balance data to accounts table in our database after transfer
            accountRepo.Update(externalTransfer.FromAccount.AccNo, externalTransfer.FromAccount.Balance);

            return true;
        }
    }

    

}
