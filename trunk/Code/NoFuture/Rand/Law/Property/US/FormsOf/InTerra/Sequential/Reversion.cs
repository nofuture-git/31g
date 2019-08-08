using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Sequential
{
    public class Reversion : LifeEstate
    {
        public Reversion(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Reversion() : base(null) { }

        public static IList<bool[]> FactoryPaths = new List<bool[]> { new[] { false, false, true } };

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}