using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Exo.NfHtml;
using NoFuture.Rand.Gov.US.Sec;

namespace NoFuture.Rand.Exo.Tests.NfHtmlTests
{
    [TestFixture]
    public class SecGetXbrlUriTests
    {
        [Test]
        public void TestParseContent()
        {
            var testUri = new Uri(
                "https://www.sec.gov/Archives/edgar/data/1593936/000155837016004206/0001558370-16-004206-index.htm");
            var testContent =
                System.IO.File.ReadAllText(
                   TestAssembly.UnitTestsRoot + @"\ExampleDlls\ExampleSecIndexHtm.html");
            var testSubject = new NoFuture.Rand.Com.PublicCorporation { CIK = new CentralIndexKey { Value = "0000768899" }};
            testSubject.UpsertName(KindsOfNames.Legal, "TrueBlue, Inc.");
            testSubject.SecReports.Add(new Form10K {HtmlFormLink = testUri });
            var testResult = Copula.TryGetXmlLink(testContent,testUri,ref testSubject);

            Assert.IsTrue(testResult);
            var testResultItem = testSubject.SecReports.FirstOrDefault(x =>x is Form10K && ((Form10K)x).HtmlFormLink == testUri);
            Assert.IsNotNull(testResultItem);
            Assert.IsInstanceOf(typeof(Form10K), testResultItem);
            Assert.IsNotNull( ((Form10K)testResultItem).XmlLink);
            Assert.IsNotNull(testSubject.EIN);
            Assert.AreEqual("371737959", testSubject.EIN.Value);
        }

        [Test]
        public void TestGetXbrlXmlPartialUri()
        {
            var testInput = new List<string>
            {
                "href=\"/index.htm\"",
                "href=\"/cgi-bin/browse-edgar?action=getcurrent\"",
                "href=\"javascript:history.back()\"",
                "href=\"/edgar/searchedgar/webusers.htm\"",
                "href=\"/edgar/searchedgar/companysearch.html\"",
                "href=\"/cgi-bin/viewer?action=view&amp;cik=1593936&amp;accession_number=0001558370-15-000377&amp;xbrl_type=v\"",
                "id=\"interactiveDataBtn\"",
                "href=\"/Archives/edgar/data/1593936/000155837015000377/mik-20150131x10k.htm\"",
                "href=\"/Archives/edgar/data/1593936/000155837015000377/mik-20150131ex311e43031.htm\"",
                "href=\"/Archives/edgar/data/1593936/000155837015000377/mik-20150131ex312c02202.htm\"",
                "href=\"/Archives/edgar/data/1593936/000155837015000377/mik-20150131ex3217e6c3a.htm\"",
                "href=\"/Archives/edgar/data/1593936/000155837015000377/mik-20150131ex991c7a0da.htm\"",
                "href=\"/Archives/edgar/data/1593936/000155837015000377/mik-20150131x10kg001.jpg\"",
                "href=\"/Archives/edgar/data/1593936/000155837015000377/0001558370-15-000377.txt\"",
                "href=\"/Archives/edgar/data/1593936/000155837015000377/mik-20150131.xml\"",
                "href=\"/Archives/edgar/data/1593936/000155837015000377/mik-20150131.xsd\"",
                "href=\"/Archives/edgar/data/1593936/000155837015000377/mik-20150131_cal.xml\"",
                "href=\"/Archives/edgar/data/1593936/000155837015000377/mik-20150131_def.xml\"",
                "href=\"/Archives/edgar/data/1593936/000155837015000377/mik-20150131_lab.xml\"",
                "href=\"/Archives/edgar/data/1593936/000155837015000377/mik-20150131_pre.xml\"",
                "href=\"/cgi-bin/browse-edgar?CIK=0001593936&amp;action=getcompany\"",
                "href=\"/cgi-bin/browse-edgar?filenum=001-36501&amp;action=getcompany\"",
                "href=\"/cgi-bin/browse-edgar?action=getcompany&amp;SIC=5945&amp;owner=include\""
            };

            var testResult = SecGetXbrlUri.GetXbrlXmlPartialUri(testInput);
            Assert.AreEqual("/Archives/edgar/data/1593936/000155837015000377/mik-20150131.xml", testResult);
        }
    }
}
