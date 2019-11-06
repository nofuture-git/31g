using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// A contact in which the defendant stands to benefit from the
    /// protection of the law and is therefore accountable to the law.
    /// </summary>
    /// <remarks>
    /// Developed from <![CDATA["International Shoe v. Washington, 326 U.S. 310 (1945)"]]>
    /// for companies it typically means they are doing business within the state.
    /// </remarks>
    [Aka("purposeful availment", "specific in personam jurisdiction")]
    public class MinimumContact : JurisdictionBase
    {
        internal MinimumContact(): base(null) { }
        public MinimumContact(ICourt name) : base(name) { }

        /// <summary>
        /// A directed attempt of one person to contact another
        /// </summary>
        /// <remarks>
        /// This is one-way, when Foo travels to some other jurisdiction
        /// and attempts to sell a vacuum to Bar, then there is directed
        /// contact between Foo to Bar but not the other way (i.e. Bar to Foo).
        /// </remarks>
        public Func<ILegalPerson, ILegalPerson> GetDirectedContactTo { get; set; } = lp => null;

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
                Tuple.Create(nameof(GetDirectedContactTo), GetDirectedContactTo),
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

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var title = defendant.GetLegalPersonTypeName();

            var injuryLocation = GetInjuryLocation(defendant);
            var isInjuryLocation = NameEquals(injuryLocation);
            if (!isInjuryLocation)
                return false;

            foreach (var voca in GetCommerciallyEngagedLocation(defendant) ?? new IVoca[]{})
            {
                var isCommerciallyEngaged = NameEquals(voca);
                if(!isCommerciallyEngaged)
                    continue;

                AddReasonEntry($"{title} {defendant.Name}, {nameof(GetInjuryLocation)} returned '{injuryLocation.Name}'");
                AddReasonEntry($"{title} {defendant.Name}, {nameof(GetCommerciallyEngagedLocation)} returned '{voca.Name}'");
                AddReasonEntry($"'{voca.Name}' & '{Name}', {nameof(NameEquals)} is true");
                return true;
            }

            foreach (var voca in GetActiveVirtualContactLocation(defendant) ?? new IVoca[] { })
            {
                var isVirtuallyEngaged = NameEquals(voca);
                if(!isVirtuallyEngaged)
                    continue;
                AddReasonEntry($"{title} {defendant.Name}, {nameof(GetInjuryLocation)} returned '{injuryLocation.Name}'");
                AddReasonEntry($"{title} {defendant.Name}, {nameof(GetActiveVirtualContactLocation)} returned '{voca.Name}'");
                AddReasonEntry($"'{voca.Name}' & '{Name}', {nameof(NameEquals)} is true");
                return true;
            }

            return false;

        }
    }
}
