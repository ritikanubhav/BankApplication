using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp5
{
    public interface IAccount
    {

        string AccNo { get; set; }
        string Name { get; set; }
        string Pin { get; set; }
        bool Active { get; set; }
        DateTime DateOfOpening { get; set; }
        double Balance { get; set; }
        PrivilegeType PrivilegeType { get; set; }
        IPolicy Policy { get; set; }

        bool Open();
        bool Close();
        string GetAccType();

    }
}
