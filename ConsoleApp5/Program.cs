using BankApplication.BusinessLayer;
using BankApplication.Common;
//using BankApplication.DataAccess;
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
                Console.WriteLine("6. Exit");
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
                            bool isDepositSuccess=accountManager.Deposit(accNo, depositAmount);
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
                        
                            bool isWithdrawSuccessful = accountManager.Withdraw(withdrawAccNo, withdrawAmount,withdrawPin);

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
                        catch(Exception ex)
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
                            IAccount extFromAcc = null;
                            extFromAcc.AccNo=extFromAccNo;
                            ExternalTransfer extTransfer = new ExternalTransfer(extFromAcc, extTransferAmount, new ExternalAccount
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
                            Console.WriteLine($"Error: {ex.Message}");
                            logger.Error(ex, "Error during External Transfer");
                        }
                        break;

                    case "6":
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
