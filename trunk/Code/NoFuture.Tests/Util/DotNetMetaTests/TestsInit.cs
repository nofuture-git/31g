using System;
using System.IO;
using NUnit.Framework;

namespace NoFuture.Util.DotNetMeta.Tests
{
    [SetUpFixture]
    public sealed class TestAssembly
    {
        private static string _testRoot;
        private static string _rootBin;
        private static string _dotNetMetaTestRoot;
        public static string UnitTestsRoot => _testRoot;
        public static string DotNetMetaTestRoot => _dotNetMetaTestRoot;
        public static string RootBin => _rootBin;

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


        }
    }
}
