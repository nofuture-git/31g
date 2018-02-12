using System.IO;
using NUnit.Framework;

namespace NoFuture.Tests.Tokens
{
    [TestFixture]
    public class TestEtx
    {
        [Test]
        public void TestGetHtmlAsXml()
        {
            var testContent00 = TestAssembly.UnitTestsRoot + @"\Rand\BloombergSearchRslt_multiple.html";
            var testContent01 = TestAssembly.UnitTestsRoot + @"\Rand\ffiecHtml.html";

            var testResult = NoFuture.Tokens.Etx.GetHtmlAsXml(File.ReadAllText(testContent00));
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.DocumentElement);

            var view2TestInput = NoFuture.Tokens.Etx.GetHtmlOnly(File.ReadAllText(testContent01));
            File.WriteAllText(TestAssembly.UnitTestsRoot + @"\Rand\view2TestInput.html", view2TestInput);

            testResult = NoFuture.Tokens.Etx.GetHtmlAsXml(File.ReadAllText(testContent01));
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.DocumentElement);

        }
    }
}
