using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Sequential
{
    public class ContingentRemainder : Remainder
    {
        public ContingentRemainder(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public ContingentRemainder() : base(null) { }

        public static IList<bool[]> FactoryPaths = new List<bool[]> { new[] { false, false, false, true, false, true }, new[] { false, false, false, false, true, false, true } };

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}