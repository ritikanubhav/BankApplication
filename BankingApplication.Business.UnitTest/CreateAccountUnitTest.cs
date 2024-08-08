using BankApplication.BusinessLayer;
using BankApplication.Common;
namespace BankingApplication.Business.UnitTest
{
    [TestClass]
    public class AccountManagerTests
    {
        public AccountManager accountManager;

        [TestInitialize]
        public void Setup()
        {
            accountManager = new AccountManager();
        }

        [TestMethod]
        public void CreateAccount_ShouldCreateSavingsAccount()
        {
            // Arrange
            string name = "John";
            string pin = "1234";
            double balance = 6000.0;
            PrivilegeType privilegeType = PrivilegeType.REGULAR;
            AccountType accType = AccountType.SAVINGS;

            // Act
            IAccount account = accountManager.CreateAccount(name, pin, balance, privilegeType, accType);

            // Assert
            Assert.IsNotNull(account);
            Assert.AreEqual(name, account.Name);
            Assert.AreEqual(pin, account.Pin);
            Assert.AreEqual(balance, account.Balance);
            Assert.AreEqual(privilegeType, account.PrivilegeType);
            Assert.AreEqual("SAVINGS", account.GetAccType());
            Assert.IsTrue(account.Active);
        }

        [TestMethod]
        [ExpectedException(typeof(MinBalanceNeedsToBeMaintainedException))]
        public void CreateAccount_ShouldThrowMinBalanceNeedsToBeMaintainedException()
        {
            // Arrange
            string name = "Jane";
            string pin = "5678";
            double balance = 4000.0; // Less than minimum balance for REGULAR savings account
            PrivilegeType privilegeType = PrivilegeType.REGULAR;
            AccountType accType = AccountType.SAVINGS;

            // Act
            accountManager.CreateAccount(name, pin, balance, privilegeType, accType);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAccountTypeException))]
        public void CreateAccount_ShouldThrowInvalidAccountTypeException()
        {
            // Arrange
            string name = "Jan";
            string pin = "5678";
            double balance = 10000.0;
            PrivilegeType privilegeType = PrivilegeType.REGULAR;
            AccountType accType = (AccountType)999; // Invalid account type

            // Act
            accountManager.CreateAccount(name, pin, balance, privilegeType, accType);
        }
    }
}