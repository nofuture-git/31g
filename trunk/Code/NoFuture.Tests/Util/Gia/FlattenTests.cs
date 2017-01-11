using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Util.Gia
{
    [TestClass]
    public class FlattenTests
    {

        [TestMethod]
        public void TestFlattenTypeMembers()
        {
            var testAsm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => x.GetName().FullName.StartsWith("AdventureWorks"));

            if (testAsm == null)
            {
                Assembly.Load(
                    System.IO.File.ReadAllBytes(TestAssembly.RootBin + @"\NoFuture.Hbm.Sid.dll"));
                testAsm =
                    Assembly.Load(
                        System.IO.File.ReadAllBytes(
                            TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks.dll"));
            }

            Assert.IsNotNull(testAsm);

            var testGia = new NoFuture.Util.Gia.Args.FlattenTypeArgs()
            {
                Assembly = testAsm,
                UseTypeNames = false,
                Separator = "-",
                TypeFullName = "AdventureWorks.Person.Person",
                Depth = 16
            };
            var testPrint = NoFuture.Util.Gia.Flatten.FlattenType(testGia);
            Assert.IsNotNull(testPrint);
            var printLines = testPrint.PrintLines();
            Assert.IsNotNull(printLines);
            System.IO.File.WriteAllLines(TestAssembly.UnitTestsRoot + @"\FlattenedExample.txt", printLines);
            foreach (var p in printLines)
                System.Diagnostics.Debug.WriteLine(p);
        }

        [TestMethod]
        public void TestFlattenTypeMembersWithLimit()
        {
            var limitOn = "System.String";
            var testAsm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => x.GetName().FullName.StartsWith("AdventureWorks"));

            if (testAsm == null)
            {
                Assembly.Load(
                    System.IO.File.ReadAllBytes(TestAssembly.RootBin + @"\NoFuture.Hbm.Sid.dll"));
                testAsm =
                    Assembly.Load(
                        System.IO.File.ReadAllBytes(
                            TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks.dll"));
            }

            Assert.IsNotNull(testAsm);
            
            var testGia = new NoFuture.Util.Gia.Args.FlattenTypeArgs()
            {
                Assembly = testAsm,
                UseTypeNames = false,
                Separator = "-",
                TypeFullName = "AdventureWorks.Person.Person",
                Depth = 16,
                LimitOnThisType = limitOn
            };
            var testPrint = NoFuture.Util.Gia.Flatten.FlattenType(testGia);
            Assert.IsNotNull(testPrint);
            var printLines = testPrint.PrintLines();
            Assert.IsNotNull(printLines);
            System.IO.File.WriteAllLines(TestAssembly.UnitTestsRoot + @"\FlattenedExample.txt", printLines);
            foreach(var p in printLines)
                System.Diagnostics.Debug.WriteLine(p);
        }

        [TestMethod]
        public void TestFlattenTypeGetGraphVizMrecords()
        {
            var testAsm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => x.GetName().FullName.StartsWith("AdventureWorks"));

            if (testAsm == null)
            {
                Assembly.Load(
                    System.IO.File.ReadAllBytes(TestAssembly.RootBin + @"\NoFuture.Hbm.Sid.dll"));
                testAsm =
                    Assembly.Load(
                        System.IO.File.ReadAllBytes(
                            TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks.dll"));
            }

            Assert.IsNotNull(testAsm);

            var testGia = new NoFuture.Util.Gia.Args.FlattenTypeArgs()
            {
                Assembly = testAsm,
                UseTypeNames = false,
                Separator = "-",
                TypeFullName = "AdventureWorks.Person.Person",
                Depth = 16
            };
            var flattenedType = NoFuture.Util.Gia.Flatten.FlattenType(testGia);

            var testResult = flattenedType.GetGraphVizMrecords;

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach(var fj in testResult)
                System.Diagnostics.Debug.WriteLine(fj.ToGraphVizString());
        }

        [TestMethod]
        public void TestFlattenTypeGetGraphVizEdges()
        {
            var testAsm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => x.GetName().FullName.StartsWith("AdventureWorks"));

            if (testAsm == null)
            {
                Assembly.Load(
                    System.IO.File.ReadAllBytes(TestAssembly.RootBin + @"\NoFuture.Hbm.Sid.dll"));
                testAsm =
                    Assembly.Load(
                        System.IO.File.ReadAllBytes(
                            TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks.dll"));
            }

            Assert.IsNotNull(testAsm);

            var testGia = new NoFuture.Util.Gia.Args.FlattenTypeArgs()
            {
                Assembly = testAsm,
                UseTypeNames = false,
                Separator = "-",
                TypeFullName = "AdventureWorks.Person.Person",
                Depth = 16
            };
            var flattenedType = NoFuture.Util.Gia.Flatten.FlattenType(testGia);

            var testResult = flattenedType.GetGraphVizEdges;
            Assert.IsNotNull(testResult);
            foreach (var fj in testResult)
                System.Diagnostics.Debug.WriteLine(fj.ToGraphVizString());
            
        }

        [TestMethod]
        public void TestFlattentypeToGraphVizString()
        {
            var testAsm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => x.GetName().FullName.StartsWith("AdventureWorks"));

            if (testAsm == null)
            {
                Assembly.Load(
                    System.IO.File.ReadAllBytes(TestAssembly.RootBin + @"\NoFuture.Hbm.Sid.dll"));
                testAsm =
                    Assembly.Load(
                        System.IO.File.ReadAllBytes(
                            TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks.dll"));
            }

            Assert.IsNotNull(testAsm);

            var testGia = new NoFuture.Util.Gia.Args.FlattenTypeArgs()
            {
                Assembly = testAsm,
                UseTypeNames = false,
                Separator = "-",
                TypeFullName = "AdventureWorks.Person.Person",
                Depth = 16
            };
            var flattenedType = NoFuture.Util.Gia.Flatten.FlattenType(testGia);

            var testResult = flattenedType.ToGraphVizString();

            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            System.IO.File.WriteAllText(TestAssembly.UnitTestsRoot + @"\TestGraphVizFlatType.gv", testResult);
        }
    }
}
