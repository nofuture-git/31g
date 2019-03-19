using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Contract.US;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Contract.Tests.AcceptanceTests
{
    /// <summary>
    /// MUNICIPAL CONSULTANTS & PUBLISHERS, INC. v. TOWN OF RAMAPO Court of Appeals of New York 47 N.Y.2d 144, 390 N.E.2d 1143, 417 N.Y.S.2d 218 (1979)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// A case where a contract was accepted prior to having been signed with signature.
    /// [(...) where the parties contemplate that a signed writing is required, there is no contract 
    /// until one is delivered. Scheck v. Francis, 260 N.E.2d 493 (N.Y. 1970).]
    /// This was not the case here because: 
    ///  * parties have agreed on all contractual terms
    ///  * there is nothing left for future settlement
    ///  * there is no understanding that a signature is required
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MunicipalvTownOfRamapoTests
    {
        [Test]
        public void MunicipalvTownOfRamapo()
        {
            var testSubject = new ComLawContract<Promise>
            {
                Offer = new OfferEncodeTownLaws(),
                Acceptance = o => o is OfferEncodeTownLaws ? new Acceptance2Municipal() : null,
                Assent = new MutualAssent
                {
                    IsApprovalExpressed = lp =>
                    {
                        var townOfRamapo = lp as TownOfRamapo;
                        if (townOfRamapo == null)
                        {
                            return lp is Municipal;
                        }

                        //court finds that supervisor signature was not required
                        return townOfRamapo.IsAcceptedByTownAttorney;
                    },
                    TermsOfAgreement = lp => new HashSet<Term<object>>{new Term<object>("",DBNull.Value)}
                }
            };
            testSubject.Consideration = new Consideration<Promise>(testSubject)
            {
                IsSoughtByOfferor = (lp, p) => true,
                IsGivenByOfferee = (lp, p) => true
            };

            var testResult = testSubject.IsValid(new Municipal(), new TownOfRamapo());
            Console.WriteLine(testSubject.ToString());
            Assert.IsTrue(testResult);

        }
    }

    public class OfferEncodeTownLaws : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is Municipal && offeree is TownOfRamapo;
        }
    }

    public class Acceptance2Municipal : Promise
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.FirstOrDefault();
            var offeree = persons.Skip(1).Take(1).FirstOrDefault();
            return offeror is Municipal && offeree is TownOfRamapo;
        }
    }

    public class Municipal : LegalPerson, IOfferor
    {
        public Municipal() : base("MUNICIPAL CONSULTANTS & PUBLISHERS, INC.") {}
    }

    public class TownOfRamapo : LegalPerson, IOfferee
    {
        public TownOfRamapo() : base("TOWN OF RAMAPO") {}
        public bool IsAcceptedByTownAttorney => true;
        public bool IsSignedBySupervisor => false;
    }
}
