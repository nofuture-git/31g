using System;
using aima_csharp_3e.Search.Nondeterministic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test_aima_csharp_3e.ErraticVacuumWorld;

namespace Test_aima_csharp_3e.Nondeterministic
{
    [TestClass]
    public class TestAndOrGraphSearch
    {
        [TestMethod]
        public void TestSearch()
        {
            var testProblem = new ErraticVacuumProblem();
            var testSubject = new AndOrGraphSearch(testProblem);
            var testResult = testSubject.Search();

            Assert.IsTrue(testProblem.IsGoalTest(testResult));
            
        }
    }
}
