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
        protected readonly Rchar[] format;

        private string _value;

        public DriversLicense(Rchar[] format)
        {
            this.format = format;
        }

        public virtual string Random
        {
            get
            {
                var dl = new char[format.Length];
                for (var i = 0; i < format.Length; i++)
                {
                    dl[i] = format[i].Rand;
                }
                return new string(dl);
            }
        }

        public virtual bool Validate(string dlValue)
        {
            return format.All(rc => rc.Valid(dlValue));
        }

        public override string Abbrev
        {
            get { return "DL"; }
        }

        public override string Value
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_value))
                    _value = Random;
                return _value;
            }
            set
            {
                _value = value;
                if (!Validate(_value))
                    throw new RahRowRagee(string.Format("The value given of '{0}' is not valid for this instance.", _value));
            }
        }

        public override List<Anomaly> Anomalies
        {
            get { return _anomalies; }
        }
    }

}
