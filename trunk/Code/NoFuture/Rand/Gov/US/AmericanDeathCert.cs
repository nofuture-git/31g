using System;
using System.Collections.Generic;

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

    }
}