using System;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Gov
{
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