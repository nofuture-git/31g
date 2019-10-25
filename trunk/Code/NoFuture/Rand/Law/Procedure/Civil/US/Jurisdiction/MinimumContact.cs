using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// Having some minimum contact would require one to return to the forum state.
    /// The contact derives from some voluntary relation within a state.  A contact
    /// in which the defendant stands to benefit from the protection of the law and is
    /// therefore accountable to the law.
    /// </summary>
    /// <remarks>
    /// Developed from <![CDATA["International Shoe v. Washington, 326 U.S. 310 (1945)"]]>
    /// for companies it typically means they are doing business within the state.
    /// </remarks>
    [Aka("purposeful availment")]
    public class MinimumContact : JurisdictionBase
    {
        public MinimumContact() { }
        public MinimumContact(string name) : base(name) { }

        /// <summary>
        /// A obvious or direct relationship between one person and another.
        /// </summary>
        public Func<ILegalPerson, ILegalPerson> GetDirectContactTo { get; set; } = lp => null;

        /// <summary>
        /// A contract binds one person to another
        /// </summary>
        public Func<ILegalPerson, ILegalPerson> GetContractedTo { get; set; } = lp => null;

        /// <summary>
        /// Calder v. Jones, 465 U.S. 783 (1984) - concerns the Plaintiff being effected by
        /// the intentional tort of the defendant then the defendant&apos;s domicile state can
        /// be the forum state 
        /// </summary>
        [Aka("Calder effects test")]
        public Func<ILegalPerson, ILegalPerson> GetIntentionalTortTo { get; set; } = lp => null;

        public Func<ILegalPerson, IVoca[]> GetCommerciallyEngagedLocation { get; set; } = lp => null;

        /// <summary>
        /// Non-passive website interacted within the forum state
        /// </summary>
        /// <remarks>
        /// <![CDATA[Zippo Manufacturing Co. v. Zippo Dot Com, Inc., 952 F. Supp. 1119 (W.D. Pa. 1997)]]>
        /// </remarks>
        public Func<ILegalPerson, IVoca[]> GetActiveVirtualContactLocation { get; set; } = lp => null;

        public override bool IsValid(params ILegalPerson[] persons)
        {

            var isDirectContactToResident = TestPerson2Name(
                Tuple.Create(nameof(GetDirectContactTo), GetDirectContactTo),
                Tuple.Create(nameof(GetDomicileLocation), GetDomicileLocation), persons);

            if (isDirectContactToResident)
                return true;
            
            var isContractedToResident = TestPerson2Name(
                Tuple.Create(nameof(GetContractedTo), GetContractedTo),
                Tuple.Create(nameof(GetDomicileLocation), GetDomicileLocation), persons);

            if (isContractedToResident)
                return true;

            var isTortfeaserToResident = TestPerson2Name(
                Tuple.Create(nameof(GetIntentionalTortTo), GetIntentionalTortTo),
                Tuple.Create(nameof(GetDomicileLocation), GetDomicileLocation), persons);

            if (isTortfeaserToResident)
                return true;

            var defendant = GetSubjectPerson(persons);
            if (defendant == null)
                return false;

            var title = defendant.GetLegalPersonTypeName();

            foreach (var voca in GetCommerciallyEngagedLocation(defendant) ?? new IVoca[]{})
            {
                var isCommerciallyEngaged = NameEquals(voca);
                if (isCommerciallyEngaged)
                {
                    AddReasonEntry($"{title} {defendant.Name}, {nameof(GetCommerciallyEngagedLocation)} " +
                                   $"returned a name whose {nameof(NameEquals)} is true for '{Name}'");
                    return true;
                }
            }

            foreach (var voca in GetActiveVirtualContactLocation(defendant) ?? new IVoca[] { })
            {
                var isVirtuallyEngaged = NameEquals(voca);
                if (isVirtuallyEngaged)
                {
                    AddReasonEntry($"{title} {defendant.Name}, {nameof(GetActiveVirtualContactLocation)} " +
                                   $"returned a name whose {nameof(NameEquals)} is true for '{Name}'");
                    return true;
                }
            }

            return false;

        }
    }
}
