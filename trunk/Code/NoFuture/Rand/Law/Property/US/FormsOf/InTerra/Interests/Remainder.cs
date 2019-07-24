using System;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    public abstract class Remainder : LifeEstate
    {
        protected Remainder(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        protected Remainder() : base(null) { }

    }
}