using NoFuture.Rand.Law.Criminal.US.Elements;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    /// <summary>
    /// A form of self defense where the <see cref="Imminence"/> is always valid in that 
    /// the immediate threat is always and continously present
    /// </summary>
    public class BatteredWomanSyndrome : Imminence
    {
        private readonly ICrime _crime;
        public BatteredWomanSyndrome(ICrime crime)
        {
            _crime = crime;
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = _crime.GetDefendant(persons) ?? GetDefendant(persons);
            if (defendant == null)
                return false;

            AddReasonEntry($"defendant, {defendant.Name}, imminence is {nameof(BatteredWomanSyndrome)}");

            return true;
        }
    }
}
