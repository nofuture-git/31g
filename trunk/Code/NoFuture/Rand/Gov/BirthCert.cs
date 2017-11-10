using System;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Gov
{
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
}