using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Rand.Tests.DataTests.SpTests
{
    [TestClass]
    public class PondusTests
    {
        [TestMethod]
        public void TestEquals()
        {
            var testSubject = new NoFuture.Rand.Data.Sp.Pondus("test")
            {
                Inception = DateTime.Today.AddYears(-1),
                Terminus = DateTime.Today
            };

            var testCompare = new NoFuture.Rand.Data.Sp.Pondus("test")
            {
                Inception = DateTime.Today.AddYears(-1),
                Terminus = DateTime.Today
            };

            //same dates and name
            Assert.IsTrue(testSubject.Equals(testCompare));
            testCompare.Terminus = DateTime.Today.AddDays(1);

            //same name, same start date, diff end date
            Assert.IsFalse(testSubject.Equals(testCompare));

            //same dates, diff name
            testCompare.Terminus = DateTime.Today.AddDays(1);
            testCompare.Id.Name = "test2";

            Assert.IsFalse(testSubject.Equals(testCompare));

            //null date is diff date
            testCompare.Id.Name = "test";
            testCompare.Terminus = null;

            Assert.IsFalse(testSubject.Equals(testCompare));

        }
    }
}
