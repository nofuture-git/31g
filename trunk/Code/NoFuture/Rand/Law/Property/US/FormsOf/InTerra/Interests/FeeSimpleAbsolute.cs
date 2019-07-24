﻿using System;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    /// <summary>
    /// The simplest form of property interest - a kind of default which is assumed unless otherwise stated
    /// </summary>
    public class FeeSimpleAbsolute : PropertyBase, ILandPropertyInterest
    {
        public FeeSimpleAbsolute(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public FeeSimpleAbsolute() : base(null) { }

        public new RealProperty SubjectProperty { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }

    }
}
