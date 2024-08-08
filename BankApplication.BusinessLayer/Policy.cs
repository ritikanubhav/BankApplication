using BankApplication.DataAccess;

using BankApplication.Common;
namespace BankApplication.BusinessLayer
{
    public class Policy : IPolicy
    {
        public double MinBalance { get; private set; }
        public double RateOfInterest { get; private set; }

        public Policy(double minBalance, double rateOfInterest)
        {
            MinBalance = minBalance;
            RateOfInterest = rateOfInterest;
        }

        public double GetMinBalance() => MinBalance;
        public double GetRateOfInterest() => RateOfInterest;
    }

}
