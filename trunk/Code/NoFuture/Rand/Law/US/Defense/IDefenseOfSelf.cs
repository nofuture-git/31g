namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// <![CDATA[
    /// The Model Penal Code defines self-defense in § 3.04(1) as "justifiable when the actor 
    /// believes that such force is immediately necessary for the purpose of protecting himself 
    /// against the use of unlawful force by such other person on the present occasion."
    /// ]]>
    /// </summary>
    public interface IDefenseOfSelf : ILegalConcept
    {
        /// <summary>
        /// (4) an objectively reasonable fear of injury or death
        /// </summary>
        ObjectivePredicate<ILegalPerson> IsReasonableFearOfInjuryOrDeath { get; set; }

        /// <summary>
        /// (1) an unprovoked attack
        /// </summary>
        Provocation Provocation { get; set; }

        /// <summary>
        /// (2) an attack which threatens imminent injury or death 
        ///     to a person or or damage, destruction, or theft to real or personal property
        /// </summary>
        Imminence Imminence { get; set; }

        /// <summary>
        /// (3) an objectively reasonable degree of force, used in response
        /// </summary>
        Proportionality<ITermCategory> Proportionality { get; set; }
    }
}