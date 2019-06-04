using System;
using NUnit.Framework;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US.Property;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.Tests
{
    /// <summary>
    /// Murphy v. Steeplechase Amusement Co., 250 N.Y. 479 (N.Y. 1929)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, plaintiff understood risk, to continue means they contributed 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class MurphyvSteeplechaseAmusementCoTests
    {
        [Test]
        public void MurphyvSteeplechaseAmusementCo()
        {
            var test = new ContributoryNegligence<AmusementRide>(ExtensionMethods.Tortfeasor)
            {
                GetContribution = lp =>
                {
                    if (lp is SteeplechaseAmusementCo)
                        return new AmusementRide(1);
                    if (lp is Murphy)
                        return new AmusementRide(1);
                    return new AmusementRide(0);
                }
            };

            var testResult = test.IsValid(new Murphy(), new SteeplechaseAmusementCo());
            Assert.IsFalse(testResult);

            Console.WriteLine(test.ToString());
        }
    }

    public class Murphy : LegalPerson, IPlaintiff
    {
        public Murphy(): base("Murphy") { }
    }

    public class SteeplechaseAmusementCo : LegalPerson, ITortfeasor
    {
        public SteeplechaseAmusementCo(): base("Steeplechase Amusement Co.") { }
    }

    public class AmusementRide : IRankable
    {
        private readonly int _rank;

        public AmusementRide(int rank)
        {
            _rank = rank;
        }

        public int GetRank()
        {
            return _rank;
        }
    }
}
