using BankApplication.Common;
using BankApplication.DataAccess;
namespace BankApplication.BusinessLayer
{
    public class ICICIBankService : IExternalBankService
    {
        public bool deposit(String accId, double amt) 
        {
            // Write ICICIBank specific code to deposit
            // Assume the logic to update the ICICIBANK table in the database
            // Return true if the deposit is successful, otherwise throw an AccountDoesNotExistException
            try
            {
                BankApplicationExternalBankDbRepo externalBankDbRepo = new BankApplicationExternalBankDbRepo();
                externalBankDbRepo.InsertIntoExternalBank(accId,amt, "ICICIBANK");
                return true;
            }
            catch (Exception ex) 
            {
                throw new AccountDoesNotExistException("Account Does not exist--" + ex.Message);
            }
        }
    }

}
