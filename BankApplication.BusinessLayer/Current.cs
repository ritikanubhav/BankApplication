using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using BankApplication.DataAccess;

using BankApplication.Common;
namespace BankApplication.BusinessLayer
{
    public class Current : Account
    {
        public Current() {
        }
        public Current(string name, string pin, PrivilegeType privilegeType, double Balance):this()
        {
            this.Name = name;
            this.Pin = pin;
            this.PrivilegeType = privilegeType;
            this.Balance = Balance;
        }
        public override string GetAccType()
        {
            return "CURRENT";
        }

    }
}
