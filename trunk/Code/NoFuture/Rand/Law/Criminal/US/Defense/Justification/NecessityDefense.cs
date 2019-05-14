using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    /// <inheritdoc cref="INecessity"/>
    public class NecessityDefense : DefenseBase, INecessity
    {
        public NecessityDefense()
        {
            Proportionality = new ChoiceThereof<ITermCategory>(ExtensionMethods.Defendant);
            Imminence = new Imminence(ExtensionMethods.Defendant);
        }

        public Predicate<ILegalPerson> IsMultipleInHarm { get; set; } = lp => false;

        public ChoiceThereof<ITermCategory> Proportionality { get; set; }

        public Imminence Imminence { get; set; }

        public Predicate<ILegalPerson> IsResponsibleForSituationArise { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
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
