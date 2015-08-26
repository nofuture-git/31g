using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Util.Gia
{
    [TestClass]
    public class AssemblyAnalysisTests
    {
        [TestMethod]
        public void TestMapAssemblyWordLeftAndRight()
        {
            var testAsm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => x.GetName().FullName.StartsWith("AdventureWorks"));

            if (testAsm == null)
            {
                Assembly.Load(
                    System.IO.File.ReadAllBytes(@"C:\Projects\31g\trunk\bin\NoFuture.Hbm.Sid.dll"));
                testAsm =
                    Assembly.Load(
                        System.IO.File.ReadAllBytes(
                            @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\AdventureWorks.dll"));
            }

            Assert.IsNotNull(testAsm);

            var testInput = new NoFuture.Util.Gia.Args.AssemblyLeftRightArgs
            {
                Assembly = testAsm,
                Separator = "-",
                MaxDepth = 16,
                TargetWord = "CreditCard"
            };

            var refactoredCode = NoFuture.Util.Gia.AssemblyAnalysis.MapAssemblyWordLeftAndRight(testInput);

            System.IO.File.WriteAllLines(@"C:\Projects\31g\trunk\temp\refactoredLeft.txt", refactoredCode.Item1.Items.Select(x => x.FlName));
        }
    }
}
