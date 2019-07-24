﻿using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    [Aka("indefeasibly vested remainder")]
    public class AbsolutelyVestedRemainder : Remainder
    {
        public AbsolutelyVestedRemainder(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public AbsolutelyVestedRemainder() : base(null) { }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}