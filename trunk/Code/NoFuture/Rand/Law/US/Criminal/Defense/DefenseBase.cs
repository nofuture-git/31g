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

        protected DefenseBase(ICrime crime)
        {
            Crime = crime;
        }
    }
}
