using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.US.Courts;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    public abstract class FederalJurisdictionBase : JurisdictionBase
    {
        protected FederalJurisdictionBase(ICourt name) : base(name)
        {
        }

        protected bool IsFederalCourt()
        {
            if (Court is FederalCourt) 
                return true;

            AddReasonEntry($"{nameof(Court)}, '{Court?.Name}' is type " +
                           $"{Court?.GetType().Name} not type {nameof(FederalCourt)}");
            return false;

        }
    }
}
