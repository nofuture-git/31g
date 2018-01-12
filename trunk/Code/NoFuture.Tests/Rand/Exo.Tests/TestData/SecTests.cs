using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Exo;
using NoFuture.Rand.Data.Exo.NfXml;
using NoFuture.Rand.Exo.Tests;
using NoFuture.Rand.Gov.US.Sec;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class SecTests
    {

        public string SEC_BY_SIC_XML_PATH = TestAssembly.UnitTestsRoot + @"\Rand\SecBySIC.xml";
        public string SEC_BY_CIK_XML_PATH = TestAssembly.UnitTestsRoot + @"\Rand\SecByCIK.xml";
        public string SEC_BY_FULLTEXT_XML_PATH = TestAssembly.UnitTestsRoot + @"\Rand\SecByFullText.xml";

        [TestMethod]
        public void TestTryGetCorpData()
        {
            var xmlContent = System.IO.File.ReadAllText(SEC_BY_CIK_XML_PATH);

            var testResultOut = new PublicCorporation();
            var testResult = Copula.TryParseSecEdgarCikSearch(xmlContent,
                new Uri(
                    "http://www.sec.gov/cgi-bin/browse-edgar?action=getcompany&CIK=041215216456&type=10-K&dateb=&owner=exclude&count=100&output=atom"),
                ref testResultOut);

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
            //Assert.AreEqual("GLEN STREET", testResultOut.MailingAddress.Item1.StreetName);

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
            var testResult = Copula.ParseSecEdgarFullTextSearch(xmlContent, new Uri("http://www.sec.gov/cgi-bin/srch-edgar"));
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach (var r in testResult)
            {
                System.Diagnostics.Debug.WriteLine($"Name: {r.Name}; CIK: {r.CIK.Value}");
                foreach (var a in r.SecReports)
                {
                    var ar = a as Form10K;
                    if (ar == null)
                        continue;
                    System.Diagnostics.Debug.WriteLine("IsLate: {2}; Accession Number: {0}; Link: {1}",
                        ar.AccessionNumber, ar.InteractiveFormLink, ar.IsLate);
                    System.Diagnostics.Debug.WriteLine($"HtmlLink: {ar.HtmlFormLink}");

                }
                System.Diagnostics.Debug.WriteLine("----------");
            }
            
        }

        [TestMethod]
        public void TestParseAccessionNumFromSummary()
        {
            var testResult =
                Copula.ParseAccessionNumFromSummary(
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
            var testResult = Form10K.CtorInteractiveLink(cik, acc);
            Assert.AreEqual(expectedRslt, testResult);
        }

        [TestMethod]
        public void TestGetUriFullTextSearch()
        {
            var testInput = new Edgar.FullTextSearch();
            testInput.CompanyName = "CITIBANK, N.A.";

            var testResult = SecFullTxtSearch.GetUri(testInput);

            System.Diagnostics.Debug.WriteLine(testResult.ToString());
        }

        [TestMethod]
        public void TestTryGetDayOfYearFiscalEnd()
        {
            int testResultOut;
            var testResult = Copula.TryGetDayOfYearFiscalEnd("--12-25", out testResultOut);
            System.Diagnostics.Debug.WriteLine(testResultOut);
            Assert.IsTrue(testResult);
        }

        [TestMethod]
        public void TestParseNameFromTitle()
        {
            var testResult = Copula.ParseNameFromTitle("10-K - 1347 Property Insurance Holdings, Inc.");
            System.Diagnostics.Debug.WriteLine(testResult.Item2);
            Assert.IsInstanceOfType(testResult.Item1, typeof(Form10K));
            Assert.AreEqual("1347 Property Insurance Holdings, Inc.", testResult.Item2);
            testResult = Copula.ParseNameFromTitle("13F-HR - 10-15 ASSOCIATES, INC.");
            Assert.IsInstanceOfType(testResult.Item1, typeof(Form13Fhr));
            Assert.AreEqual("10-15 ASSOCIATES, INC.", testResult.Item2);
            System.Diagnostics.Debug.WriteLine(testResult.Item2);
        }

        [TestMethod]
        public void TestSecFormFactory()
        {
            var testResult = SecForm.SecFormFactory("10-K");
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType(testResult, typeof(Form10K));
        }
    }
}
