using System;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Gov
{
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
}