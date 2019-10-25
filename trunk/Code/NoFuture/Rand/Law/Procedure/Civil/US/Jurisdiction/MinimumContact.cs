using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// Having some minimum contact would require one to return to the forum state.
    /// The contact derives from some voluntary relation of a defendant within a state.
    /// </summary>
    /// <remarks>
    /// Developed from <![CDATA["International Shoe v. Washington, 326 U.S. 310 (1945)"]]>
    /// for companies it typically means they are doing business within the state
    /// merely owning property is not sufficient.
    /// </remarks>
    public class MinimumContact : UnoHomine
    {
        public MinimumContact(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public MinimumContact() : base(ExtensionMethods.DefendantFx) { }

        public Predicate<ILegalPerson> IsDirectContact { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsContractedWithResident { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsCommerciallyEngaged { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsSeekingToServe { get; set; } = lp => false;

        /// <summary>
        /// Calder v. Jones, 465 U.S. 783 (1984) - concerns the Plaintiff being effected by
        /// the intentional tort of the defendant then the defendant can sue in their state 
        /// </summary>
        [Aka("Calder effects test")]
        public Predicate<ILegalPerson> IsIntentionalTort { get; set; } = lp => false;

        /// <summary>
        /// Non-passive website interacted within the forum state
        /// </summary>
        /// <remarks>
        /// <![CDATA[Zippo Manufacturing Co. v. Zippo Dot Com, Inc., 952 F. Supp. 1119 (W.D. Pa. 1997)]]>
        /// </remarks>
        public Predicate<ILegalPerson> IsActiveVirtualContact { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetSubjectPerson(persons);
            if (defendant == null)
                return false;

            var title = defendant.GetLegalPersonTypeName();

            var predicateTuple = new[]
            {
                Tuple.Create(IsDirectContact, nameof(IsDirectContact)),
                Tuple.Create(IsContractedWithResident, nameof(IsContractedWithResident)),
                Tuple.Create(IsCommerciallyEngaged, nameof(IsCommerciallyEngaged)),
                Tuple.Create(IsSeekingToServe, nameof(IsSeekingToServe)),
                Tuple.Create(IsIntentionalTort, nameof(IsIntentionalTort)),
                Tuple.Create(IsActiveVirtualContact, nameof(IsActiveVirtualContact)),
            };

            foreach (var tuple in predicateTuple)
            {
                if (tuple.Item1(defendant))
                {
                    AddReasonEntry($"{title} {defendant.Name}, {tuple.Item2} is true");
                    return true;
                }
            }

            return false;
        }
    }
}
