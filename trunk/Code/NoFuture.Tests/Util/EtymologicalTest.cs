using System;
using NUnit.Framework;
using NoFuture.Util;
using Dbg = System.Diagnostics.Debug;

namespace NoFuture.Tests.Util
{
    [TestFixture]
    public class EtymologicalTest
    {


        [Test]
        public void TestNomenclatureToponyms()
        {
            var testSubject = new NoFuture.Util.Etymological.Biz.Toponyms();

            string[] nullArray = null;
            Assert.IsFalse(testSubject.HasSemblance(nullArray));
            string nullString = null;
            Assert.IsFalse(testSubject.HasSemblance(nullString));

            Assert.IsTrue(testSubject.HasSemblance("Addr"));
            Assert.IsTrue(testSubject.HasSemblance("Address"));
            Assert.IsTrue(testSubject.HasSemblance("Addresses"));

            Assert.IsTrue(testSubject.HasSemblance(new[] {"Client","Address", "Line1"}));

            Assert.IsTrue(testSubject.HasSemblance(new[] { "Client", "Address", "Line1" }));

            Assert.IsTrue(testSubject.HasSemblance(new[] { "Address", "Line1" }));

            Assert.IsTrue(testSubject.HasSemblance(new[] { "Address", "Of", "Client" }));
            Assert.IsFalse(testSubject.HasSemblance(new[] { "Address", "Lines", "Line1" }));

            Assert.IsTrue(testSubject.HasSemblance(new[] { "PostalCode", "Id"}));

            Assert.IsTrue(testSubject.HasSemblance(new[] { "Addressdata", "Zip"}));

            Assert.IsFalse(testSubject.HasSemblance(new[] { "Client", "Response", "Id" }));

        }

        [Test]
        public void TestNomenclatureChronos()
        {
            var testSubject = new NoFuture.Util.Etymological.Biz.Chronos();
            string[] nullArray = null;
            Assert.IsFalse(testSubject.HasSemblance(nullArray));
            string nullString = null;
            Assert.IsFalse(testSubject.HasSemblance(nullString));
            Assert.IsTrue(testSubject.HasSemblance(new []{"Birth", "Date"}));
            Assert.IsTrue(testSubject.HasSemblance(new[] { "Date", "Of", "Birth" }));
            Assert.IsFalse(testSubject.HasSemblance(new[] { "Date", "Of", "Date" }));

            
        }

        [Test]
        public void TestToPlural()
        {
            var testResult = NoFuture.Util.Etymological.En.ToPlural("Birth");
            Dbg.WriteLine($"Birth -> {testResult}");
            Assert.AreEqual("Births", testResult);

            testResult = NoFuture.Util.Etymological.En.ToPlural("Apple");
            Dbg.WriteLine($"Apple -> {testResult}");
            Assert.AreEqual("Apples", testResult);

            testResult = NoFuture.Util.Etymological.En.ToPlural("Banana");
            Dbg.WriteLine($"Banana -> {testResult}");
            Assert.AreEqual("Bananas", testResult);

            testResult = NoFuture.Util.Etymological.En.ToPlural("Woman");
            Dbg.WriteLine($"Woman -> {testResult}");
            Assert.AreEqual("Women", testResult);

            testResult = NoFuture.Util.Etymological.En.ToPlural("Wolf");
            Dbg.WriteLine($"Wolf -> {testResult}");
            Assert.AreEqual("Wolves", testResult);

            testResult = NoFuture.Util.Etymological.En.ToPlural("Freeway");
            Dbg.WriteLine($"Freeway -> {testResult}");
            Assert.AreEqual("Freeways", testResult);

            testResult = NoFuture.Util.Etymological.En.ToPlural("Today");
            Dbg.WriteLine($"Today -> {testResult}");
            Assert.AreEqual("Todays", testResult);

            testResult = NoFuture.Util.Etymological.En.ToPlural("Addendum");
            Dbg.WriteLine($"Addendum -> {testResult}");
            Assert.AreEqual("Addenda", testResult);

            testResult = NoFuture.Util.Etymological.En.ToPlural("Nucleus");
            Dbg.WriteLine($"Nucleus -> {testResult}");
            Assert.AreEqual("Nuclei", testResult);

            testResult = NoFuture.Util.Etymological.En.ToPlural("Criterion");
            Dbg.WriteLine($"Criterion -> {testResult}");
            Assert.AreEqual("Criteria", testResult);

            testResult = NoFuture.Util.Etymological.En.ToPlural("Die");
            Dbg.WriteLine($"Die -> {testResult}");
            Assert.AreEqual("Dies", testResult);

            testResult = NoFuture.Util.Etymological.En.ToPlural("Life");
            Dbg.WriteLine($"Life -> {testResult}");
            Assert.AreEqual("Lives", testResult);

            testResult = NoFuture.Util.Etymological.En.ToPlural("Shelf");
            Dbg.WriteLine($"Shelf -> {testResult}");
            Assert.AreEqual("Shelves", testResult);

            testResult = NoFuture.Util.Etymological.En.ToPlural("Wife");
            Dbg.WriteLine($"Wife -> {testResult}");
            Assert.AreEqual("Wives", testResult);
        }
    }
}
