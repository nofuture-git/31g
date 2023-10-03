using System;
using System.Collections.Generic;
using NoFuture.Law.Attributes;

namespace NoFuture.Law.Property.US.FormsOf.InTerra.Sequential
{
    [Aka("indefeasibly vested remainder")]
    public class AbsolutelyVestedRemainder : Remainder
    {
        public AbsolutelyVestedRemainder(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public AbsolutelyVestedRemainder() : base(null) { }

        public static IList<bool[]> FactoryPaths = new List<bool[]> { new[] { false, false, false, false, false } };

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}