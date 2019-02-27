using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.US;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft
{
    /// <summary>
    /// Model Penal Code 223.4 Theft by Extortion
    /// </summary>
    public class ByExtortion : ConsolidatedTheft
    {
        /// <summary>
        /// Model Penal Code 223.4.(1)-(7)
        /// </summary>
        public class ByThreatening : CriminalBase
        {
            public Predicate<ILegalPerson> IsToInjury { get; set; } = lp => false;

            public Predicate<ILegalPerson> IsToAnyOtherCrime { get; set; } = lp => false;

            public Predicate<ILegalPerson> IsToAccuseOfCrime { get; set; } = lp => false;

            /// <summary>
            /// A secret that would subject a person to hatred, contempt, ridicule or impair repute
            /// </summary>
            public Predicate<ILegalPerson> IsToExposeHurtfulSecret { get; set; } = lp => false;

            /// <summary>
            /// An person appointed or elected to an office 
            /// </summary>
            public Predicate<ILegalPerson> IsByActionOfOffical { get; set; } = lp => false;

            public Predicate<ILegalPerson> IsByCollectiveAction { get; set; } = lp => false;

            /// <summary>
            /// A testimony to a legal claim
            /// </summary>
            public Predicate<ILegalPerson> IsToLegalTestimony { get; set; } = lp => false;

            public override bool IsValid(params ILegalPerson[] persons)
            {
                var defendant = GetDefendant(persons);
                if (defendant == null)
                    return false;

                var isToInjury = IsToInjury(defendant);
                var isAnyCrime = IsToAnyOtherCrime(defendant);
                var isAccuseCrime = IsToAccuseOfCrime(defendant);
                var isExpose = IsToExposeHurtfulSecret(defendant);
                var isOffical = IsByActionOfOffical(defendant);
                var isStrike = IsByCollectiveAction(defendant);
                var isTestimony = IsToLegalTestimony(defendant);

                if (new[] {isToInjury, isAnyCrime, isAccuseCrime, isExpose, isOffical, isStrike, isTestimony}.All(p =>
                    p == false))
                {
                    AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsToInjury)} is {isToInjury}");
                    AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsToAnyOtherCrime)} is {isAnyCrime}");
                    AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsToAccuseOfCrime)} is {isAccuseCrime}");
                    AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsToExposeHurtfulSecret)} is {isExpose}");
                    AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByActionOfOffical)} is {isOffical}");
                    AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByCollectiveAction)} is {isStrike}");
                    AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsToLegalTestimony)} is {isTestimony}");
                    return false;
                }

                return true;
            }
        }

        public Predicate<ILegalPerson> IsPurposelyObtained { get; set; } = lp => false;

        public ByThreatening Threatening { get; set; } = new ByThreatening();

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var threatening = Threatening ?? new ByThreatening();

            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsPurposelyObtained(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsPurposelyObtained)} is false");
                return false;
            }

            if (!threatening.IsValid(persons))
            {
                AddReasonEntryRange(threatening.GetReasonEntries());
                return false;
            }

            return true;
        }
    }
}
