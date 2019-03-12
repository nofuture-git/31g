using System;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft;
using NoFuture.Rand.Law.Criminal.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.TheftTests
{
    [TestFixture]
    public class ExampleByReceivingTests
    {
        [Test]
        public void ExampleByReceivingActTest()
        {
            var testAct = new ByReceiving
            {
                IsPresentStolen = lp => lp is ChanelFenceEg || lp is BurtThiefEg,
                IsTakenPossession = lp => lp is ChanelFenceEg || lp is BurtThiefEg || lp is SandraVictimEg,
                SubjectProperty = new LegalProperty("designer perfume"){ PropretyValue = 5000m },
            };

            var testResult = testAct.IsValid(new ChanelFenceEg(), new BurtThiefEg(), new SandraVictimEg());
            Console.WriteLine(testAct.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class ChanelFenceEg : LegalPerson, IDefendant
    {
        public ChanelFenceEg(): base("CHANEL FENCE") {  }
    }

    public class BurtThiefEg : LegalPerson, IDefendant
    {
        public BurtThiefEg() : base("BURT THIEF") { }
    }

    public class SandraVictimEg : Victim
    {
        public SandraVictimEg() : base("SANDRA VICTIM") {  }
    }
}
