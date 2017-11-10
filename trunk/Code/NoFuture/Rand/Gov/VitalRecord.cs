using System;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Gov
{
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
}
