namespace ConsoleApp5
{
    public class ExternalAccount
    {
        public string AccNo { get;  set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public ExternalAccount() { }

        public ExternalAccount(string accNo, string bankCode, string bankName)
        {
            AccNo = accNo;
            BankCode = bankCode;
            BankName = bankName;
        }
    }

}
