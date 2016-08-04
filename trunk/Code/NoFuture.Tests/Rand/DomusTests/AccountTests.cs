using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Tests.Rand.DomusTests
{
    [TestClass]
    public class AccountTests
    {
        [TestInitialize]
        public void Init()
        {
            BinDirectories.DataRoot = @"C:\Projects\31g\trunk\bin\Data\Source";
        }
        [TestMethod]
        public void TestGetRandomBankAccount()
        {
            var testResult = Checking.GetRandomCheckingAcct(new UsCityStateZip(new AddressData {City = "New York", StateAbbrv = "NY"}));

            Assert.IsNotNull(testResult);

            Assert.IsNotNull(testResult.AccountNumber);

            System.Diagnostics.Debug.WriteLine(testResult.AccountNumber.Value);

            
            testResult = Checking.GetRandomCheckingAcct(new UsCityStateZip(new AddressData { City = "New York", StateAbbrv = "NY" }));
            Assert.IsNotNull(testResult.AccountNumber);
            Assert.IsNotNull(testResult.Bank);

            System.Diagnostics.Debug.WriteLine(testResult.AccountNumber.Value);
            System.Diagnostics.Debug.WriteLine(testResult.Bank.Name);
            
        }
    }
}
