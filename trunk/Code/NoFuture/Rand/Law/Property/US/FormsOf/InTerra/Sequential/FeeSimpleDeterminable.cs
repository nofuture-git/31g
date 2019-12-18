using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Sequential
{
    /// <summary>
    /// estate in land that automatically expires upon the happening of stated even, not certain to occur
    /// </summary>
    public class FeeSimpleDeterminable : DefeasibleFee
    {
        public FeeSimpleDeterminable(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public FeeSimpleDeterminable() : base(null) { }

        public static IList<bool[]> FactoryPaths = new List<bool[]> { new[] { true, false, false, true } };

        /// <summary>
        /// The existence of an estate in fee simple determinable requires the presence of special limitations
        /// </summary>
        /// <remarks>
        /// often as &quot;durational language&quot; with words
        /// like, &quot;so long as&quot;, &quot;while&quot;,
        /// &quot;until&quot;, &quot;during&quot;, etc.
        /// </remarks>
        [Aka("particular circumstances")]
        public Predicate<ILegalPerson> IsSpecialLimitationPresent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }

            var title = subj.GetLegalPersonTypeName();

            if (!IsSpecialLimitationPresent(subj))
            {
                AddReasonEntry($"{title} {subj}, {nameof(IsSpecialLimitationPresent)} is false");
                return false;
            }

            return true;
        }
    }
}