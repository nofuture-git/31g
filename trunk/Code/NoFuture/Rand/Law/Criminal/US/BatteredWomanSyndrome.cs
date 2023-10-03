using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US
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
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();

            AddReasonEntry($"{title}, {defendant.Name}, imminence is {nameof(BatteredWomanSyndrome)}");

            return true;
        }
    }
}
