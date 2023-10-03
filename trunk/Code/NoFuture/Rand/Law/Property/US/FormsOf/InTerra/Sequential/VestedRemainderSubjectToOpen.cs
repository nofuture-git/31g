using System;
using System.Collections.Generic;
using NoFuture.Law.Attributes;

namespace NoFuture.Law.Property.US.FormsOf.InTerra.Sequential
{
    /// <summary>
    /// The Rule of Convenience is the idea the a future interest becomes actual upon any one qualified to make a claim and does so.
    /// </summary>
    [Aka("vested remainder subject to partial defeasance")]
    public class VestedRemainderSubjectToOpen : Remainder
    {
        public VestedRemainderSubjectToOpen(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public VestedRemainderSubjectToOpen() : base(null) {  }

        public static IList<bool[]> FactoryPaths = new List<bool[]> { new[] { false, false, false, true, true }, new[] { false, false, false, false, true, true } };

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}