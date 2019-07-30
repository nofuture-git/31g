using System;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    public abstract class DefeasibleFee : LandPropertyInterestBase
    {
        protected DefeasibleFee(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        protected DefeasibleFee() : base(null) { }
    }
}
