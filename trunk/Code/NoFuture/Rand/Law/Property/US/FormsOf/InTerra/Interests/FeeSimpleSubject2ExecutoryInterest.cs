using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    /// <summary>
    /// A shifting executory interest where the condition will shift ownership from one grantee to another grantee (neither being the grantor)
    /// </summary>
    public class FeeSimpleSubject2ExecutoryInterest : DefeasibleFee
    {
        public FeeSimpleSubject2ExecutoryInterest(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public FeeSimpleSubject2ExecutoryInterest() : base(null) { }

        public static IList<bool[]> FactoryPaths = new List<bool[]> { new[] { true, false, true } };

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}