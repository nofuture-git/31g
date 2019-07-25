using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    public class VestedRemainderSubjectToDivestment : Remainder
    {
        public VestedRemainderSubjectToDivestment(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public VestedRemainderSubjectToDivestment() : base(null) { }

        public static IList<bool[]> FactoryPaths = new List<bool[]> { };

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}