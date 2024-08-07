using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    internal class Current : Account
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
