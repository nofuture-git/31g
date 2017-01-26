using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests
{
    [TestClass]
    public sealed class TestAssembly
    {
        private static string _testRoot;
        private static string _rootBin;
        public static string UnitTestsRoot => _testRoot;
        public static string RootBin => _rootBin;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            _testRoot = @"C:\Projects\31g\trunk\Code\NoFuture.Tests";
            if(!System.IO.Directory.Exists(_testRoot))
                throw new InvalidOperationException("The root directory, from which all resources " +
                                                    "specific to testing are located, was not found. ");
            _rootBin = @"C:\Projects\31g\trunk\bin";
            if(!System.IO.Directory.Exists(_rootBin))
                throw new InvalidOperationException("The root directory, in which all NoFuture binaries are " +
                                                    "built to, was not found.");

        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
        }
    }
}
