using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace NoFuture.Tokens.Gia.Tests
{
    [TestFixture]
    public class FlattenTests
    {

        private Assembly GetTestAsm()
        {
            var testAsm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => x.GetName().FullName.StartsWith("AdventureWorks"));

            if (testAsm == null)
            {
                Assembly.Load(File.ReadAllBytes(AsmDiagramTests.PutTestFileOnDisk("ThirdDll.dll")));
                Assembly.Load(File.ReadAllBytes(AsmDiagramTests.PutTestFileOnDisk("SomethingShared.dll")));
                Assembly.Load(File.ReadAllBytes(AsmDiagramTests.PutTestFileOnDisk("SomeSecondDll.dll")));
                testAsm = Assembly.Load(File.ReadAllBytes(AsmDiagramTests.PutTestFileOnDisk("AdventureWorks2012.dll")));
            }
            NoFuture.Shared.Cfg.NfConfig.AssemblySearchPaths.Add(AsmDiagramTests.GetTestFileDirectory());
            NoFuture.Util.Binary.FxPointers.AddResolveAsmEventHandlerToDomain();


            return testAsm;
        }

        [Test]
        public void TestFlattenTypeMembers()
        {
            var testAsm = GetTestAsm();

            Assert.IsNotNull(testAsm);

            var testGia = new FlattenTypeArgs()
            {
                Assembly = testAsm,
                UseTypeNames = false,
                Separator = "-",
                TypeFullName = "AdventureWorks.Person.Person",
                Depth = 16
            };
            var testPrint = Flatten.FlattenType(testGia);
            Assert.IsNotNull(testPrint);
            var printLines = testPrint.PrintLines();
            Assert.IsNotNull(printLines);
            System.IO.File.WriteAllLines(AsmDiagramTests.GetTestFileDirectory() + @"\FlattenedExample.txt", printLines);
            foreach (var p in printLines)
                Console.WriteLine(p);
        }

        [Test]
        public void TestFlattenTypeMembersWithLimit()
        {
            var limitOn = "System.String";
            var testAsm = GetTestAsm();

            Assert.IsNotNull(testAsm);
            
            var testGia = new FlattenTypeArgs()
            {
                Assembly = testAsm,
                UseTypeNames = false,
                Separator = "-",
                TypeFullName = "AdventureWorks.Person.Person",
                Depth = 16,
                LimitOnThisType = limitOn
            };
            var testPrint = Flatten.FlattenType(testGia);
            Assert.IsNotNull(testPrint);
            var printLines = testPrint.PrintLines();
            Assert.IsNotNull(printLines);
            System.IO.File.WriteAllLines(AsmDiagramTests.GetTestFileDirectory() + @"\FlattenedExample.txt", printLines);
            foreach(var p in printLines)
                Console.WriteLine(p);
        }

        [Test]
        public void TestFlattenTypeGetGraphVizMrecords()
        {
            var testAsm = GetTestAsm();

            Assert.IsNotNull(testAsm);

            var testGia = new FlattenTypeArgs()
            {
                Assembly = testAsm,
                UseTypeNames = false,
                Separator = "-",
                TypeFullName = "AdventureWorks.Person.Person",
                Depth = 16
            };
            var flattenedType = Flatten.FlattenType(testGia);

            var testResult = flattenedType.GetGraphVizMrecords;

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach(var fj in testResult)
                Console.WriteLine(fj.ToGraphVizString());
        }

        [Test]
        public void TestFlattenTypeGetGraphVizEdges()
        {
            var testAsm = GetTestAsm();

            Assert.IsNotNull(testAsm);

            var testGia = new FlattenTypeArgs()
            {
                Assembly = testAsm,
                UseTypeNames = false,
                Separator = "-",
                TypeFullName = "AdventureWorks.Person.Person",
                Depth = 16
            };
            var flattenedType = Flatten.FlattenType(testGia);

            var testResult = flattenedType.GetGraphVizEdges;
            Assert.IsNotNull(testResult);
            foreach (var fj in testResult)
                Console.WriteLine(fj.ToGraphVizString());
            
        }

        [Test]
        public void TestFlattentypeToGraphVizString()
        {
            var testAsm = GetTestAsm();
            Assert.IsNotNull(testAsm);

            var testGia = new FlattenTypeArgs()
            {
                Assembly = testAsm,
                UseTypeNames = false,
                Separator = "-",
                TypeFullName = "AdventureWorks.Person.Person",
                Depth = 16
            };
            var flattenedType = Flatten.FlattenType(testGia);

            var testResult = flattenedType.ToGraphVizString();

            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            System.IO.File.WriteAllText(AsmDiagramTests.GetTestFileDirectory() + @"\TestGraphVizFlatType.gv", testResult);
        }
    }
}
