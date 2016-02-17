using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Exceptions;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public class DriversLicense : GovernmentId
    {
        private readonly List<Anomaly> _anomalies = new List<Anomaly>();

        public DriversLicense(Rchar[] format)
        {
            this.format = format;
        }

        public override string Abbrev
        {
            get { return "DL"; }
        }

        public override List<Anomaly> Anomalies
        {
            get { return _anomalies; }
        }
    }

}
