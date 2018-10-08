using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using NoFuture.Util.DotNetMeta;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenName;
using NoFuture.Util.DotNetMeta.TokenType;

namespace NoFuture.Tests.Util.DotNetMetaTests
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
            _jsonFolder = Path.Combine(TestAssembly.UnitTestsRoot, @"Util\DotNetMetaTests");
            _asmIndicesPath = Path.Combine(_jsonFolder, "NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds.GetAsmIndices.json");
            _tokenIdsPath = Path.Combine(_jsonFolder, "NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds.GetTokenIds.json");
            _tokenNamePath = Path.Combine(_jsonFolder, "NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds.GetTokenNames.json");
            _tokenTypesPath = Path.Combine(_jsonFolder, "NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds.GetTokenTypes.json");
        }

        [Test]
        public void TestReadFiles()
        {
            var test00 = AsmIndexResponse.ReadFromFile(_asmIndicesPath);
            var test01 = TokenIdResponse.ReadFromFile(_tokenIdsPath);
            var test02 = TokenNameResponse.ReadFromFile(_tokenNamePath);
            var test03 = TokenTypeResponse.ReadFromFile(_tokenTypesPath);

            Assert.IsNotNull(test00);
            Assert.IsNotNull(test01);
            Assert.IsNotNull(test02);
            Assert.IsNotNull(test03);
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
                tokenData.Item2.GetAsRoot(), tokenData.Item4.GetTypesAsSingle(), tokenData.Item1);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Items);
            Assert.AreNotEqual(0 ,testResult.Items.Length);
        }

        [Test]
        public void TestSelectDistinct()
        {
            
        }
    }
}
