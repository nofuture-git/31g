using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Gov
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

        public AmericanDeathCert(MannerOfDeath mannerOfDeath, IPerson person) : base(person)
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
        /// Generates a <see cref="DeathCert"/> at random based on the given <see cref="p"/>
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nullOnFutureDate">
        /// Switch parameter to have null returned whenever the random date-of-death is 
        /// in the future.  
        /// </param>
        /// <returns></returns>
        public static DeathCert GetRandomDeathCert(IPerson p, bool nullOnFutureDate = true)
        {
            if (p?.BirthCert == null)
                return null;

            var deathDate = AmericanUtil.GetDeathDate(p.BirthCert.DateOfBirth, p.MyGender);

            if (nullOnFutureDate && deathDate > DateTime.Now)
                return null;

            var manner = Etx.DiscreteRange(AmericanData.MannerOfDeathAvgs);
            return new AmericanDeathCert(manner, p) {DateOfDeath = deathDate};
        }
    }
}