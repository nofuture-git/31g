using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.US.Contracts.Breach
{
    public class Excuse : ObjectiveLegalConcept
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            throw new NotImplementedException();
        }

        public override bool IsEnforceableInCourt => true;
    }
}
