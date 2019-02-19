namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    /// <summary>
    /// A form of self defense where the <see cref="Imminence"/> is always valid in that 
    /// the immediate threat is always and continously present
    /// </summary>
    public class BatteredWomanSyndrome : Imminence
    {
        public BatteredWomanSyndrome(ICrime crime) : base(crime)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = Crime.GetDefendant(persons);
            if (defendant == null)
                return false;

            AddReasonEntry($"defendant, {defendant.Name}, imminence is {nameof(BatteredWomanSyndrome)}");

            return true;
        }
    }
}
