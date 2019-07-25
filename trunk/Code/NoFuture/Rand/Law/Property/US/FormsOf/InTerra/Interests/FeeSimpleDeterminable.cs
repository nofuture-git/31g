using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    /// <summary>
    /// estate in land that automatically expires upon the happening of stated even, not certain to occur
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
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
        /// often as &quot;durational language&quot; with words like, &quot;so long as&quot;, &quot;while&quot;, &quot;until&quot;, etc.
        /// </remarks>
        public Predicate<ILegalPerson> IsSpecialLimitationPresent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}