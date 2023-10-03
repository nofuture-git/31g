using System;
using NoFuture.Law.Attributes;

namespace NoFuture.Law.Property.US.FormsOf.InTerra.Sequential
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