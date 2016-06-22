using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public class DriversLicense : StateIssuedId
    {
        private readonly List<Anomaly> _anomalies = new List<Anomaly>();
        
        public DriversLicense(Rchar[] format)
        {
            this.format = format;
        }

        public override string ToString()
        {
            return $"{this.IssuingState} {base.ToString()}";
        }

        public override string Abbrev => "DL";

        public override List<Anomaly> Anomalies => _anomalies;
    }

}
