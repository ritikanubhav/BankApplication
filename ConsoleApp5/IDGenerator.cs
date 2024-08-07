using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    public static class IDGenerator
    {

        private static int id = 1000;

        public static string GenerateAccNo(string accType)
        {
            return $"{accType.Substring(0, 3).ToUpper()}{id++}";
        }

        public static int GenerateID()
        {
            return id++;
        }
    }
}
