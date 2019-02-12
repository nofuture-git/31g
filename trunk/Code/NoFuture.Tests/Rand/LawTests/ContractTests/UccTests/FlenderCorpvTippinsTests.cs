using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US.Contracts.Ucc;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests.ContractTests.UccTests
{
    /// <summary>
    /// FLENDER CORPORATION v. TIPPINS INTERNATIONAL, INC. Superior Court of Pennsylvania 830 A.2d 1279 (Pa.Super. 2003)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Doctrine issue, demo of the Knockout rule where terms of the same name but different meanings also get excluded
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class FlenderCorpvTippinsTests
    {
        [Test]
        public void FlenderCorpvTippins()
        {
            var testSubject = new Agreement
            {
                IsApprovalExpressed = lp => true,
                TermsOfAgreement = lp =>
                {
                    var flender = lp as FlenderCorp;
                    if (flender != null)
                        return flender.GetTerms();
                    var tippins = lp as Tippins;
                    if (tippins != null)
                        return tippins.GetTerms();
                    return new HashSet<Term<object>>();
                }
            };
            var testResult = testSubject.GetAgreedTerms(new FlenderCorp(), new Tippins());
            Assert.IsNotNull(testResult);
            //knockout rule where two terms with different meanings are not included in set-union
            Assert.IsTrue(testResult.All(t => t.Name != "arbitration provision"));
            Console.WriteLine(testSubject.ToString());
        }
    }

    public class FlenderCorp : LegalPerson
    {
        public FlenderCorp() : base("FLENDER CORPORATION") { }
        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("invoice", new object()),
                new Term<object>("arbitration provision", new FedStateCourtsInIllinois()),
                new Term<object>("gear drive assemblies", new GearDriveAssemblies())
            };
        }
    }

    public class Tippins : LegalPerson
    {
        public Tippins() :base("Tippins International, Inc.") {}

        public ISet<Term<object>> GetTerms()
        {
            return new HashSet<Term<object>>
            {
                new Term<object>("purchase order", new object()),
                new Term<object>("arbitration provision", new ChamberCommAustria()),
                new Term<object>("gear drive assemblies", new GearDriveAssemblies())
            };
        }
    }

    public class GearDriveAssemblies
    {
        public override bool Equals(object obj)
        {
            return obj is GearDriveAssemblies;
        }

        public override int GetHashCode()
        {
            return "gear drive assemblies".GetHashCode();
        }
    }

    public class FedStateCourtsInIllinois
    {
        public override bool Equals(object obj)
        {
            return obj is FedStateCourtsInIllinois;
        }

        public override int GetHashCode()
        {
            return "Federal and/or State Courts located in Chicago, Illinois".GetHashCode();
        }
    }

    public class ChamberCommAustria
    {
        public override bool Equals(object obj)
        {
            return obj is ChamberCommAustria;
        }

        public override int GetHashCode()
        {
            return "International Chamber of Commerce in Vienna, Austria".GetHashCode();
        }
    }
}
