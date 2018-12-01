using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class TransactionTests
    {
        [Test]
        public void TestGetThisAsTraceId()
        {
            var data = new SortedSet<ITransaction>(new TransactionComparer());

            var testSubjectId = Transaction.AddTransaction(data, DateTime.UtcNow, 8000M.ToPecuniam(), new VocaBase("Owner's Capital"));
            var testSubject = data.First(t => t.UniqueId == testSubjectId);
            var testResult = testSubject.GetThisAsTraceId();
            Assert.IsNotNull(testResult);
            Assert.AreEqual(testSubject.AtTime, testResult.AtTime);
            Assert.AreEqual(testSubject.UniqueId, testResult.UniqueId);
            Assert.AreEqual(testSubject.Description, testResult.Description);

            testResult = testSubject.GetThisAsTraceId(DateTime.UtcNow.AddDays(1));
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(testSubject.AtTime, testResult.AtTime);
            Assert.AreEqual(testSubject.UniqueId, testResult.UniqueId);
            Assert.AreEqual(testSubject.Description, testResult.Description);

            var journalName = new VocaBase("Journal J1");
            testResult = testSubject.GetThisAsTraceId(DateTime.UtcNow.AddDays(1), journalName);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(testSubject.AtTime, testResult.AtTime);
            Assert.AreEqual(testSubject.UniqueId, testResult.UniqueId);
            Assert.AreEqual(journalName, testResult.Description);

            var testResultTrace = testResult.Trace;
            Assert.IsNotNull(testResultTrace);
            Assert.AreEqual(testSubject.AtTime, testResultTrace.AtTime);
            Assert.AreEqual(testSubject.UniqueId, testResultTrace.UniqueId);
            Assert.AreEqual(testSubject.Description, testResultTrace.Description);
        }
    }
}
