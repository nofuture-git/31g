using System;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Gov
{
    public interface IVitalRecord : IIdentifier<IPerson>
    {
        
    }

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

    public class BirthCert : VitalRecord
    {
        public BirthCert(IPerson person)
            : base(person)
        {
        }

        public override string Abbrev
        {
            get { return "Birth certificate"; }
        }
        public IPerson Mother { get; set; }
        public IPerson Father { get; set; }
        public DateTime DateOfBirth { get; set; }

        public override string ToString()
        {
            return DateOfBirth.ToString("yyyy-M-dd");
        }
    }

    public class AmericanBirthCert : BirthCert
    {
        public AmericanBirthCert(IPerson person) : base(person)
        {
        }

        public CityArea City { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", base.ToString(), City.AddressData.City, City.AddressData.StateAbbrv);
        }
        
    }
}
