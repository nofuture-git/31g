using System;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    public class FeeSimpleSubject2ExecutoryInterest : DefeasibleFee
    {
        public FeeSimpleSubject2ExecutoryInterest(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public FeeSimpleSubject2ExecutoryInterest() : base(null) { }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}