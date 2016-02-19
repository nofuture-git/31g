using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Com;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class SecTests
    {

        public const string SEC_BY_SIC_XML_PATH = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Rand\SecBySIC.xml";
        public const string SEC_BY_CIK_XML_PATH = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Rand\SecByCIK.xml";
        public const string SEC_BY_FULLTEXT_XML_PATH = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Rand\SecByFullText.xml";

        [TestMethod]
        public void TestTryGetCorpData()
        {
            var xmlContent = System.IO.File.ReadAllText(SEC_BY_CIK_XML_PATH);

            var testResultOut = new PublicCorporation();
            var testResult = NoFuture.Rand.Gov.Sec.Edgar.TryParseCorpData(xmlContent, ref testResultOut);

            Assert.IsTrue(testResult);
            Assert.IsNotNull(testResultOut);

            Assert.AreEqual("0000717538", testResultOut.CIK.Value);
            Assert.AreEqual("6021", testResultOut.SIC.Value);
            Assert.AreEqual("ARROW FINANCIAL CORP", testResultOut.Name);
            
            Assert.IsNotNull(testResultOut.MailingAddress);
            Assert.IsNotNull(testResultOut.BusinessAddress);

            Assert.AreEqual("250", testResultOut.BusinessAddress.Item1.PostBox);
            Assert.AreEqual("GLEN", testResultOut.BusinessAddress.Item1.StreetName);
            Assert.AreEqual("ST", testResultOut.BusinessAddress.Item1.StreetKind);

            Assert.AreEqual("250", testResultOut.MailingAddress.Item1.PostBox);
            Assert.AreEqual("GLEN STREET", testResultOut.MailingAddress.Item1.StreetName);

            Assert.AreEqual("GLENS FALLS", testResultOut.BusinessAddress.Item2.City);
            Assert.AreEqual("NY", testResultOut.BusinessAddress.Item2.PostalState);
            Assert.AreEqual("12801", testResultOut.BusinessAddress.Item2.ZipCode);

            Assert.AreEqual("GLENS FALLS", testResultOut.MailingAddress.Item2.City);
            Assert.AreEqual("NY", testResultOut.MailingAddress.Item2.PostalState);
            Assert.AreEqual("12801", testResultOut.MailingAddress.Item2.ZipCode);

            Assert.IsNotNull(testResultOut.Phone);
            Assert.IsNotNull(testResultOut.Phone[0]);
            Assert.AreEqual("518", testResultOut.Phone[0].AreaCode);
            Assert.AreEqual("415", testResultOut.Phone[0].CentralOfficeCode);
            Assert.AreEqual("4299", testResultOut.Phone[0].SubscriberNumber);

        }

        [TestMethod]
        public void TestParseCompanyFullTextSearch()
        {
            var xmlContent = System.IO.File.ReadAllText(SEC_BY_FULLTEXT_XML_PATH);
            var testResult = NoFuture.Rand.Gov.Sec.Edgar.ParseFullTextSearch(xmlContent);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach (var r in testResult)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Name: {0}; CIK: {1}", r.Name, r.CIK.Value));
                foreach (var ar in r.AnnualReports)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("IsLate: {2}; Accession Number: {0}; Link: {1}",
                        ar.AccessionNumber, ar.InteractiveFormLink, ar.IsLate));
                    System.Diagnostics.Debug.WriteLine(string.Format("HtmlLink: {0}",ar.HtmlFormLink));

                }
                System.Diagnostics.Debug.WriteLine("----------");
            }
            
        }

        [TestMethod]
        public void TestParseAccessionNumFromSummary()
        {
            var testResult =
                NoFuture.Rand.Gov.Sec.Edgar.ParseAccessionNumFromSummary(
                    "<b>Filed Date:</b> 04/15/2015 <b>Accession Number:</b> 0001549727-15-000033 <b>Size:</b> 3 MB");
            Assert.IsNotNull(testResult);
            Assert.AreEqual("0001549727-15-000033", testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestCtorInteractiveLink()
        {
            const string cik = "805729";
            const string acc = "0001549727-15-000033";
            var expectedRslt =
                new Uri("http://www.sec.gov/cgi-bin/viewer?action=view&cik=" + cik + "&accession_number=" + acc +
                        "&xbrl_type=v");
            var testResult = NoFuture.Rand.Gov.Sec.Edgar.CtorInteractiveLink(cik, acc);
            Assert.AreEqual(expectedRslt, testResult);
        }

        [TestMethod]
        public void TestFormattedAccessionNumber()
        {
            var testSubject = new NoFuture.Rand.Gov.Sec.Form10K();
            testSubject.AccessionNumber = "000152013815000247";
            var testResult = testSubject.FormattedAccessionNumber;
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("0001520138-15-000247", testResult);
        }

        [TestMethod]
        public void TestGetUriFullTextSearch()
        {
            var testInput = new NoFuture.Rand.Gov.Sec.Edgar.FullTextSearch();
            testInput.CompanyName = "CITIBANK, N.A.";

            var testResult = NoFuture.Rand.Gov.Sec.Edgar.GetUriFullTextSearch(testInput);

            System.Diagnostics.Debug.WriteLine(testResult.ToString());
        }
    }
}
