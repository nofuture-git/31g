namespace NoFuture.Rand.Law.US.Criminal.Defense.Justification
{
    /// <summary>
    /// <![CDATA[
    /// The Model Penal Code defines self-defense in § 3.04(1) as "justifiable when the actor 
    /// believes that such force is immediately necessary for the purpose of protecting himself 
    /// against the use of unlawful force by such other person on the present occasion."
    /// ]]>
    /// </summary>
    public class DefenseOfSelf : DefenseOfBase
    {
        public DefenseOfSelf(ICrime crime) : base(crime)
        {
        }

        /// <summary>
        /// (4) an objectively reasonable fear of injury or death
        /// </summary>
        public ObjectivePredicate<ILegalPerson> IsReasonableFearOfInjuryOrDeath { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = Crime.GetDefendant(persons);
            if (defendant == null)
                return false;
            if (!base.IsValid(persons))
                return false;

            if (!IsReasonableFearOfInjuryOrDeath(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsReasonableFearOfInjuryOrDeath)} is false");
                return false;
            }

            return true;
        }
    }
}
