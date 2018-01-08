using System;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public class AmericanBirthCert : BirthCert
    {
        public AmericanBirthCert(string personFullName) : base(personFullName)
        {
        }

        public string State { get; set; }
        public string City { get; set; }

        public override string ToString()
        {
            return string.Join(" ", base.ToString(), City, State);
        }
    }
}