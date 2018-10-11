using System;
using System.IO;
using System.Linq;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenName;
using NoFuture.Util.DotNetMeta.TokenType;
using NUnit.Framework;

namespace NoFuture.Util.DotNetMeta.Tests
{
    public class TestMetadataTokenName
    {

        [Test]
        public void TestBuildMetadataTokenName()
        {
            var tokenData = TestInit.GetTokenData();
            var testResult = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetAsRoot(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Items);
            Assert.AreNotEqual(0 ,testResult.Items.Length);
        }

        [Test]
        public void TestSelectDistinct()
        {
            var tokenData = TestInit.GetTokenData();
            var testSubject = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetAsRoot(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1);
            Assert.IsNotNull(testSubject?.Items);

            var testResult = testSubject.SelectDistinctShallowArray();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Console.WriteLine(testResult.Length);

            foreach (var t in testResult.Take(30))
                Console.WriteLine(t);
        }

        [Test]
        public void TestSelectDistinct_WithTypes()
        {
            var tokenData = TestInit.GetTokenData();
            var testSubject = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetAsRoot(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1);

            var testResult = testSubject.SelectDistinct(tokenData.Item4.GetAsRoot());
            Assert.IsNotNull(testResult?.Items);
            Assert.AreNotEqual(0,testResult.Items.Length);
            Console.WriteLine(testResult.Items.Length);

        }

        [Test]
        public void TestSelectDistinct_ByMethod()
        {
            var tokenData = TestInit.GetTokenData();
            var testSubject = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetAsRoot(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1);
            var testResult = testSubject.SelectDistinct("TestBegin", "Index");
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Items);
            Assert.AreNotEqual(0, testResult.Items.Length);
        }

        [Test]
        public void TestBuildWithTokenTypes()
        {
            var tokenData = TestInit.GetTokenData();
            var testSubject = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetAsRoot(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1, tokenData.Item4.GetAsRoot());
            Assert.IsNotNull(testSubject);
        }

    }
}
