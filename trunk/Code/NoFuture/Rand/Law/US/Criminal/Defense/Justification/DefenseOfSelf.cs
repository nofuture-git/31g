using NoFuture.Rand.Law.US.Criminal.Terms;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Justification
{
    /// <summary>
    /// <![CDATA[
    /// The Model Penal Code defines self-defense in § 3.04(1) as "justifiable when the actor 
    /// believes that such force is immediately necessary for the purpose of protecting himself 
    /// against the use of unlawful force by such other person on the present occasion."
    /// ]]>
    /// </summary>
    public class DefenseOfSelf : DefenseBase
    {
        public DefenseOfSelf(ICrime crime) : base(crime)
        {
            Provacation = new Provacation(crime);
            Imminence = new Imminence(crime);
            Proportionality = new Proportionality<ITermCategory>(crime);
        }

        /// <summary>
        /// (1) an unprovoked attack
        /// </summary>
        public Provacation Provacation { get; set; }

        /// <summary>
        /// (2) an attack which threatens imminent injury or death
        /// </summary>
        public Imminence Imminence { get; set; }

        /// <summary>
        /// (3) an objectively reasonable degree of force, used in response
        /// </summary>
        public Proportionality<ITermCategory> Proportionality { get; set; }

        /// <summary>
        /// (4) an objectively reasonable fear of injury or death
        /// </summary>
        public ObjectivePredicate<ILegalPerson> IsReasonableFearOfInjuryOrDeath { get; set; } = lp => false;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;
            if (Imminence != null && !Imminence.IsValid(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Imminence)} is false");
                AddReasonEntryRange(Imminence.GetReasonEntries());
                return false;
            }
            if (Provacation != null && !Provacation.IsValid(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Provacation)} is false");
                AddReasonEntryRange(Provacation.GetReasonEntries());
                return false;
            }
            if (Proportionality != null && !Proportionality.IsValid(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Proportionality)} is false");
                AddReasonEntryRange(Proportionality.GetReasonEntries());
                return false;
            }

            if (!IsReasonableFearOfInjuryOrDeath(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsReasonableFearOfInjuryOrDeath)} is false");
                return false;
            }

            AddReasonEntryRange(Imminence?.GetReasonEntries());
            AddReasonEntryRange(Provacation?.GetReasonEntries());
            AddReasonEntryRange(Proportionality?.GetReasonEntries());

            return true;
        }
    }
}
