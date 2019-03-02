using System;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft;
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
                SubjectOfTheft = new LegalProperty("designer perfume"),
                AmountOfTheft = 5000m
            };

            var testResult = testAct.IsValid(new ChanelFenceEg(), new BurtThiefEg(), new SandraVictimEg());
            Console.WriteLine(testAct.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class ChanelFenceEg : LegalPerson
    {
        public ChanelFenceEg(): base("CHANEL FENCE") {  }
    }

    public class BurtThiefEg : LegalPerson
    {
        public BurtThiefEg() : base("BURT THIEF") { }
    }

    public class SandraVictimEg : LegalPerson
    {
        public SandraVictimEg() : base("SANDRA VICTIM") {  }
    }
}
