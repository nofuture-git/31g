using System;
using System.Linq;
using NoFuture.Law.Constitutional.US;
using NoFuture.Law.Property.US.FormsOf.InTerra;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Law.Constitutional.Tests
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
            Func<ILegalPerson[], ILegalPerson> chargedWithDeprivation =
                lps => lps.FirstOrDefault(lp => lp is MooseLodge);


            var testSubject2 = new OperationOfPrivateClub()
            {
                FairlyDescribedAsStateActor = new StateAction2.TestIsStateActor(chargedWithDeprivation)
                {
                    IsTraditionalGovernmentFunction = a => !(a is OperationOfPrivateClub),
                }
            };

            var testResult2 = testSubject2.IsValid(new Irvis(), new MooseLodge());
            Console.WriteLine(testSubject2.ToString());
            Assert.IsFalse(testResult2);
        }
    }

    public class OperationOfPrivateClub : StateAction2 { }

    public class MooseLodge : LegalPerson, IPlaintiff
    {
        public MooseLodge(): base("MooseLodge") { }
    }

    public class Irvis : LegalPerson, IDefendant
    {
        public Irvis(): base("Irvis") { }
    }
}
