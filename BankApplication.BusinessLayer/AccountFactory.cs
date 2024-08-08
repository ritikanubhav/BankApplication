using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using BankApplication.Common;
using BankApplication.DataAccess;
namespace BankApplication.BusinessLayer
{
    public class AccountFactory
    {
        public static IAccount CreateAccount(string name, string pin, double balance, PrivilegeType privilegeType, AccountType accType)
        {
            IAccount account;
            switch (accType)
            {
                case AccountType.SAVINGS:
                    account = new Saving(name,pin,privilegeType,balance);
                    break;
                case AccountType.CURRENT:
                    account = new Current(name, pin, privilegeType, balance);
                    break;
                default:
                    throw new InvalidAccountTypeException("Invalid account type.");
            }
            account.Open();

            return account;
        }
    }
}
    
