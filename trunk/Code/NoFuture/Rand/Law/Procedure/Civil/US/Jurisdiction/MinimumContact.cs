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

            var isDirectContactToResident = TestDefendant2Person2LocationIsCourt(
                Tuple.Create(nameof(GetDirectedContactTo), GetDirectedContactTo),
                Tuple.Create(nameof(GetDomicileLocation), GetDomicileLocation), persons);

            if (isDirectContactToResident)
                return true;
            
            var isContractedToResident = TestDefendant2Person2LocationIsCourt(
                Tuple.Create(nameof(GetContractedTo), GetContractedTo),
                Tuple.Create(nameof(GetDomicileLocation), GetDomicileLocation), persons);

            if (isContractedToResident)
                return true;

            var isTortfeaserToResident = TestDefendant2Person2LocationIsCourt(
                Tuple.Create(nameof(GetIntentionalTortTo), GetIntentionalTortTo),
                Tuple.Create(nameof(GetDomicileLocation), GetDomicileLocation), persons);

            if (isTortfeaserToResident)
                return true;

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var defendantHome = GetDomicileLocation(defendant);
            var isDefendantHomeState = NamesEqual(Court, defendantHome);

            var defendantTitle = defendant.GetLegalPersonTypeName();

            //you can sue a person in their home state
            if (isDefendantHomeState)
            {
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(GetDomicileLocation)} returned '{defendantHome.Name}'");
                AddReasonEntry($"'{defendantHome.Name}' & {nameof(Court)} '{Court.Name}', {nameof(NamesEqual)} is true");
                return true;
            }

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;

            var plaintiffTitle = plaintiff.GetLegalPersonTypeName();

            var injuryLocation = GetInjuryLocation(plaintiff);
            if (injuryLocation == null)
            {
                AddReasonEntry($"{plaintiffTitle} {plaintiff.Name}, {nameof(GetInjuryLocation)} returned nothing");
                return false;
            }

            foreach (var voca in GetCommerciallyEngagedLocation(defendant) ?? new IVoca[]{})
            {
                //defendant conducts biz here and it has a meaningful connection to plaintiff's claim
                var isCommerciallyEngaged = NamesEqual(Court, voca) && NamesEqual(Court, injuryLocation);
                if(!isCommerciallyEngaged)
                    continue;

                AddReasonEntry($"{plaintiffTitle} {plaintiff.Name}, {nameof(GetInjuryLocation)} returned '{injuryLocation.Name}'");
                AddReasonEntry($"'{injuryLocation.Name}' & {nameof(Court)} '{Court.Name}', {nameof(NamesEqual)} is true");
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(GetCommerciallyEngagedLocation)} returned '{voca.Name}'");
                AddReasonEntry($"'{voca.Name}' & {nameof(Court)} '{Court.Name}', {nameof(NamesEqual)} is true");
                return true;
            }

            foreach (var voca in GetActiveVirtualContactLocation(defendant) ?? new IVoca[] { })
            {
                //defendant has active virtual contact here and it has a meaningful connection to plaintiff's claim
                var isVirtuallyEngaged = NamesEqual(Court, voca) && NamesEqual(Court, injuryLocation);
                if (!isVirtuallyEngaged)
                    continue;

                AddReasonEntry($"{plaintiffTitle} {plaintiff.Name}, {nameof(GetInjuryLocation)} returned '{injuryLocation.Name}'");
                AddReasonEntry($"'{injuryLocation.Name}' & {nameof(Court)} '{Court.Name}', {nameof(NamesEqual)} is true");
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(GetActiveVirtualContactLocation)} returned '{voca.Name}'");
                AddReasonEntry($"'{voca.Name}' & {nameof(Court)} '{Court.Name}', {nameof(NamesEqual)} is true");
                return true;
            }

            return false;

        }
    }
}
