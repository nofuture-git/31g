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

        [Test]
        public void TestGetUnion()
        {
            var tokenData = TestInit.GetTokenData();
            var testSubject = tokenData.Item3.GetAsRoot();
            var testInput = testSubject.SelectDistinct("OptumController", "Index");
            var testInput2 = testSubject.SelectDistinct("OptumController", "GetInitSsoToken");

            var trIds = testInput.Items.Select(t => t.Id).ToList();
            var tr2Ids = testInput2.Items.Select(t => t.Id).ToList();

            var trNames = testInput.Items.Select(t => t.Name).ToList();
            var tr2Names = testInput2.Items.Select(t => t.Name).ToList();

            Console.WriteLine($"Total id count {trIds.Count + tr2Ids.Count}" );
            trIds.AddRange(tr2Ids);
            var distinctIds = trIds.Distinct().ToList();
            Console.WriteLine($"Distinct id count {distinctIds.Count}" );

            Console.WriteLine($"Total name count {trNames.Count  + tr2Names.Count}");
            trNames.AddRange(tr2Names);
            var distinctNames = trNames.Distinct().ToList();
            Console.WriteLine($"Distinct name count {distinctNames.Count}");

            var testResult = testInput.GetUnion(testInput2);
            Assert.AreNotEqual(0 , testResult.Items.Length);
        }

    }
}
