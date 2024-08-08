namespace BankApplication.Common
{
    public interface IPolicy
    {
        double GetMinBalance();
        double GetRateOfInterest();
    }

}
