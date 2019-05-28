using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.Tort.US.Elements.ReasonableCare;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Dellwo v. Pearson, 107 N.W.2d 859 (Minn. 1961)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class DellwovPearsonTests
    {
        [Test]
        public void DellwovPearson()
        {
            var test = new OfChildren(ExtensionMethods.Tortfeasor)
            {
                //failed to hold adult standard of care which was required
                IsAction = lp => !(lp is Pearson), 
                IsVoluntary = lp => lp is Pearson,
                IsUnderage = lp => lp is Pearson,
                Duty = new Duty(ExtensionMethods.Tortfeasor) {IsStatuteOrigin = lp => true}
            };
            var testResult = test.IsValid(new Dellwo(), new Pearson());
            Assert.IsFalse(testResult);
            Console.WriteLine(test.ToString());
        }
    }

    public class Dellwo : LegalPerson, IPlaintiff
    {
        public Dellwo(): base("Jeanette E. Dellwo") { }
    }

    public class Pearson : LegalPerson, ITortfeasor
    {
        public Pearson(): base("Pearson") { }
    }
}
