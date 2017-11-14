using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    [Serializable]
    public abstract class Pondus : IIdentifier<Pecuniam>
    {
        public IMereo Description { get; set; }

        public string Src { get; set; }
        public abstract string Abbrev { get; }
        public Pecuniam Value { get; set; }
    }
}
