using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApplication.DataAccess;

using BankApplication.Common;
namespace BankApplication.BusinessLayer
{
    public class Transfer
    {
        public IAccount FromAccount { get; set; }
        public IAccount ToAccount { get; set; }
        public double Amount { get; set; }
        public string Pin { get; set; }

    }
}
