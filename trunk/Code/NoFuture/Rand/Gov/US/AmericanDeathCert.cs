using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Gov.US
{
    /// <summary>
    /// https://www.cdc.gov/nchs/data/dvs/death11-03final-acc.pdf
    /// </summary>
    [Serializable]
    public class AmericanDeathCert : DeathCert
    {
        /// <summary>
        /// Item 37.
        /// </summary>
        public enum MannerOfDeath
        {
            Natural,
            Accident,
            Suicide,
            Homicide,
            PendingInvestigation,
            CouldNotBeDetermined
        }

        public AmericanDeathCert(MannerOfDeath mannerOfDeath, string personFullNamen) : base(personFullNamen)
        {
            CauseOfDeath = new Stack<string>();
            Category = mannerOfDeath;
        }

        /// <summary>
        /// 32. PART I. Enter the chain of events--diseases, injuries, or 
        /// complications--that directly caused the death. 
        /// </summary>
        public Stack<string> CauseOfDeath { get; }

        public MannerOfDeath Category { get; }

        public override string ToString()
        {
            return string.Join(" ", base.ToString(), Category);
        }

        /// <summary>
        /// Gets a date of death based on the <see cref="AmericanEquations.LifeExpectancy"/>
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        [RandomFactory]
        public static DateTime RandomDeathDate(DateTime dob, string gender)
        {
            var normDist = AmericanEquations.LifeExpectancy(gender);
            var ageAtDeath = Etx.RandomValueInNormalDist(normDist.Mean, normDist.StdDev);
            var years = (int)Math.Floor(ageAtDeath);
            var days = (int)Math.Round((ageAtDeath - years) * Constants.DBL_TROPICAL_YEAR);

            var deathDate =
                dob.AddYears(years)
                    .AddDays(days)
                    .AddHours(Etx.RandomInteger(0, 12))
                    .AddMinutes(Etx.RandomInteger(0, 59))
                    .AddSeconds(Etx.RandomInteger(0, 59));
            return deathDate;
        }
    }
}