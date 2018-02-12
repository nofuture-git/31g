using System;
using NUnit.Framework;

namespace NoFuture.Rand.Exo.Tests
{
    [SetUpFixture]
    public sealed class TestAssembly
    {
        private static string _testDataDir;
        public static string TestDataDir => _testDataDir;

        [OneTimeSetUp]
        public static void AssemblyInitialize()
        {
            _testDataDir = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Rand\Exo.Tests\TestData";
            if (!System.IO.Directory.Exists(_testDataDir))
                throw new InvalidOperationException("The root directory, from which all resources " +
                                                    "specific to testing are located, was not found. ");
        }
    }
}
