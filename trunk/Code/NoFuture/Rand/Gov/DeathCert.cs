using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov
{
    /// <summary>
    /// A general form of a government Death Certificate
    /// </summary>
    [Serializable]
    public class DeathCert : VitalRecord
    {
        public DeathCert(string personFullName) : base(personFullName)
        {
        }

        public DeathCert(IVoca personName) : base(personName)
        {
        }

        public DeathCert(DateTime dateOfDeath)
        {
            DateOfDeath = dateOfDeath;
        }

        public DeathCert()
        {

        }

        public DateTime DateOfDeath { get; set; }

        public override string ToString()
        {
            return DateOfDeath.ToString("yyyy-M-dd");
        }

    }
}