using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Persons
{
    [EtymologyNote("Latin", "'ex' + 'peritus'", "out-of + experienced, tested")]
    public interface IExpert<T> : ILegalPerson where T: IRationale
    {
        Predicate<T> IsSkilledOrKnowledgeableOf { get; set; }
    }
}
