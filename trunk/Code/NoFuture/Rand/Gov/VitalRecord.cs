using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Gov
{
    public interface IVitalRecord : IIdentifier<IPerson>
    {
        
    }
    [Serializable]
    public abstract class VitalRecord : IVitalRecord
    {
        protected VitalRecord(IPerson person)
        {
            Value = person;
        }
        public string Src { get; set; }
        public abstract string Abbrev { get; }
        public IPerson Value { get; set; }
    }
    [Serializable]
    public class BirthCert : VitalRecord
    {
        public BirthCert(IPerson person)
            : base(person)
        {
        }

        public override string Abbrev => "Birth certificate";
        public IPerson Mother { get; set; }
        public IPerson Father { get; set; }
        public DateTime DateOfBirth { get; set; }

        public override string ToString()
        {
            return DateOfBirth.ToString("yyyy-M-dd");
        }
    }

    [Serializable]
    public class DeathCert : VitalRecord
    {
        public DeathCert(IPerson person) : base(person)
        {
        }

        public override string Abbrev => "Death certificate";
        public DateTime DateOfDeath { get; set; }

        public override string ToString()
        {
            return DateOfDeath.ToString("yyyy-M-dd");
        }

    }

    [Serializable]
    public class AmericanBirthCert : BirthCert
    {
        public AmericanBirthCert(IPerson person) : base(person)
        {
        }

        public UsCityStateZip BirthPlace { get; set; }

        public override string ToString()
        {
            return string.Join(" ", base.ToString(), BirthPlace?.City, BirthPlace?.PostalState);
        }
    }

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

            var deathDate = NAmerUtil.GetDeathDate(p.BirthCert.DateOfBirth, p.MyGender);

            if (nullOnFutureDate && deathDate > DateTime.Now)
                return null;

            var manner = Etx.DiscreteRange(NAmerUtil.Tables.MannerOfDeathAvgs);
            return new AmericanDeathCert(manner, p) {DateOfDeath = deathDate};
        }
    }
}
