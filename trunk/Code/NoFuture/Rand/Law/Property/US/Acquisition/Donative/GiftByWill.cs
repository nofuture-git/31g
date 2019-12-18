using System;

namespace NoFuture.Rand.Law.Property.US.Acquisition.Donative
{
    public class GiftByWill : PropertyConsent
    {
        public GiftByWill(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}
