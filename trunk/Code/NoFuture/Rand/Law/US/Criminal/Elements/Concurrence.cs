using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Criminal.Elements
{
    [Aka("conduct")]
    public class Concurrence : ObjectiveLegalConcept, IElement
    {
        public MensRea MensRea { get; set; }
        public ActusReus ActusReus { get; set; }

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            if (MensRea == null)
            {
                AddReasonEntry("mens rea is required for any crime");
                return false;
            }

            if (ActusReus == null)
            {
                AddReasonEntry("actus rea is required for any crime");
                return false;
            }

            return MensRea.IsValid(offeror, offeree) && ActusReus.IsValid(offeror, offeree);
        }
    }
}
