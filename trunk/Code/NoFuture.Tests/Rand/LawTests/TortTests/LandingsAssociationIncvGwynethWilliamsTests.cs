using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.UnintentionalTort;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Landings Association, Inc. v. Williams, 728 S.E.2d 577 (Ga. 2012)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, secondary assumed risk is just another form of contributory 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class LandingsAssociationIncvGwynethWilliamsTests
    {
        [Test]
        public void LandingsAssociationIncvGwynethWilliams()
        {
            var test = new ContributoryNegligence<Walking>(ExtensionMethods.Tortfeasor)
            {
                GetContribution = lp =>
                {
                    if (lp is GwynethWilliams)
                        return new Walking(1);
                    if (lp is LandingsAssociationInc)
                        return new Walking(10);
                    return new Walking(0);
                }
            };

            var testResult = test.IsValid(new LandingsAssociationInc(), new GwynethWilliams());
            Assert.IsFalse(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class LandingsAssociationInc : LegalPerson, ITortfeasor
    {
        public LandingsAssociationInc(): base("Landings Association, Inc.") { }
    }

    public class GwynethWilliams : LegalPerson, IPlaintiff
    {
        public GwynethWilliams(): base("Gwyneth Williams") { }
    }

    public class Walking : IRankable
    {
        private readonly int _rank;

        public Walking(int rank)
        {
            _rank = rank;
        }

        public int GetRank()
        {
            return _rank;
        }
    }
}
