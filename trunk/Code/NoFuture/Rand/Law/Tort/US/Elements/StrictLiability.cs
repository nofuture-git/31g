using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Property;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    public class StrictLiability : PropertyConsent
    {
        public StrictLiability(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalProperty> IsDefectiveAtTimeOfSale { get; set; } = p => false;
        
        public Causation Causation { get; set; }
        
        public IInjury Injury { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }

    }
}
