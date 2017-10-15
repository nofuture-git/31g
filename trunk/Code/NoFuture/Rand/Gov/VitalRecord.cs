using System;
using System.Collections.Generic;
using NoFuture.Rand.Data.Types;
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
        /// DO NOT enter terminal events such as cardiac
        /// arrest, respiratory arrest, or ventricular fibrillation 
        /// without showing the etiology.DO NOT ABBREVIATE. Enter 
        /// only one cause on a line.Add additional
        /// lines if necessary.
        /// </summary>
        public Stack<string> CauseOfDeath { get; }

        public MannerOfDeath Category { get; }

        public override string ToString()
        {
            return string.Join(" ", base.ToString(), Category);
        }
    }
}
