using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.Procedure.Civil.US
{
    public interface ILinkedLegalConcept : ILegalConcept
    {
        ILegalConcept LinkedTo { get; set; }
    }
}
