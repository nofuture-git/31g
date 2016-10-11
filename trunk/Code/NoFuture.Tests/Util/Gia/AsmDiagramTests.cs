using System;
using System.Collections.Generic;
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

            var tesdf = testSubject.Edges;

            var t = tesdf.FirstOrDefault(x => x.NodeName[0] == "WorkOrder" && x.NodeName[1] == "Product");
            Assert.IsNotNull(t);
            var f = tesdf.FirstOrDefault(x => x.NodeName[0] == "Product" && x.NodeName[1] == "WorkOrder");
            Assert.IsNotNull(f);

            Assert.IsTrue(f.AreCounterparts(t));
        }

        [TestMethod]
        public void TestGetAdjacencyMatrix()
        {
            var testSubject = new NoFuture.Util.Gia.GraphViz.AsmDiagram(GetTestAsm());
            var testResult = testSubject.GetAdjacencyMatrix();
            Assert.IsNotNull(testResult.Item1);
            Assert.AreNotEqual(0, testResult.Item1.Length);
            Assert.IsNotNull(testResult.Item2);
            Assert.AreNotEqual(0,testResult.Item2.GetLongLength(0));
            Assert.AreNotEqual(0L, testResult.Item2.GetLongLength(1));
            Assert.AreEqual(testResult.Item2.GetLongLength(0), testResult.Item2.GetLongLength(1));

            for(var i = 0; i < testResult.Item2.GetLongLength(0); i++)
            {
                var ln = new List<int>();
                for (var j = 0; j < testResult.Item2.GetLongLength(1); j++)
                {
                    ln.Add(testResult.Item2[i,j]);
                }
                System.Diagnostics.Debug.WriteLine(string.Join(" ", ln));
            }
        }

        [TestMethod]
        public void TestGetAdjacencyMatrixJson()
        {
            var testSubject = new NoFuture.Util.Gia.GraphViz.AsmDiagram(GetTestAsm());
            var testResult = testSubject.GetAdjacencyMatrixJson();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            System.Diagnostics.Debug.WriteLine(testResult);
        }

    }
}
