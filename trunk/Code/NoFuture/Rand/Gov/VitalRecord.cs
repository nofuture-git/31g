using System;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public abstract class VitalRecord : IVitalRecord
    {
        protected VitalRecord(string personFullName)
        {
            PersonFullName = personFullName;
        }
        public string Src { get; set; }
        public abstract string Abbrev { get; }
        public string PersonFullName { get; set; }
    }
}
