using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.US.Criminal
{
    /// <summary>
    /// criminal prosecution is always instituted by the government
    /// </summary>
    public class Government : LegalPerson
    {
        private static readonly Guid _uid = Guid.NewGuid();
        protected Government() { }

        private static Government _value;
        public static Government Value => _value ?? (_value = new Government());

        public override bool Equals(object obj)
        {
            return obj is Government;
        }

        public override int GetHashCode()
        {
            return _uid.GetHashCode();
        }

        public static ILegalPerson GetDefendant(ILegalPerson ror, ILegalPerson ree, IRationale obj)
        {
            ree = ree ?? Government.Value;
            ror = ror ?? Government.Value;
            ILegalPerson defendant = null;
            if (!ror.Equals(Government.Value) && ree.Equals(Government.Value))
                defendant = ror;
            if (ror.Equals(Government.Value) && !ree.Equals(Government.Value))
                defendant = ree;
            if (defendant == null)
            {
                obj?.AddReasonEntry($"it is not clear who the defendant is between {ror?.Name} and {ree?.Name}");
            }

            return defendant;
        }
    }
}
