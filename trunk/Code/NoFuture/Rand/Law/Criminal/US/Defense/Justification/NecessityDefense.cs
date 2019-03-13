using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    /// <summary>
    /// protects defendant from criminal responsibility when the defendant commits a crime to avoid a greater, imminent harm
    /// </summary>
    [Aka("choice of evils defense")]
    public class NecessityDefense : DefenseBase
    {
        public NecessityDefense(ICrime crime) : base(crime)
        {
            var getDefendant = crime == null ? LegalPerson.GetNullPerson : crime.GetDefendant;
            Proportionality = new ChoiceThereof<ITermCategory>(getDefendant);
            Imminence = new Imminence(getDefendant);
        }

        /// <summary>
        /// (1) there must be more than one harm that will occur under the circumstances
        /// </summary>
        public Predicate<ILegalPerson> IsMultipleInHarm { get; set; } = lp => false;

        /// <summary>
        /// (2) the harms must be ranked, with one of the harms ranked more severe than the other
        /// </summary>
        public ChoiceThereof<ITermCategory> Proportionality { get; set; }

        /// <summary>
        /// (3) the defendant must have objectively reasonable belief that the greater harm is imminent
        /// </summary>
        public Imminence Imminence { get; set; }

        /// <summary>
        /// (4) [optional] the defendant did not intentionally or recklessly place himself in a
        ///     situation in which it would be probable that he would be forced to
        ///     choose the criminal conduct
        /// </summary>
        public Predicate<ILegalPerson> IsResponsibleForSituationArise { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = Crime.GetDefendant(persons);
            if (defendant == null)
                return false;
            if (IsResponsibleForSituationArise != null && IsResponsibleForSituationArise(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsResponsibleForSituationArise)} is true");
                return false;
            }

            if (!IsMultipleInHarm(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsMultipleInHarm)} is false");
                return false;
            }

            if (Imminence != null && !Imminence.IsValid(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Imminence)} is false");
                AddReasonEntryRange(Imminence.GetReasonEntries());
                return false;
            }
            if (Proportionality != null && !Proportionality.IsValid(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Proportionality)} is false");
                AddReasonEntryRange(Proportionality.GetReasonEntries());
                return false;
            }


            AddReasonEntryRange(Imminence?.GetReasonEntries());
            AddReasonEntryRange(Proportionality?.GetReasonEntries());

            return true;
        }
    }
}
