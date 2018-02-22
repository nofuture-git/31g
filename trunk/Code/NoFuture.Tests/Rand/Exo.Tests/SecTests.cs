using System;
using NUnit.Framework;
using NoFuture.Rand.Com;
using NoFuture.Rand.Exo.NfXml;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Gov.US.Sec;

namespace NoFuture.Rand.Exo.Tests.TestData
{
    [TestFixture]
    public class SecTests
    {

        public string SEC_BY_SIC_XML_PATH = TestAssembly.TestDataDir + @"\SecBySIC.xml";
        public string SEC_BY_CIK_XML_PATH = TestAssembly.TestDataDir + @"\SecByCIK.xml";
        public string SEC_BY_FULLTEXT_XML_PATH = TestAssembly.TestDataDir + @"\SecByFullText.xml";

        [Test]
        public void TestTryGetCorpData()
        {
            var xmlContent = System.IO.File.ReadAllText(SEC_BY_CIK_XML_PATH);

            var testResultOut = new PublicCompany();
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

            Assert.IsInstanceOf(typeof(UsStreetPo), testResultOut.MailingAddress.Street);
            Assert.IsInstanceOf(typeof(UsCityStateZip), testResultOut.MailingAddress.CityArea);

            var mailingAddrStreet = (UsStreetPo)testResultOut.MailingAddress.Street;
            var mailingAddrCity = (UsCityStateZip) testResultOut.MailingAddress.CityArea;

            Assert.IsInstanceOf(typeof(UsStreetPo), testResultOut.BusinessAddress.Street);
            Assert.IsInstanceOf(typeof(UsCityStateZip), testResultOut.BusinessAddress.CityArea);

            var bizAddrStreet = (UsStreetPo)testResultOut.BusinessAddress.Street;
            var bizAddrCity = (UsCityStateZip)testResultOut.BusinessAddress.CityArea;

            Assert.AreEqual("250", mailingAddrStreet.PostBox);

            Assert.AreEqual("250", bizAddrStreet.PostBox);
            Assert.AreEqual("GLEN", bizAddrStreet.StreetName);
            Assert.AreEqual("ST", bizAddrStreet.StreetKind);


            Assert.AreEqual("GLENS FALLS", bizAddrCity.City);
            Assert.AreEqual("NY", bizAddrCity.StateAbbrev);
            Assert.AreEqual("12801", bizAddrCity.ZipCode);

            Assert.AreEqual("GLENS FALLS", mailingAddrCity.City);
            Assert.AreEqual("NY", mailingAddrCity.StateAbbrev);
            Assert.AreEqual("12801", mailingAddrCity.ZipCode);

            Assert.IsNotNull(testResultOut.Phone);
            Assert.IsNotNull(testResultOut.Phone[0]);
            Assert.AreEqual("518", testResultOut.Phone[0].AreaCode);
            Assert.AreEqual("415", testResultOut.Phone[0].CentralOfficeCode);
            Assert.AreEqual("4299", testResultOut.Phone[0].SubscriberNumber);

        }

        [Test]
        public void TestParseCompanyFullTextSearch()
        {
            var xmlContent = System.IO.File.ReadAllText(SEC_BY_FULLTEXT_XML_PATH);
            var testResult = Copula.ParseSecEdgarFullTextSearch(xmlContent, new Uri("http://www.sec.gov/cgi-bin/srch-edgar"));
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach (var r in testResult)
            {
                System.Console.WriteLine($"Name: {r.Name}; CIK: {r.CIK.Value}");
                foreach (var a in r.SecReports)
                {
                    var ar = a as Form10K;
                    if (ar == null)
                        continue;
                    System.Console.WriteLine("IsLate: {2}; Accession Number: {0}; Link: {1}",
                        ar.AccessionNumber, ar.InteractiveFormLink, ar.IsLate);
                    System.Console.WriteLine($"HtmlLink: {ar.HtmlFormLink}");

                }
                System.Console.WriteLine("----------");
            }
            
        }

        [Test]
        public void TestParseAccessionNumFromSummary()
        {
            var testResult =
                Copula.ParseAccessionNumFromSummary(
                    "<b>Filed Date:</b> 04/15/2015 <b>Accession Number:</b> 0001549727-15-000033 <b>Size:</b> 3 MB");
            Assert.IsNotNull(testResult);
            Assert.AreEqual("0001549727-15-000033", testResult);
            System.Console.WriteLine(testResult);
        }

        [Test]
        public void TestCtorInteractiveLink()
        {
            const string cik = "805729";
            const string acc = "0001549727-15-000033";
            var expectedRslt =
                new Uri("https://www.sec.gov/cgi-bin/viewer?action=view&cik=" + cik + "&accession_number=" + acc +
                        "&xbrl_type=v");
            var testResult = Form10K.CtorInteractiveLink(cik, acc);
            Assert.AreEqual(expectedRslt, testResult);
        }

        [Test]
        public void TestGetUriFullTextSearch()
        {
            var testInput = new Edgar.FullTextSearch();
            testInput.CompanyName = "CITIBANK, N.A.";

            var testResult = SecFullTxtSearch.GetUri(testInput);

            System.Console.WriteLine(testResult.ToString());
        }

        [Test]
        public void TestTryGetDayOfYearFiscalEnd()
        {
            int testResultOut;
            var testResult = Copula.TryGetDayOfYearFiscalEnd("--12-25", out testResultOut);
            System.Console.WriteLine(testResultOut);
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestParseNameFromTitle()
        {
            var testResult = Copula.ParseNameFromTitle("10-K - 1347 Property Insurance Holdings, Inc.");
            System.Console.WriteLine(testResult.Item2);
            Assert.IsInstanceOf(typeof(Form10K), testResult.Item1);
            Assert.AreEqual("1347 Property Insurance Holdings, Inc.", testResult.Item2);
            testResult = Copula.ParseNameFromTitle("13F-HR - 10-15 ASSOCIATES, INC.");
            Assert.IsInstanceOf(typeof(Form13Fhr), testResult.Item1);
            Assert.AreEqual("10-15 ASSOCIATES, INC.", testResult.Item2);
            System.Console.WriteLine(testResult.Item2);
        }

        [Test]
        public void TestSecFormFactory()
        {
            var testResult = SecForm.SecFormFactory("10-K");
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf(typeof(Form10K), testResult);
        }
    }
}
