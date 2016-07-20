using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Util.Gia;

namespace NoFuture.Tests.Util.Gia
{
    [TestClass]
    public class AsmDiagramTests
    {
        private Assembly GetTestAsm()
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
            return testAsm;
        }

        [TestMethod]
        public void TestCtor()
        {

            var testAsm = GetTestAsm();

            var testSubject = new NoFuture.Util.Gia.GraphViz.AsmDiagram(testAsm);

            var testResult = testSubject.ToGraphVizString();

            Assert.IsNotNull(testResult);

            System.IO.File.WriteAllText(@"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\AsmDiagramTest.gv", testResult);
        }

        [TestMethod]
        public void TestGetDupIndices()
        {
            var testSubject = new NoFuture.Util.Gia.GraphViz.AsmDiagram(GetTestAsm());

            var tesdf = testSubject.Items;

            var t = tesdf.FirstOrDefault(x => x.NodeName[0] == "WorkOrder" && x.NodeName[1] == "Product");
            Assert.IsNotNull(t);
            var f = tesdf.FirstOrDefault(x => x.NodeName[0] == "Product" && x.NodeName[1] == "WorkOrder");
            Assert.IsNotNull(f);

            Assert.IsTrue(f.AreCounterparts(t));
        }

    }
}
