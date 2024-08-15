using System;
using BankApplication.BusinessLayer;
using BankApplication.Common;
namespace ConsoleApp5
{
    public class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main()
        {
            AccountManager accountManager = new AccountManager();
            IAccount account1 = null;
            IAccount account2 = null;
            Console.WriteLine("----------------------------------------------------------------------------------");
            Console.WriteLine("                          WELCOME TO BANK OF CONSILIO!!");
            Console.WriteLine("----------------------------------------------------------------------------------");
            while (true)
            {
                Console.WriteLine("\n                            Bank Application Menu:");
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Transfer Funds");
                Console.WriteLine("5. External Transfer");
                Console.WriteLine("6. Generate Reports");
                Console.WriteLine("7. Exit");
                Console.WriteLine("----------------------------------------------------------------------------------");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        try
                        {
                            Console.WriteLine("Select Account Type (0: SAVINGS, 1: CURRENT): ");
                            AccountType accType = (AccountType)int.Parse(Console.ReadLine());

                            Console.Write("Enter Name: ");
                            string name = Console.ReadLine();

                            Console.Write("Set Your PIN: ");
                            string pin = Console.ReadLine();

                            Console.WriteLine("Select Privilege Type (0: REGULAR, 1: GOLD, 2: PREMIUM): ");
                            PrivilegeType privilegeType = (PrivilegeType)int.Parse(Console.ReadLine());

                            Console.Write("Enter Initial Balance: ");
                            double balance = double.Parse(Console.ReadLine());

                            IAccount newAccount = accountManager.CreateAccount(name, pin, balance, privilegeType, accType);

                            Console.WriteLine("----------------------------------------------------------------------------------");
                            Console.WriteLine($"Account created successfully! Account Number: {newAccount.AccNo}");
                            Console.WriteLine("----------------------------------------------------------------------------------");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            Console.WriteLine($"Error: {ex.Message}");
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            logger.Error(ex, "Error during Creating Account:");
                        }
                        break;

                    case "2":
                        try
                        {
                            Console.Write("Enter Account Number: ");
                            string accNo = Console.ReadLine().ToUpper();
                            Console.Write("Enter Amount to Deposit: ");
                            double depositAmount = double.Parse(Console.ReadLine());
                            bool isDepositSuccess = accountManager.Deposit(accNo, depositAmount);
                            if (isDepositSuccess)
                            {
                                Console.WriteLine("----------------------------------------------------------------------------------");
                                Console.WriteLine("Deposit successful!");
                                Console.WriteLine("----------------------------------------------------------------------------------");
                            }
                            else
                            {
                                Console.WriteLine("----------------------------------------------------------------------------------");
                                Console.WriteLine("Error in depositing.");
                                Console.WriteLine("----------------------------------------------------------------------------------");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            Console.WriteLine($"Error: {ex}");
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            logger.Error(ex, "Error during Deposit");
                        }
                        break;

                    case "3":
                        try
                        {
                            Console.Write("Enter Account Number: ");
                            string withdrawAccNo = Console.ReadLine().ToUpper();
                            Console.Write("Enter PIN: ");
                            string withdrawPin = Console.ReadLine();
                            Console.Write("Enter Amount to Withdraw: ");
                            double withdrawAmount = double.Parse(Console.ReadLine());

                            bool isWithdrawSuccessful = accountManager.Withdraw(withdrawAccNo, withdrawAmount, withdrawPin);

                            if (isWithdrawSuccessful)
                            {
                                Console.WriteLine("----------------------------------------------------------------------------------");
                                Console.WriteLine("Withdraw successful!");
                                Console.WriteLine("----------------------------------------------------------------------------------");
                            }
                            else
                            {
                                Console.WriteLine("----------------------------------------------------------------------------------");
                                Console.WriteLine("Error in Withdrawal.");
                                Console.WriteLine("----------------------------------------------------------------------------------");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            Console.WriteLine($"Error: {ex.Message}");
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            logger.Error(ex, "Error during Deposit");
                        }
                        break;

                    case "4":
                        try
                        {
                            Console.Write("Enter From Account Number: ");
                            string fromAccNo = Console.ReadLine().ToUpper();
                            Console.Write("Enter To Account Number: ");
                            string toAccNo = Console.ReadLine().ToUpper();
                            Console.Write("Enter PIN: ");
                            string transferPin = Console.ReadLine();
                            Console.Write("Enter Amount to Transfer: ");
                            double transferAmount = double.Parse(Console.ReadLine());
                            bool isTransferSuccesful = accountManager.TransferFunds(fromAccNo, toAccNo, transferPin, transferAmount);
                            if (isTransferSuccesful)
                            {
                                Console.WriteLine("----------------------------------------------------------------------------------");
                                Console.WriteLine("Transfer successful!");
                            }
                            else
                            {
                                Console.WriteLine("----------------------------------------------------------------------------------");
                                Console.WriteLine("Transfer Unsuccesful");
                                Console.WriteLine("----------------------------------------------------------------------------------");
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            Console.WriteLine($"Error During Transferring Funds: {ex.Message}");
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            logger.Error(ex, "Error During TransferFunds");
                        }
                        break;

                    case "5":
                        try
                        {
                            Console.Write("Enter From Account Number: ");
                            string extFromAccNo = Console.ReadLine().ToUpper();
                            Console.Write("Enter From Account PIN: ");
                            string extFromAccPin = Console.ReadLine();
                            Console.Write("Enter Amount to Transfer: ");
                            double extTransferAmount = double.Parse(Console.ReadLine());
                            Console.Write("Enter External Account Number: ");
                            string extToAccNo = Console.ReadLine().ToUpper();
                            Console.Write("Enter External Bank Code: ");
                            string extBankCode = Console.ReadLine().ToUpper();
                            Console.Write("Enter External Bank Name: ");
                            string extBankName = Console.ReadLine().ToUpper();

                            ExternalTransfer extTransfer = new ExternalTransfer(extFromAccNo, extTransferAmount, new ExternalAccount
                            {
                                AccNo = extToAccNo,
                                BankCode = extBankCode,
                                BankName = extBankName
                            }, extFromAccPin);

                            accountManager.ExternalTransferFunds(extTransfer);
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            Console.WriteLine("External Transfer successful!");
                            Console.WriteLine("----------------------------------------------------------------------------------");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            Console.WriteLine($"Error: {ex.Message}");
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            logger.Error(ex, "Error during External Transfer");
                        }
                        break;

                    case "6":
                        try
                        {
                            bool exit = false;
                            while (!exit)
                            {
                                Console.WriteLine("----------------------------------------------------------------------------------");
                                Console.WriteLine("\n                            Report Generator Menu:");
                                Console.WriteLine("----------------------------------------------------------------------------------");
                                Console.WriteLine("01. Print All Log Transactions");
                                Console.WriteLine("02. Print All Log Transactions for an accountId");
                                Console.WriteLine("03. Print All Log Transactions for a Transaction Type");
                                Console.WriteLine("04. Print Total No Of Accounts");
                                Console.WriteLine("05. Display No Of Accounts Type Wise");
                                Console.WriteLine("06. Display Total Worth Of Bank");
                                Console.WriteLine("07. Display Policy Informations");
                                Console.WriteLine("08. Print All Transfers");
                                Console.WriteLine("09. Print All WithDrawals");
                                Console.WriteLine("10. Print All Deposits");
                                Console.WriteLine("11. Print All Transactions For Today");
                                Console.WriteLine("12. Back to Main Menu");
                                Console.WriteLine("----------------------------------------------------------------------------------");
                                Console.Write("Enter your choice: ");
                                string resGenChoice = Console.ReadLine();
                                switch (resGenChoice)
                                {
                                    case "1":
                                        ResultGenerator.PrintAllLogTransactions();
                                        break;
                                    case "2":
                                        Console.WriteLine("Enter Account No:");
                                        string accountId = Console.ReadLine().ToUpper();
                                        ResultGenerator.PrintAllLogTransactions(accountId);
                                        break;
                                    case "3":
                                        Console.WriteLine("Choose a Transaction Type:\n1.Deposit\n2.Withdraw\n3.Transfer\n4.External Transfer");
                                        Console.WriteLine("Enter Your Choice:");
                                        string transType = Console.ReadLine();
                                        switch(transType)
                                        {
                                            case "1":
                                                ResultGenerator.PrintAllLogTransactions(TransactionTypes.DEPOSIT);
                                                break;
                                            case "2":
                                                ResultGenerator.PrintAllLogTransactions(TransactionTypes.WITHDRAW);
                                                break;
                                            case "3":
                                                ResultGenerator.PrintAllLogTransactions(TransactionTypes.TRANSFER);
                                                break;
                                            case "4":
                                                ResultGenerator.PrintAllLogTransactions(TransactionTypes.EXTERNALTRANSFER);
                                                break;
                                            default:
                                                Console.WriteLine("Invalid Choice");
                                                break;
                                        }
                                        break;
                                    case "4":
                                        int totalAcc = ResultGenerator.GetTotalNoOfAccounts();
                                        Console.WriteLine($"\nTotal No of Accounts opened till now : {totalAcc}");
                                        break;
                                    case "5":
                                        ResultGenerator.DisplayNoOfAccTypeWise();
                                        break;
                                    case "6":
                                        ResultGenerator.DispTotalWorthOfBank();
                                        break;
                                    case "7":
                                        ResultGenerator.DispPolicyInfo();
                                        break;
                                    case "8":
                                        ResultGenerator.PrintAllTransfers();
                                        break;
                                    case "9":
                                        ResultGenerator.PrintAllWithdrawals();
                                        break;
                                    case "10":
                                        ResultGenerator.PrintAllDeposits();
                                        break;
                                    case "11":
                                        ResultGenerator.PrintAllTransactionsForToday();
                                        break;
                                    case "12":
                                        exit=true;
                                        break;
                                    default:
                                        Console.WriteLine("----------------------------------------------------------------------------------");
                                        Console.WriteLine("Invalid choice. Please try again.");
                                        Console.WriteLine("----------------------------------------------------------------------------------");
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            Console.WriteLine($"Error: {ex.Message}");
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            logger.Error(ex, "Error during Generating Result");
                        }
                        break;

                    case "7":
                        Console.WriteLine("----------------------------------------------------------------------------------");
                        Console.WriteLine("Exiting application.");
                        Console.WriteLine("----------------------------------------------------------------------------------");
                        return;

                    default:
                        Console.WriteLine("----------------------------------------------------------------------------------");
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.WriteLine("----------------------------------------------------------------------------------");
                        break;

                }
            }
        }
    }
}
