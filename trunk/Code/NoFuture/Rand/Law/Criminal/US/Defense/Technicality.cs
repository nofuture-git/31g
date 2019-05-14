using System;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense
{
    /// <inheritdoc cref="ITechnicality"/>
    public class Technicality : DefenseBase, ITechnicality
    {
        public ITermCategory AssertedFact { get; set; }

        public ITermCategory ActualFact { get; set; }

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
