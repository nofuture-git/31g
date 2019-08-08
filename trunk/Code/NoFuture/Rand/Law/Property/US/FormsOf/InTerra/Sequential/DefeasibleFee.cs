using System;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Sequential
{
    public abstract class DefeasibleFee : LandPropertyInterestBase
    {
        protected DefeasibleFee(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        protected DefeasibleFee() : base(null) { }
    }
}
