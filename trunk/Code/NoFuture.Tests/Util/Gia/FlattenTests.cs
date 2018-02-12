﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace NoFuture.Tests.Util.Gia
{
    [TestFixture]
    public class FlattenTests
    {

        [Test]
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
                Console.WriteLine(p);
        }

        [Test]
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
                Console.WriteLine(p);
        }

        [Test]
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
                Console.WriteLine(fj.ToGraphVizString());
        }

        [Test]
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
                Console.WriteLine(fj.ToGraphVizString());
            
        }

        [Test]
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

        [Test]
        public void TestFlattenType_DeleteMe()
        {
            var testAsm =
                NoFuture.Util.Binary.Asm.NfLoadFrom(
                    @"C:\Projects\31g\trunk\temp\debug\testAsms\Debug\Bfw.Domain.Model.dll");
            var testTypeName = "Bfw.Domain.Model.Customer.Participant";

            var startCount = 0;
            var testResult = NoFuture.Util.Gia.Flatten.FlattenType(testAsm, testTypeName, ref startCount, 16, null,
                false, null, null);

            var lastItem = testResult.LastOrDefault();

            Assert.IsNotNull(lastItem);
            Console.WriteLine(lastItem);

            var lf = lastItem.Items.Count;

            var lastLine = lastItem.Items[lf - 2];

            Assert.IsNotNull(lastLine);
            Console.WriteLine(lastLine.TypeFullName);
        }
    }
}
