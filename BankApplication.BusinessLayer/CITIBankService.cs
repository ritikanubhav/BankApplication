using BankApplication.DataAccess;

using BankApplication.Common;
namespace BankApplication.BusinessLayer
{
    public class CITIBankService : IExternalBankService
    {
        public bool deposit(String accId, double amt) 
        {
            // Write CITIBank specific code to deposit
            // Assume the logic to update the CITIBANK table in the database
            // Return true if the deposit is successful, otherwise throw an AccountDoesNotExistException
            return true;
        }
    }

}
