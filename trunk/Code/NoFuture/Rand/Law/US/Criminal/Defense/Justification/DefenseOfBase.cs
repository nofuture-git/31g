namespace NoFuture.Rand.Law.US.Criminal.Defense.Justification
{
    public abstract class DefenseOfBase : DefenseBase
    {
        protected DefenseOfBase(ICrime crime) : base(crime)
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
        ///     to a person or or damage, destruction, or theft to real or personal property
        /// </summary>
        public Imminence Imminence { get; set; }

        /// <summary>
        /// (3) an objectively reasonable degree of force, used in response
        /// </summary>
        public Proportionality<ITermCategory> Proportionality { get; set; }

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
            AddReasonEntryRange(Imminence?.GetReasonEntries());
            AddReasonEntryRange(Provacation?.GetReasonEntries());
            AddReasonEntryRange(Proportionality?.GetReasonEntries());
            return true;
        }
    }
}
