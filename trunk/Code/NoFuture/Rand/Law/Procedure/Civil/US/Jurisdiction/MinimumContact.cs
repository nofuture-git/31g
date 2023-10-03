using System;
using NoFuture.Law;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;

namespace NoFuture.Law.Procedure.Civil.US.Jurisdiction
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

        /// <summary>
        /// Locations in which a person is actively commercially engaged.
        /// </summary>
        /// <remarks>
        /// Simply owning property or the like is considered passive and does not count
        /// </remarks>
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

            if (IsDirectContactToResident(persons))
            {
                return true;
            }

            if (IsContractedToResident(persons))
            {
                return true;
            }

            if (IsTortfeaserToResident(persons))
            {
                return true;
            }

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var defendantTitle = defendant.GetLegalPersonTypeName();

            if (IsCourtDomicileLocationOfDefendant(defendant, defendantTitle))
            {
                return true;
            }

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;

            var plaintiffTitle = plaintiff.GetLegalPersonTypeName();

            if (IsCommerciallyEngagedConnectedToInjuryLocation(defendant, plaintiff, defendantTitle, plaintiffTitle))
            {
                return true;
            }

            if (IsVirtualContactConnectedToInjuryLocation(defendant, plaintiff, defendantTitle, defendantTitle))
            {
                return true;
            }

            return false;

        }

        protected internal virtual bool IsDirectContactToResident(ILegalPerson[] persons)
        {
            return TestDefendant2Person2LocationIsCourt(
                Tuple.Create(nameof(GetDirectedContactTo), GetDirectedContactTo),
                Tuple.Create(nameof(GetDomicileLocation), GetDomicileLocation), persons);
        }

        protected internal virtual bool IsContractedToResident(ILegalPerson[] persons)
        {
            return TestDefendant2Person2LocationIsCourt(
                Tuple.Create(nameof(GetContractedTo), GetContractedTo),
                Tuple.Create(nameof(GetDomicileLocation), GetDomicileLocation), persons);
        }

        protected internal virtual bool IsTortfeaserToResident(ILegalPerson[] persons)
        {
            return TestDefendant2Person2LocationIsCourt(
                Tuple.Create(nameof(GetIntentionalTortTo), GetIntentionalTortTo),
                Tuple.Create(nameof(GetDomicileLocation), GetDomicileLocation), persons);
        }

        protected internal virtual bool IsCommerciallyEngagedConnectedToInjuryLocation(ILegalPerson defendant, ILegalPerson plaintiff,
            string defendantTitle = null, string plaintiffTitle = null)
        {
            defendantTitle = defendantTitle ?? defendant.GetLegalPersonTypeName();
            plaintiffTitle = plaintiffTitle ?? plaintiff.GetLegalPersonTypeName();

            var injuryLocation = GetInjuryLocation(plaintiff);
            if (injuryLocation == null)
            {
                AddReasonEntry($"{plaintiffTitle} {plaintiff.Name}, {nameof(GetInjuryLocation)} returned nothing");
                return false;
            }

            foreach (var voca in GetCommerciallyEngagedLocation(defendant) ?? new IVoca[] { })
            {
                //defendant conducts biz here and it has a meaningful connection to plaintiff's claim
                var isCommerciallyEngaged = NamesEqual(Court, voca) && NamesEqual(Court, injuryLocation);
                if (!isCommerciallyEngaged)
                    continue;

                AddReasonEntry($"{plaintiffTitle} {plaintiff.Name}, {nameof(GetInjuryLocation)} returned '{injuryLocation.Name}'");
                AddReasonEntry($"'{injuryLocation.Name}' & {nameof(Court)} '{Court.Name}', {nameof(NamesEqual)} is true");
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(GetCommerciallyEngagedLocation)} returned '{voca.Name}'");
                AddReasonEntry($"'{voca.Name}' & {nameof(Court)} '{Court.Name}', {nameof(NamesEqual)} is true");
                return true;
            }

            return false;
        }

        protected internal virtual bool IsVirtualContactConnectedToInjuryLocation(ILegalPerson defendant, ILegalPerson plaintiff,
            string defendantTitle = null, string plaintiffTitle = null)
        {
            defendantTitle = defendantTitle ?? defendant.GetLegalPersonTypeName();
            plaintiffTitle = plaintiffTitle ?? plaintiff.GetLegalPersonTypeName();

            var injuryLocation = GetInjuryLocation(plaintiff);
            if (injuryLocation == null)
            {
                AddReasonEntry($"{plaintiffTitle} {plaintiff.Name}, {nameof(GetInjuryLocation)} returned nothing");
                return false;
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

        /// <summary>
        /// Helper method reduce redundant code
        /// </summary>
        /// <param name="defendant2Person">
        /// Item1 is just the name you want to appear in the reasons (e.g. nameof(SuchAndSuch)).
        /// Item2 is a function to resolve the subject person (e.g. defendant, tortfeaser) to some other person (plaintiff, victim, etc.)
        /// </param>
        /// <param name="person2Location">
        /// Item1 is just the name you want to appear in the reasons (e.g. nameof(SuchAndSuch)).
        /// Item2 is a function to resolve the other person (plaintiff, victim, etc.) to some
        ///       name\location to compare to to <see cref="CivilProcedureBase.Court"/> name
        /// </param>
        /// <param name="persons">
        /// The legal persons passed into the calling function (e.g. IsValid)
        /// </param>
        protected virtual bool TestDefendant2Person2LocationIsCourt(
            Tuple<string, Func<ILegalPerson, ILegalPerson>> defendant2Person,
            Tuple<string, Func<ILegalPerson, IVoca>> person2Location,
            params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null || defendant2Person?.Item2 == null || person2Location?.Item2 == null)
                return false;

            var title = defendant.GetLegalPersonTypeName();

            var otherPerson = defendant2Person.Item2(defendant);

            var someFunctionName = defendant2Person.Item1;

            if (otherPerson == null)
                return false;

            var otherTitle = otherPerson.GetLegalPersonTypeName();

            var someOtherFunctionName = person2Location.Item1;

            var location = person2Location.Item2(otherPerson);
            if (NamesEqual(Court, location))
            {
                AddReasonEntry($"{title} {defendant.Name}, {someFunctionName} returned person '{otherPerson.Name}'");
                AddReasonEntry($"{otherTitle} {otherPerson.Name}, {someOtherFunctionName} returned '{location.Name}'");
                AddReasonEntry($"'{location.Name}' & {nameof(Court)} '{Court.Name}', {nameof(NamesEqual)} is true");
                return true;
            }

            return false;
        }
    }
}
