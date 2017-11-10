using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public abstract class GovernmentId : RIdentifier
    {
        public DateTime? IssuedDate { get; set; }
    }
}