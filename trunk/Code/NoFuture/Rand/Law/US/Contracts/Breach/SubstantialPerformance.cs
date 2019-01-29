using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Breach
{
    /// <summary>
    /// <![CDATA[
    /// mere technical, inadvertent or unimportant omissions or defects 
    /// would NOT amount to a breach
    /// ]]>
    /// </summary>
    [Aka("close-enough")]
    public class SubstantialPerformance : ObjectiveLegalConcept
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            throw new NotImplementedException();
        }

        public override bool IsEnforceableInCourt => true;
    }
}
