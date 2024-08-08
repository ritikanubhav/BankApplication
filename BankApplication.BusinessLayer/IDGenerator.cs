using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using BankApplication.Common;
namespace BankApplication.BusinessLayer
{
    public  class IDGenerator
    {
        private static readonly string filePath = "CurrentID.txt";

        public static string GenerateAccNo(string accType)
        {
            return $"{accType.Substring(0, 3).ToUpper()}{GenerateID()}";
        }

        public static int GenerateID()
        {
            int currentID = ReadCurrentID();
            int newID = currentID + 1;
            WriteNewID(newID);
            return newID;
        }

        private static int ReadCurrentID()
        {
            if (!File.Exists(filePath))
            {
                // Create the file with an initial ID if it doesn't exist
                File.WriteAllText(filePath, "1001");
            }

            string idStr = File.ReadAllText(filePath);
            if (int.TryParse(idStr, out int currentID))
            {
                return currentID;
            }
            else
            {
                throw new Exception("Invalid ID in file.");
            }
        }

        private static void WriteNewID(int newID)
        {
            File.WriteAllText(filePath, newID.ToString());
        }
    }
}
