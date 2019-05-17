using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US
{
    public class NegligenceTort : LegalConcept
    {
        public Duty Duty { get; set; }

        public ILegalConcept Breach { get; set; }

        public Causation Causation { get; set; }

        public IAssault Contact { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var tortfeasor = persons.Tortfeasor();
            if (tortfeasor == null)
                return false;

            if (!Duty.IsValid(persons))
            {
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(Duty)} {nameof(IsValid)} is false");
                return false;
            }
            if (!Breach.IsValid(persons))
            {
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(Breach)} {nameof(IsValid)} is false");
                return false;
            }
            if (!Causation.IsValid(persons))
            {
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(Causation)} {nameof(IsValid)} is false");
                return false;
            }
            if (!Contact.IsValid(persons))
            {
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(Contact)} {nameof(IsValid)} is false");
                return false;
            }

            return true;
        }
    }
}
