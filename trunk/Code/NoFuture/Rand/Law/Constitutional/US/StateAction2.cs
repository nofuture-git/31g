using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Constitutional.US
{
    /// <summary>
    /// The US Constitution doctrine of when a person is protected against incursions by government 
    /// </summary>
    public class StateAction2 : Act
    {
        public StateAction2() : base(ExtensionMethods.Defendant)
        {

        }

        /// <summary>
        /// Edmonson v. Leesville Concrete Company, Inc. 500 U.S. 614 (1991)
        /// whether the claimed constitutional deprivation resulted from the exercise
        /// of a right or privilege having its source in state authority
        /// </summary>
        public virtual Predicate<ILegalPerson> IsSourceStateAuthority { get; set; } = lp => false;

        /// <summary>
        /// Edmonson v. Leesville Concrete Company, Inc. 500 U.S. 614 (1991)
        /// whether the private party charged with the deprivation could be described in all fairness as a state actor
        /// </summary>
        public virtual TestIsStateActor FairlyDescribedAsStateActor { get; set; }

        /// <summary>
        /// Who is being charged with deprivation of a constitutional right - will default to <see cref="UnoHomine.GetSubjectPerson"/>
        /// </summary>
        public virtual Func<ILegalPerson[], ILegalPerson> GetPartyChargedWithDeprivation { get; set; } =
            lps => null;

        /// <summary>  Defaults to true  </summary>
        public override Predicate<ILegalPerson> IsAction { get; set; } = p => true;

        /// <summary> Defaults to true </summary>
        public override Predicate<ILegalPerson> IsVoluntary { get; set; } = p => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            GetPartyChargedWithDeprivation = GetPartyChargedWithDeprivation ?? GetSubjectPerson;

            if (GetPartyChargedWithDeprivation == null)
            {
                AddReasonEntry($"{nameof(GetPartyChargedWithDeprivation)} is null");
                return false;
            }

            var chargedPerson = GetPartyChargedWithDeprivation(persons);

            if (chargedPerson == null)
            {
                AddReasonEntry($"{nameof(GetPartyChargedWithDeprivation)} returned null for all persons: {persons.GetTitleNamePairs()}");
                return false;
            }

            var chargedPersonTitle = chargedPerson.GetLegalPersonTypeName();

            var lugerTestOne = IsSourceStateAuthority(chargedPerson);
            AddReasonEntry($"{chargedPersonTitle} {chargedPerson.Name}, {nameof(IsSourceStateAuthority)} returned {lugerTestOne}");

            if (FairlyDescribedAsStateActor == null)
            {
                AddReasonEntry($"{nameof(FairlyDescribedAsStateActor)} is null");
                return false;
            }

            if (FairlyDescribedAsStateActor.GetSubjectPerson == null)
                FairlyDescribedAsStateActor.GetSubjectPerson = GetPartyChargedWithDeprivation;

            if (FairlyDescribedAsStateActor.Action == null)
                FairlyDescribedAsStateActor.Action = this;
            
            var lugerTestTwo = FairlyDescribedAsStateActor.IsValid(persons);

            AddReasonEntryRange(FairlyDescribedAsStateActor.GetReasonEntries());

            return lugerTestOne && lugerTestTwo;
        }

        /// <summary>
        /// The second part of the Lugar test
        /// </summary>
        public class TestIsStateActor : UnoHomine
        {
            public TestIsStateActor(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
            {
            }

            public IAct Action { get; set; }

            /// <summary>
            /// When private parties make extensive use of state procedures
            /// with &quot;the overt, significant assistance of state officials.&quot;
            /// </summary>
            /// <remarks>
            /// Tulsa Professional Collection Services, Inc. v. Pope (1988);
            /// Burton v. Wilmington Parking Authority (1961);
            /// </remarks>
            public virtual Predicate<ILegalPerson> IsReliesOnGovernmentAssistance { get; set; } = lp => false;

            /// <summary>
            /// The government must have traditionally and exclusively performed the function.
            /// Furthermore, very few functions fall into this category
            /// </summary>
            /// <example>
            /// Does include functions like: running elections, operating a company town
            /// </example>
            /// <example>
            /// Does NOT include functions like: running sports associations, administering insurance payments,
            /// operation nursing home, providing special education, representing indigent criminal defendants,
            /// resolving private disputes, supplying electricity.
            /// </example>
            /// <remarks>
            /// Terry v. Adams (1953);
            /// Marsh v. Alabama (1946);
            /// cf. San Francisco Arts &amp; Athletics, Inc. v. United States Olympic Comm. (1987);
            /// </remarks>
            public virtual Predicate<IAct> IsTraditionalGovernmentFunction { get; set; } = lp => false;

            /// <summary>
            /// Considers the quality of the discrimination, for example:
            /// denial of land and home - very bad; denial of drinks and dinner - no big deal
            /// </summary>
            /// <remarks>
            /// Shelley v. Kraemer 334 U.S. 1 (1948),  - true
            /// Moose Lodge v. Irvis, 407 U.S. 163 (1972),  - false
            /// </remarks>
            public Predicate<IAct> IsInvidiousDiscrimination { get; set; } = a => false;

            public override bool IsValid(params ILegalPerson[] persons)
            {
                if (GetSubjectPerson == null)
                {
                    AddReasonEntry($"{nameof(GetSubjectPerson)} is null");
                    return false;
                }

                if (Action == null)
                {
                    AddReasonEntry($"{nameof(Action)} is null");
                    return false;
                }

                var subjectPerson = GetSubjectPerson(persons);

                if (subjectPerson == null)
                {
                    AddReasonEntry($"{nameof(GetSubjectPerson)} returned null");
                    return false;
                }

                var title = subjectPerson.GetLegalPersonTypeName();

                var reliesOnGovtBenefits = IsReliesOnGovernmentAssistance(subjectPerson);
                var traditionalGovtFx = IsTraditionalGovernmentFunction(Action);
                var isNasty = IsInvidiousDiscrimination(Action);

                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsReliesOnGovernmentAssistance)} returned {reliesOnGovtBenefits}");
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsTraditionalGovernmentFunction)} returned {traditionalGovtFx}");
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsInvidiousDiscrimination)} returned {isNasty}");

                if (reliesOnGovtBenefits || traditionalGovtFx || isNasty)
                {
                    return true;
                }

                return false;
            }

        }
    }


}
