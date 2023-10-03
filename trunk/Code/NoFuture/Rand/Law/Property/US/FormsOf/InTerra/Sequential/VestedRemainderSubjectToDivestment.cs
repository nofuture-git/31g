using System;
using System.Collections.Generic;

namespace NoFuture.Law.Property.US.FormsOf.InTerra.Sequential
{
    public class VestedRemainderSubjectToDivestment : Remainder
    {
        public VestedRemainderSubjectToDivestment(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public VestedRemainderSubjectToDivestment() : base(null) { }

        public static IList<bool[]> FactoryPaths = new List<bool[]>
        {
            new[] {false, false, false, true, false, false},
            new[] {false, false, false, false, true, false, false}
        };

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}