using System;
using NoFuture.Rand.Law.Constitutional.US;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Constitutional.Tests
{
    /// <summary>
    /// Moose Lodge v. Irvis, 407 U.S. 163 (1972).
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Purely private party in private setting and discrimation being had is trival doesn't count
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MooseLodgevIrvisTests
    {
        [Test]
        public void MooseLodgevIrvis()
        {
            var testSubject = new StateAction()
            {
                Consent = Consent.NotGiven(),
                GetActByPerson = lp => new ServeDrinksDinner(),
                IsInvidiousDiscrimination = a => !(a is ServeDrinksDinner),
                IsProtectedRight = a => false,
                IsPublicCommunity = p => !(p is MooseLodgeNo107),
                SubjectProperty = new MooseLodgeNo107(),
            };

            var testResult = testSubject.IsValid(new MooseLodge(), new Irvis());
            Console.WriteLine(testSubject.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class MooseLodge : LegalPerson, IPlaintiff
    {
        public MooseLodge(): base("MooseLodge") { }
    }

    public class Irvis : LegalPerson, IDefendant
    {
        public Irvis(): base("Irvis") { }
    }

    public class MooseLodgeNo107 : RealProperty { }

    public class ServeDrinksDinner : Act
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
