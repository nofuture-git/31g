using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense
{
    /// <summary>
    /// Assumes the government asserted some incorrect fact in their paperwork submitted to the court
    /// </summary>
    public class Technicality : DefenseBase
    {
        public Technicality() : base(ExtensionMethods.Defendant) { }

        public Technicality(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        /// <summary>
        /// What the government asserted
        /// </summary>
        public ITermCategory AssertedFact { get; set; }

        /// <summary>
        /// What the actual fact-of-the-matter turned out to be.
        /// </summary>
        public ITermCategory ActualFact { get; set; }

        /// <summary>
        /// The enclosure that determines if what the <see cref="AssertedFact"/> equals <see cref="ActualFact"/>
        /// </summary>
        public Func<ITermCategory, ITermCategory, bool> IsMistaken { get; set; } = (t0, t1) => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (AssertedFact == null)
            {
                AddReasonEntry($"{nameof(AssertedFact)} returned null");
                return false;
            }

            if (ActualFact == null)
            {
                AddReasonEntry($"{nameof(ActualFact)} return null");
                return false;
            }

            var isMistaken = IsMistaken(AssertedFact, ActualFact);

            AddReasonEntry($"{nameof(IsMistaken)} returned {isMistaken} for " +
                           $"{nameof(AssertedFact)} '{AssertedFact}' " +
                           $"to {nameof(ActualFact)} '{ActualFact}'");
            return isMistaken;
        }
    }
}
