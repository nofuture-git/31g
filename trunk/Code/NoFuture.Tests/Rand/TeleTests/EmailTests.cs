using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Tele;

namespace NoFuture.Rand.Tests.TeleTests
{
    [TestClass]
    public class EmailTests
    {
        [TestMethod]
        public void TestRandomEmailUri_UsCommon()
        {
            var testResult = Tele.Email.GetRandomEmail("booty", true);
            Assert.IsNotNull(testResult);
            var testResultParts = testResult.Value.Split('@');
            Assert.AreEqual(2, testResultParts.Length);
            Assert.IsTrue(Net.UsWebmailDomains.Contains(testResultParts[1]));
            Assert.AreEqual("booty", testResultParts[0]);
            System.Diagnostics.Debug.WriteLine(testResult);
        }


        [TestMethod]
        public void TestRandomEmailUriPersonal()
        {
            var testResult = Email.GetRandomEmail(true, "Robert", "Edward", "Lee");
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Value.Contains("lee"));
            System.Diagnostics.Debug.WriteLine(testResult);

            testResult = Email.GetRandomEmail(false, "Robert", "Edward", "Lee");
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestRandomEmailUri()
        {
            var testResult = Email.GetRandomEmail(null);
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Value));
            System.Diagnostics.Debug.WriteLine(testResult.ToUri());

        }

        [TestMethod]
        public void TestGetChildishRandomEmail()
        {
            var testResult = Email.GetChildishEmailAddress();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Value));
            System.Diagnostics.Debug.WriteLine(testResult.ToString());

        }
    }
}
