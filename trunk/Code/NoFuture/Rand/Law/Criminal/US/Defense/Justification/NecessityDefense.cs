using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    /// <inheritdoc cref="INecessity"/>
    public class NecessityDefense : DefenseBase, INecessity
    {
        public NecessityDefense(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
            Proportionality = new ChoiceThereof<ITermCategory>(getSubjectPerson);
            Imminence = new Imminence(getSubjectPerson);
        }

        public NecessityDefense() : base(ExtensionMethods.Defendant)
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
            var legalPerson = persons.Defendant();
            if (legalPerson == null)
                return false;
            var lpPersonType = legalPerson.GetLegalPersonTypeName();
            if (IsResponsibleForSituationArise != null && IsResponsibleForSituationArise(legalPerson))
            {
                AddReasonEntry($"{lpPersonType}, {legalPerson.Name}, {nameof(IsResponsibleForSituationArise)} is true");
                return false;
            }

            if (!IsMultipleInHarm(legalPerson))
            {
                AddReasonEntry($"{lpPersonType}, {legalPerson.Name}, {nameof(IsMultipleInHarm)} is false");
                return false;
            }

            if (Imminence != null && !Imminence.IsValid(legalPerson))
            {
                AddReasonEntry($"{lpPersonType}, {legalPerson.Name}, {nameof(Imminence)} is false");
                AddReasonEntryRange(Imminence.GetReasonEntries());
                return false;
            }
            if (Proportionality != null && !Proportionality.IsValid(legalPerson))
            {
                AddReasonEntry($"{lpPersonType}, {legalPerson.Name}, {nameof(Proportionality)} is false");
                AddReasonEntryRange(Proportionality.GetReasonEntries());
                return false;
            }

            AddReasonEntryRange(Imminence?.GetReasonEntries());
            AddReasonEntryRange(Proportionality?.GetReasonEntries());

            return true;
        }
    }
}
