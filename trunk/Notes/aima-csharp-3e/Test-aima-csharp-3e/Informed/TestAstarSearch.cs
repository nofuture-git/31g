using System;
using System.Collections.Generic;
using aima_csharp_3e.Agent;
using aima_csharp_3e.Search;
using aima_csharp_3e.Search.Framework;
using aima_csharp_3e.Search.Informed;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test_aima_csharp_3e.RoadMapRomania;

namespace Test_aima_csharp_3e.Informed
{
    [TestClass]
    public class TestAstarSearch
    {
        [TestMethod]
        public void TestSearchWithRoadMap()
        {
            var problem = new RoadMapProblem();
            var hOfn = new StraightLineDistance();

            var testSubject = new AstarSearch(problem, hOfn);

            var testResult = testSubject.Search();

            Assert.AreEqual(418, testResult.PathCost);

            var destCity = testResult.State as City;
            Assert.IsNotNull(destCity);
            System.Diagnostics.Debug.WriteLine(testResult);
        }
    }
}
