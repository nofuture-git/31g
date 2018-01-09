using System;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public class DeathCert : VitalRecord
    {
        public DeathCert(string personFullName) : base(personFullName)
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