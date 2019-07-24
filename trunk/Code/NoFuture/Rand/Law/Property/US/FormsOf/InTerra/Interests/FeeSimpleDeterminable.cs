using System;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    public class FeeSimpleDeterminable : DefeasibleFee
    {
        public FeeSimpleDeterminable(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public FeeSimpleDeterminable() : base(null) { }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}