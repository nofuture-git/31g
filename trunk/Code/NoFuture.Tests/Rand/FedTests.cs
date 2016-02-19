using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class FedTests
    {
        [TestMethod]
        public void TestParseBankNames()
        {
            var testFile = System.IO.File.ReadAllText(@"C:\Projects\31g\trunk\Code\NoFuture.Tests\Rand\lrg_bnk_lst.txt");
            DateTime? rptDt = null;
            var testResult = NoFuture.Rand.Gov.Fed.LargeCommercialBanks.ParseBankData(testFile, out rptDt);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0,testResult.Count);
            System.Diagnostics.Debug.WriteLine(rptDt);
            foreach (var nm in testResult)
            {
                System.Diagnostics.Debug.WriteLine(nm.Item1);
                System.Diagnostics.Debug.WriteLine(nm.Item2);
                System.Diagnostics.Debug.WriteLine("-----");
            }
        }

        [TestMethod]
        public void TestRoutingTransitNumber()
        {
            var testSubject = NoFuture.Rand.Gov.Fed.RoutingTransitNumber.RandomRoutingNumber();
            Assert.IsNotNull(testSubject);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testSubject.Value));
            Assert.IsFalse(string.IsNullOrWhiteSpace(testSubject.FedDistrictFullName));
            Assert.AreEqual(9, testSubject.Value.Length);

            System.Diagnostics.Debug.WriteLine(testSubject.Value);
            System.Diagnostics.Debug.WriteLine(testSubject.FedDistrictFullName);

            testSubject.Value = "053000196";

            Assert.AreEqual("05", testSubject.FedDistrict);
            Assert.AreEqual("30", testSubject.CheckProcCenter);
            Assert.AreEqual("0019", testSubject.AbaInstitutionId);
            Assert.AreEqual(6, testSubject.CheckDigit);
        }

        [TestMethod]
        public void TestTryParseFfiecInstitutionProfileAspxHtml()
        {
            var testContent = System.IO.File.ReadAllText(@"C:\Temp\ffiec.html");

            NoFuture.Rand.Com.FinancialFirm firmOut;
            var testResult = NoFuture.Rand.Gov.Fed.Ffiec.TryParseFfiecInstitutionProfileAspxHtml(testContent,
                out firmOut);

            Assert.IsTrue(testResult);

        }
    }
}
