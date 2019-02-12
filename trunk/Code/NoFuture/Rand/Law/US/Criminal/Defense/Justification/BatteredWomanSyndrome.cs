namespace NoFuture.Rand.Law.US.Criminal.Defense.Justification
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

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            AddReasonEntry($"defendant, {defendant.Name}, imminence is {nameof(BatteredWomanSyndrome)}");

            return true;
        }
    }
}
