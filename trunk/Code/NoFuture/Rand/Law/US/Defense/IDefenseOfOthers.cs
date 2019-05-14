namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// <![CDATA[
    /// under the circumstances as the actor believes them to be, the 
    /// person whom he seeks to protect would be justified in using 
    /// such protective force (Model Penal Code § 3.05(1) (b))
    /// ]]>
    /// </summary>
    public interface IDefenseOfOthers : ILegalConcept
    {
        /// <summary>
        /// <![CDATA[
        /// The subjective test where it reasonably appeared that the victim had a right to self-defense 
        /// and, thereby, that right is conveyed to the defendant
        /// ]]>
        /// </summary>
        SubjectivePredicate<ILegalPerson> IsReasonablyAppearedInjuryOrDeath { get; set; }

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