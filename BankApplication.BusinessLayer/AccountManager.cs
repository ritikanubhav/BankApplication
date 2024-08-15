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

        //creating Transactionrepo object of dataAccess layer to implement transaction table methods
        BankApplicationDbTransactionRepo transRepo = new BankApplicationDbTransactionRepo();

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

            //update the balance in account instance
            toAccount.Balance += amount;

            //Updating account balance to accounts table in our database
            if(accountRepo.Update(toAccount))
            {
                //logging transaction
                TransactionLog.LogTransaction(toAccount.AccNo, TransactionTypes.DEPOSIT, new Transaction(toAccount.AccNo, amount));

                //inserting Transaction in database
                transRepo.InsertTransaction(new Transaction(toAccount.AccNo, amount),TransactionTypes.DEPOSIT);
                return true;
            }
            return false;
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

            //Updating account balance data to accounts table in our database
            if (accountRepo.Update(fromAccount))
            {
                //logging transaction
                TransactionLog.LogTransaction(fromAccount.AccNo, TransactionTypes.WITHDRAW, new Transaction(fromAccount.AccNo, amount));

                //inserting Transaction to the database
                transRepo.InsertTransaction(new Transaction(fromAccount.AccNo, amount), TransactionTypes.WITHDRAW);
                return true;
            }
            return false;
        }

        public bool TransferFunds(string fromAccNo,string toAccNo,string pin,double amount)
        {
            //getting accounts details from database of from and to accounts
            IAccount fromAccount = accountRepo.GetAccountByAccNo(fromAccNo);
            IAccount toAccount = accountRepo.GetAccountByAccNo(toAccNo);

            //creating a transfer object
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

            //update the balance in accounts
            transfer.FromAccount.Balance -= transfer.Amount;
            transfer.ToAccount.Balance += transfer.Amount;

            //Updating account balance data to accounts table in our database after transfer
            //and checking if it is successful
            if(accountRepo.FundTransfer(fromAccNo, toAccNo, amount))
            {
                //logging Transactions
                int transactionID = IDGenerator.GenerateID();

                Transaction withdrawTransaction = new Transaction(transfer.FromAccount.AccNo, transfer.Amount);
                Transaction depositTransaction = new Transaction(transfer.ToAccount.AccNo, transfer.Amount);

                TransactionLog.LogTransaction(transfer.FromAccount.AccNo, TransactionTypes.WITHDRAW, withdrawTransaction);
                TransactionLog.LogTransaction(transfer.ToAccount.AccNo, TransactionTypes.DEPOSIT, depositTransaction);

                // inserting transaction log to database
                transRepo.InsertTransaction(withdrawTransaction, TransactionTypes.TRANSFER);
                return true;
            }
            return false;
        }

        public bool ExternalTransferFunds(ExternalTransfer externalTransfer)
        {
            //getting fromAccount object from database and validating first
            IAccount fromAccount = accountRepo.GetAccountByAccNo(externalTransfer.FromAccount);

            if (fromAccount == null)
                throw new AccountDoesNotExistException($"No such Account exist with account No:{externalTransfer.FromAccount}");

            if (!fromAccount.Active)
                throw new InactiveAccountException("Inactive account");

            if (fromAccount.Pin != externalTransfer.FromAccPin)
                throw new InvalidPinException("Invalid Pin");

            if (fromAccount.Balance < externalTransfer.Amount)
                throw new InsufficientBalanceException("Insufficient Balnace");

            if (fromAccount.Balance - externalTransfer.Amount < fromAccount.Policy.GetMinBalance())
                throw new MinBalanceNeedsToBeMaintainedException("Minimum balance needs to be maintained.");

            // Check if the transaction is within permissible daily limits
            double dailyLimit = AccountPrivilegeManager.GetDailyLimit(fromAccount.PrivilegeType);
            double totalTransferredAmountToday = TransactionLog.GetTotalTransferredAmountToday(fromAccount.AccNo);

            if (totalTransferredAmountToday + externalTransfer.Amount > dailyLimit)
                throw new DailyLimitExceededException();
            
            //updating the balance of transferrer
             fromAccount.Balance -= externalTransfer.Amount;

            //Updating account  balance data in accounts table in our database after transfer
            if (accountRepo.Update(fromAccount))
            {
                // Assuming success in external transfer, log transaction and update status
                TransactionLog.LogTransaction(fromAccount.AccNo, TransactionTypes.EXTERNALTRANSFER, externalTransfer);
                externalTransfer.Status = TransactionStatus.OPEN;

                //Deposit the amount to external bank---
                //step 1.create an instance of externalservicefactory to use getservice method
                ExternalBankServiceFactory externalBankServiceFactory=ExternalBankServiceFactory.Instance;

                //step 2. generate bankserice of the extenal bank using bankcode
                IExternalBankService externalBankService =externalBankServiceFactory.GetService(externalTransfer.ToExternalAccount.BankCode);

                //step 3.call the deposit function of the extenalbank
                externalBankService.deposit(externalTransfer.ToExternalAccount.AccNo,externalTransfer.Amount);

                //Inserting Transaction into transaction table in database
                transRepo.InsertTransaction(externalTransfer, TransactionTypes.EXTERNALTRANSFER);
                return true;
            }
            return false;
        }
    }

    

}
