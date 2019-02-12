using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.US.Criminal.Defense
{
    public abstract class DefenseBase : LegalConcept
    {
        public ICrime Crime { get; }

        public IEnumerable<ITermCategory> Terms { get; } = new List<ITermCategory>();

        protected DefenseBase(ICrime crime)
        {
            Crime = crime;
        }
    }
}
