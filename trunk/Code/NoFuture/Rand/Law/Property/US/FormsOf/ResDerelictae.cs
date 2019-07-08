using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    [EtymologyNote("Latin", "res derelictae", "things abandoned")]
    public class ResDerelictae : LegalProperty
    {
        public ResDerelictae()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public ResDerelictae(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public ResDerelictae(string name, string groupName) : base(name, groupName) { }

        public ResDerelictae(ILegalProperty property) : base(property) { }
    }
}
