using System;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.Criminal.US.Terms;
using NoFuture.Rand.Law.Criminal.US.Terms.Violence;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Katko v. Briney, 183 N.W.2d 657 (Iowa 1971)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, same as Bird v Holbrook, except Katko is a thief - still no allowed to use
    /// deadly force in defense of property.
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class KatkovBrineyTests
    {
        [Test]
        public void KatkovBriney()
        {
            var test = new DefenseOfProperty(ExtensionMethods.Tortfeasor)
            {
                Proportionality = new Proportionality<ITermCategory>(ExtensionMethods.Tortfeasor)
                {
                    GetChoice = lp =>
                    {
                        if (lp is Briney)
                            return new SeriousBodilyInjury();
                        return new NondeadlyForce();
                    }
                }
            };

            var testResult = test.IsValid(new Briney(), new Katko());
            Console.WriteLine(test.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class Katko : LegalPerson, IPlaintiff
    {
        public Katko(): base("") { }
    }

    public class Briney : LegalPerson, ITortfeasor
    {
        public Briney(): base("") { }
    }
}
