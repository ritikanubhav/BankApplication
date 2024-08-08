using BankApplication.Common;
namespace BankApplication.BusinessLayer
{
    public class ICICIBankService : IExternalBankService
    {
        public bool deposit(String accId, double amt) 
        {
            // Write ICICIBank specific code to deposit
            // Assume the logic to update the ICICIBANK table in the database
            // Return true if the deposit is successful, otherwise throw an AccountDoesNotExistException
            return true;
        }
    }

}
