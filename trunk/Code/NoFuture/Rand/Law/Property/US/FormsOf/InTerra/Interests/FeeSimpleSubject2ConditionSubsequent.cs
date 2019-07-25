using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    public class FeeSimpleSubject2ConditionSubsequent : DefeasibleFee
    {
        public FeeSimpleSubject2ConditionSubsequent(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public FeeSimpleSubject2ConditionSubsequent() : base(null) { }

        public static IList<bool[]> FactoryPaths = new List<bool[]> { new[] { true, false, false, false } };

        /// <summary>
        /// The distinguishing factor that separates this from is close counterpart of <see cref="FeeSimpleDeterminable"/>
        /// </summary>
        /// <remarks>
        /// often as &quot;non-durational language&quot; with words
        /// like, &quot;but if&quot;, &quot;upon condition that&quot;, or
        /// explicitly saying, &quot;right of entry&quot;, &quot;right to enter
        /// and retake&quot;
        /// </remarks>
        public Predicate<ILegalPerson> IsRightOfEntry { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}