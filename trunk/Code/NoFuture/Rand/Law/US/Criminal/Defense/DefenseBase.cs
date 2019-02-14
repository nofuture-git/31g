using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.US.Criminal.Defense
{
    public abstract class DefenseBase : LegalConcept
    {
        public ICrime Crime { get; }

        protected DefenseBase(ICrime crime)
        {
            Crime = crime;
        }

        public Func<IEnumerable<ILegalPerson>> OtherParties
        {
            get => Crime.OtherParties;
            set => Crime.OtherParties = value;
        }
    }
}
