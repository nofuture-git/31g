using System;
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
}
