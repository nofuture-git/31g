using System;
using aima_csharp_3e.Search.Local;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test_aima_csharp_3e.Local
{
    [TestClass]
    public class TestExpSchedule
    {
        [TestMethod]
        public void TestGetTemp()
        {
            var testSubject = new ExpSchedule(0.045D);
            //double lam=0.005D, int k=20, int limit=100
            for (var i = 1; i < testSubject.Limit+1; i++)
            {
                var testResult = testSubject.Eval(i);
                System.Diagnostics.Debug.WriteLine(testResult);
            }
        }
    }
}
