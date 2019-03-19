using System;
using NoFuture.Rand.Law.Criminal.Homicide.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Criminal.Tests.HomicideTests
{
    /// <summary>
    /// State v. West, 844 S.W.2d 144 (1992)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, the prosecution must prove premeditation 
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class StatevWestTests
    {
        [Test]
        public void StatevWest()
        {
            var testCrime = new Felony
            {
                ActusReus = new MurderFirstDegree
                {
                    IsCorpusDelicti = lp => lp is West,
                    IsPremediated = West.IsPremeditated

                },
                MensRea = new DeadlyWeapon("gun")
                {
                    IsUtilizable = lp => lp is West,
                    IsCanCauseDeath = lp => lp is West
                }
            };

            var testResult = testCrime.IsValid(new West());
            Console.WriteLine(testCrime.ToString());
            Assert.IsFalse(testResult);
        }
    }

    public class West : LegalPerson, IDefendant
    {
        public West(): base("WILLIAM WEST, APPELLANT.") { }

        /// <summary>
        /// no evidence, was just a theory
        /// </summary>
        public bool IsWentBackToHouseMeanPremeditation { get; set; } = false;

        /// <summary>
        /// no evidence except own testimony
        /// </summary>
        public bool IsPostEventCalmnessMeanPremeditation { get; set; } = false;

        /// <summary>
        /// court advises this only implies guilt, not premeditation
        /// </summary>
        public bool IsConcealWeaponMeanPremeditation { get; set; } = false;

        public static bool IsPremeditated(ILegalPerson lp)
        {
            var west = lp as West;
            if (west == null)
                return false;

            return west.IsConcealWeaponMeanPremeditation
                   || west.IsPostEventCalmnessMeanPremeditation
                   || west.IsWentBackToHouseMeanPremeditation
                ;
        }

    }
}
