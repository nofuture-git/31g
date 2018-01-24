using System.Linq;
using NUnit.Framework;
using NoFuture.Rand.Tele;

namespace NoFuture.Rand.Tests.TeleTests
{
    [TestFixture]
    public class EmailTests
    {
        [Test]
        public void TestRandomEmailUri_UsCommon()
        {
            var testResult = Tele.Email.RandomEmail("booty", true);
            Assert.IsNotNull(testResult);
            var testResultParts = testResult.Value.Split('@');
            Assert.AreEqual(2, testResultParts.Length);
            Assert.IsTrue(Net.UsWebmailDomains.Contains(testResultParts[1]));
            Assert.AreEqual("booty", testResultParts[0]);
            System.Diagnostics.Debug.WriteLine(testResult);
        }


        [Test]
        public void TestRandomEmailUriPersonal()
        {
            var testResult = Email.RandomEmail(true, "Robert", "Edward", "Lee");
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Value.Contains("lee"));
            System.Diagnostics.Debug.WriteLine(testResult);

            testResult = Email.RandomEmail(false, "Robert", "Edward", "Lee");
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [Test]
        public void TestRandomEmailUri()
        {
            var testResult = Email.RandomEmail(null);
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Value));
            System.Diagnostics.Debug.WriteLine(testResult.ToUri());

        }

        [Test]
        public void TestGetChildishRandomEmail()
        {
            var testResult = Email.RandomChildishEmail();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Value));
            System.Diagnostics.Debug.WriteLine(testResult.ToString());

        }
    }
}
