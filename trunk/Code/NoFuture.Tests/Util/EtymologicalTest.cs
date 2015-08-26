using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Util;

namespace NoFuture.Tests.Util
{
    [TestClass]
    public class EtymologicalTest
    {
        [TestMethod]
        public void TestTransformScreamingCapsToCamelCase()
        {
            const string TEST_INPUT = "USER_NAME";
            var testOutput = Etc.TransformScreamingCapsToCamelCase(TEST_INPUT);
            Assert.AreEqual("userName",testOutput);
        }

        [TestMethod]
        public void TestToCamelCase()
        {
            const string TEST_INPUT = "UserName";
            var testOutput = Etc.ToCamelCase(TEST_INPUT);
            Assert.AreEqual("userName", testOutput);

            testOutput = Etc.ToCamelCase("__" + TEST_INPUT);
            Assert.AreEqual("__userName",testOutput);

            testOutput = Etc.ToCamelCase("__" + TEST_INPUT.ToUpper());
            Assert.AreEqual("__username", testOutput);

            testOutput = Etc.ToCamelCase("ID");
            Assert.AreNotEqual("iD",testOutput);
            Assert.AreEqual("id",testOutput);

        }

        [TestMethod]
        public void TestTransformCamelCaseToSeparator()
        {
            const string TEST_INPUT = "UserName";
            var testOutput = Etc.TransformCamelCaseToSeparator(TEST_INPUT, '_');
            Assert.AreEqual("User_Name",testOutput);

        }

        [TestMethod]
        public void TestToEnglishSentenceCsv()
        {
            var testInput = "red fish, blue fish, green fish";
            var testOutput = Etc.ToEnglishSentenceCsv(testInput,',');
            Assert.AreEqual("red fish, blue fish and green fish",testOutput);

            testInput = "red fish-blue fish";
            testOutput = Etc.ToEnglishSentenceCsv(testInput, '-');
            Assert.AreEqual("red fish and blue fish",testOutput);

            testInput = "red fish-";
            testOutput = Etc.ToEnglishSentenceCsv(testInput, '-');
            Assert.AreEqual("red fish",testOutput);

            testInput = "red fish -- --";
            testOutput = Etc.ToEnglishSentenceCsv(testInput, '-');
            Assert.AreEqual("red fish",testOutput);

            testInput = "red fish-blue fish - - --";
            testOutput = Etc.ToEnglishSentenceCsv(testInput, '-');
            Assert.AreEqual("red fish and blue fish", testOutput);
        }

        [TestMethod]
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

        [TestMethod]
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
    }
}
