using System;
using NoFuture.Rand.Law.Exceptions;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Justification
{
    /// <summary>
    /// A form of self defense where the <see cref="Imminence"/> is always valid in that 
    /// the immediate threat is always and continously present
    /// </summary>
    public class BatteredWomanSyndrome : DefenseOfSelf
    {
        public BatteredWomanSyndrome(ICrime crime) : base(crime)
        {
        }

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;
            if (Imminence == null || !Imminence.IsValid(defendant))
            {
                throw new LawException($"defendant, {defendant.Name}, for " +
                                       $"type {nameof(BatteredWomanSyndrome)}, by " +
                                       $"definition {nameof(Imminence)} is always and continously present");
            }

            return base.IsValid(offeror, offeree);
        }
    }
}
