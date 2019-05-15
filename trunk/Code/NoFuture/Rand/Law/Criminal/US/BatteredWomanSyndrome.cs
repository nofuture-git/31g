using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US
{
    /// <summary>
    /// A form of self defense where the <see cref="Imminence"/> is always valid in that 
    /// the immediate threat is always and continuously present
    /// </summary>
    public class BatteredWomanSyndrome : Imminence
    {
        public BatteredWomanSyndrome(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) :base(getSubjectPerson)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            AddReasonEntry($"defendant, {defendant.Name}, imminence is {nameof(BatteredWomanSyndrome)}");

            return true;
        }
    }
}
