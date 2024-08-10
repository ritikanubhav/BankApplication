
using BankApplication.Common;
namespace BankApplication.BusinessLayer
{
    public static class AccountPrivilegeManager
    {
        private static readonly Dictionary<PrivilegeType, double> dailyLimits = new Dictionary<PrivilegeType, double>();

        static AccountPrivilegeManager()
        {
            try
            {
                LoadDailyLimits();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void LoadDailyLimits()
        {
            foreach (var line in File.ReadAllLines("dailyLimits.properties"))
            {
                var parts = line.Split('=');
                if (Enum.TryParse(parts[0], out PrivilegeType privilegeType))
                {
                    dailyLimits[privilegeType] = double.Parse(parts[1]);
                }
            }
        }

        public static double GetDailyLimit(PrivilegeType privilegeType)
        {
            if (!dailyLimits.ContainsKey(privilegeType))
                throw new InvalidPrivilegeTypeException();

            return dailyLimits[privilegeType];
        }
    }
}

