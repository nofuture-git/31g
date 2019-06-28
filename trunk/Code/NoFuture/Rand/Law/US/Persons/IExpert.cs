using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Persons
{
    [EtymologyNote("Latin", "'ex' + 'peritus'", "out-of + experienced, tested")]
    public interface IExpert<T> : ILegalPerson where T: IRationale
    {
        Predicate<T> IsSkilledOrKnowledgeableOf { get; set; }
    }
}
