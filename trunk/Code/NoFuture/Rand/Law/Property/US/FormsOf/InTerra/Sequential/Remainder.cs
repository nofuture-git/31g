using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    [Aka("future interest")]
    public abstract class Remainder : LifeEstate
    {
        protected Remainder(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        protected Remainder() : base(null) { }

    }
}