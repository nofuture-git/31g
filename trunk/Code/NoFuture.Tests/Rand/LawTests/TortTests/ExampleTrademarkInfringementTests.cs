using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Law.Property.US.FormsOf.Intellectus;
using NoFuture.Law.Property.US.Terms;
using NoFuture.Law.Property.US.Terms.Tm;
using NoFuture.Law.Tort.US.IntentionalTort;
using NoFuture.Law.US;
using NUnit.Framework;

namespace NoFuture.Law.Tort.Tests
{
    [TestFixture]
    public class ExampleTrademarkInfringementTests
    {
        [Test]
        public void TestTrademarkInfringementIsValid()
        {
            var plaintiff = new SomePlaintiff();
            var defendant = new SomeDefendant();
            var strongTrademark = new StrongTrademark()
            {
                IsEntitledTo = lp => lp.IsSamePerson(plaintiff),
                IsInPossessionOf = lp => lp.IsSamePerson(plaintiff),
                Name = "STRONG MARK"
            };
            var weakTrademark = new WeakTrademark()
            {
                IsEntitledTo = lp => lp.IsSamePerson(defendant),
                IsInPossessionOf = lp => lp.IsSamePerson(defendant),
                Name = "WEAK MARK"
            };
            var test = new TrademarkInfringement(ExtensionMethods.Defendant)
            {
                GetChoice = lp =>
                {
                    if(lp is SomePlaintiff)
                        return strongTrademark;
                    if (lp is SomeDefendant)
                        return weakTrademark;
                    return null;
                },
                IsProportional = (t1, t2) => (t1 is StrongTrademark && t2 is WeakTrademark) 
                                             || (t1 is WeakTrademark && t2 is StrongTrademark),
                IsActualConfusionExist = true
            };

            var testResult = test.IsValid(plaintiff, defendant);
            Console.WriteLine(test.ToString());
            Assert.IsTrue(testResult);
        }
    }

    public class StrongTrademark : Trademark
    {
        public override StrengthOfMark GetStrengthOfMark()
        {
            return new FancifulMark();
        }
    }

    public class WeakTrademark : Trademark
    {
        public override StrengthOfMark GetStrengthOfMark()
        {
            return new GenericMark();
        }
    }
}
