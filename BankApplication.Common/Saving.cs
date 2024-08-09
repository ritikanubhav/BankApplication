using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication.Common
{
    public class Saving:Account
    {
        public Saving()
        {
        }
        public Saving(string name, string pin, PrivilegeType privilegeType, double Balance) : this()
        {
            this.Name = name;
            this.Pin = pin;
            this.PrivilegeType = privilegeType;
            this.Balance = Balance;
        }
        public override string GetAccType()
        {
            return "SAVINGS";
        }
    }
}
