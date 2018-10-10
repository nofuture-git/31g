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
        private string _jsonFolder = "";
        private string _asmIndicesPath = "";
        private string _tokenNamePath = "";
        private string _tokenIdsPath = "";
        private string _tokenTypesPath = "";

        [SetUp]
        public void AssignPaths()
        {

            //_jsonFolder = Path.Combine(TestAssembly.DotNetMetaTestRoot, @"TestJsonData");
            _jsonFolder = @"C:\Projects\We_Nf_Mobile\Refactor\Bfw.Client.Participant";
            _asmIndicesPath = Path.Combine(_jsonFolder, "NoFuture.Util.DotNetMeta.InvokeAssemblyAnalysis.Cmds.GetAsmIndices.json");
            _tokenIdsPath = Path.Combine(_jsonFolder, "NoFuture.Util.DotNetMeta.InvokeAssemblyAnalysis.Cmds.GetTokenIds.json");
            _tokenNamePath = Path.Combine(_jsonFolder, "NoFuture.Util.DotNetMeta.InvokeAssemblyAnalysis.Cmds.GetTokenNames.json");
            _tokenTypesPath = Path.Combine(_jsonFolder, "NoFuture.Util.DotNetMeta.InvokeAssemblyAnalysis.Cmds.GetTokenTypes.json");
        }

        [Test]
        public void TestReadFiles()
        {
            var test00 = AsmIndexResponse.ReadFromFile(_asmIndicesPath);
            var test01 = TokenIdResponse.ReadFromFile(_tokenIdsPath);
            var test02 = TokenNameResponse.ReadFromFile(_tokenNamePath);
            var test03 = TokenTypeResponse.ReadFromFile(_tokenTypesPath);

            Assert.IsNotNull(test00?.Asms);
            Assert.IsNotNull(test01?.Tokens);
            Assert.IsNotNull(test02?.Names);
            Assert.IsNotNull(test03?.Types);
        }

        public Tuple<AsmIndexResponse, TokenIdResponse, TokenNameResponse, TokenTypeResponse> GetTokenData()
        {
            var test00 = AsmIndexResponse.ReadFromFile(_asmIndicesPath);
            var test01 = TokenIdResponse.ReadFromFile(_tokenIdsPath);
            var test02 = TokenNameResponse.ReadFromFile(_tokenNamePath);
            var test03 = TokenTypeResponse.ReadFromFile(_tokenTypesPath);

            return new Tuple<AsmIndexResponse, TokenIdResponse, TokenNameResponse, TokenTypeResponse>(test00, test01, test02, test03);

        }

        [Test]
        public void TestBuildMetadataTokenName()
        {
            var tokenData = GetTokenData();
            var testResult = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetNamesAsSingle(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Items);
            Assert.AreNotEqual(0 ,testResult.Items.Length);
        }

        [Test]
        public void TestSelectDistinct()
        {
            var tokenData = GetTokenData();
            var testSubject = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetNamesAsSingle(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1);
            Assert.IsNotNull(testSubject?.Items);

            var testResult = testSubject.SelectDistinct();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Console.WriteLine(testResult.Length);

            foreach (var t in testResult.Take(30))
                Console.WriteLine(t);
        }

        [Test]
        public void TestSelectDistinct_WithTypes()
        {
            var tokenData = GetTokenData();
            var testSubject = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetNamesAsSingle(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1);

            var testResult = testSubject.SelectDistinct(tokenData.Item4.GetTypesAsSingle());
            Assert.IsNotNull(testResult?.Items);
            Assert.AreNotEqual(0,testResult.Items.Length);
            Console.WriteLine(testResult.Items.Length);

        }

        [Test]
        public void TestSelectDistinct_ByMethod()
        {
            var tokenData = GetTokenData();
            var testSubject = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetNamesAsSingle(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1);
            var testResult = testSubject.SelectDistinct("TestBegin", "Index");
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Items);
            Assert.AreNotEqual(0, testResult.Items.Length);
        }

        [Test]
        public void TestBuildWithTokenTypes()
        {
            var tokenData = GetTokenData();
            var testSubject = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetNamesAsSingle(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1, tokenData.Item4.GetTypesAsSingle());
            Assert.IsNotNull(testSubject);

            TokenNameResponse.SaveToFile(@"C:\Projects\We_Nf_Mobile\Refactor\Bfw.Client.Participant\bigol.json", testSubject);
        }
    }
}
