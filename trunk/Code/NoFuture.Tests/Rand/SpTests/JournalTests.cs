using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class JournalTests
    {
        [Test]
        public void TestThrowOnJournalizingInbalance()
        {
            IAccount<Identifier> testSubject = new NoFuture.Rand.Sp.Journal(new AccountId("test"));
            var dt = DateTime.Now;

            //no problems - sum is zero for date
            testSubject = testSubject.Debit(5000M.ToPecuniam(), new VocaBase("AssetAccount00"), dt)
                .Credit(5000M.ToPecuniam(), new VocaBase("LiabilityAccount00"));

            //compound entry - also no problems 
            testSubject = testSubject.Debit(15000M.ToPecuniam(), new VocaBase("AssetAccount00"), dt.AddDays(1))
                    .Credit(10000M.ToPecuniam(), new VocaBase("LiabilityAccount00"))
                    .Credit(5000M.ToPecuniam(), new VocaBase("EquityAccount00"));

            testSubject = testSubject.Debit(15000M.ToPecuniam(), new VocaBase("AssetAccount00"), dt.AddDays(3))
                .Credit(10000M.ToPecuniam(), new VocaBase("LiabilityAccount00"));

            //this should blow since the compound off-set of the previous is on a different day
            Assert.Throws<InvalidOperationException>(() =>
                testSubject.Credit(5000M.ToPecuniam(), new VocaBase("EquityAccount00"), dt.AddDays(4)));

            //turn off double-entry booking and its fine
            ((Journal) testSubject).IsDoubleBookEntry = false;
            testSubject.Credit(5000M.ToPecuniam(), new VocaBase("EquityAccount00"), dt.AddDays(4));
        }
    }
}
