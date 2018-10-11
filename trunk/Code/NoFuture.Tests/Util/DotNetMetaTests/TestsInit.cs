using System;
using System.IO;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenName;
using NoFuture.Util.DotNetMeta.TokenType;
using NUnit.Framework;

namespace NoFuture.Util.DotNetMeta.Tests
{
    [SetUpFixture]
    public sealed class TestInit
    {
        private static string _testRoot;
        private static string _rootBin;
        private static string _dotNetMetaTestRoot;
        public static string UnitTestsRoot => _testRoot;
        public static string DotNetMetaTestRoot => _dotNetMetaTestRoot;
        public static string RootBin => _rootBin;
        private static string _jsonFolder = "";
        private static string _asmIndicesPath = "";
        private static string _tokenNamePath = "";
        private static string _tokenIdsPath = "";
        private static string _tokenTypesPath = "";

        [OneTimeSetUp]
        public static void AssemblyInitialize()
        {
            _testRoot = @"C:\Projects\31g\trunk\Code\NoFuture.Tests";
            if(!System.IO.Directory.Exists(_testRoot))
                throw new InvalidOperationException("The root directory, from which all resources " +
                                                    "specific to testing are located, was not found. ");
            _rootBin = @"C:\Projects\31g\trunk\bin";
            if(!System.IO.Directory.Exists(_rootBin))
                throw new InvalidOperationException("The root directory, in which all NoFuture binaries are " +
                                                    "built to, was not found.");

            _dotNetMetaTestRoot = Path.Combine(_testRoot, @"Util\DotNetMetaTests");
            if (!System.IO.Directory.Exists(_dotNetMetaTestRoot))
                throw new InvalidOperationException("The specific directory for DotNetMeta unit tests " +
                                                    "was not found.");
            //_jsonFolder = Path.Combine(TestAssembly.DotNetMetaTestRoot, @"TestJsonData");
            _jsonFolder = @"C:\Projects\We_Nf_Mobile\Refactor\Bfw.Client.Participant";
            _asmIndicesPath = Path.Combine(_jsonFolder, "GetAsmIndices.json");
            _tokenIdsPath = Path.Combine(_jsonFolder, "GetTokenIds.json");
            _tokenNamePath = Path.Combine(_jsonFolder, "GetTokenNames.json");
            _tokenTypesPath = Path.Combine(_jsonFolder, "GetTokenTypes.json");
        }

        public static Tuple<AsmIndexResponse, TokenIdResponse, TokenNameResponse, TokenTypeResponse> GetTokenData()
        {
            var test00 = AsmIndexResponse.ReadFromFile(_asmIndicesPath);
            var test01 = TokenIdResponse.ReadFromFile(_tokenIdsPath);
            var test02 = TokenNameResponse.ReadFromFile(_tokenNamePath);
            var test03 = TokenTypeResponse.ReadFromFile(_tokenTypesPath);

            return new Tuple<AsmIndexResponse, TokenIdResponse, TokenNameResponse, TokenTypeResponse>(test00, test01, test02, test03);

        }
    }
}
