using System;
using System.Linq;
using NoFuture.Rand.Law.Property.US;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.IntentionalTort
{
    /// <summary>
    /// Similar to unjust enrichment except concerns time-sensitive information 
    /// </summary>
    public class Misappropriation : PropertyBase
    {
        public Misappropriation(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// (i) the plaintiff generates or collects information at some cost or expense
        ///  and
        /// (iii) the defendant&apos;s use of the information constitutes free-riding on
        /// the plaintiff&apos;s costly efforts to generate or collect it
        /// </summary>
        public Func<ILegalPerson, decimal> CalcInformationCost { get; set; } = lp => 0m;

        /// <summary>
        /// (ii) the value of the information is highly time-sensitive
        /// </summary>
        public Predicate<ILegalProperty> IsInformationTimeSensitive { get; set; } = p => false;

        /// <summary>
        /// (iv) the defendant&apos;s use of the information is in direct competition with
        /// a product or service offered by the plaintiff
        /// </summary>
        public Predicate<ILegalPerson> IsInformationUseDirectCompetition { get; set; } = lp => false;

        /// <summary>
        /// (v) the ability of other parties to free-ride on the efforts of the plaintiff would so
        /// reduce the incentive to produce the product or service that its existence or quality
        /// would be substantially threatened
        /// </summary>
        public Predicate<ILegalPerson> IsInformationIncentiveLost { get; set; } = lp => false;

        /// <summary>
        /// The amount of money at which one would consider gaining the information a &quot;free-ride&quot;
        /// </summary>
        public decimal FreeRidingMoneyValue { get; set; } = 1m;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;

            var title = subj.GetLegalPersonTypeName();
            if (PropertyOwnerIsSubjectPerson(persons))
                return false;

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;

            var pTitle = plaintiff.GetLegalPersonTypeName();
            var subjCost = CalcInformationCost(subj);
            var plaintiffCost = CalcInformationCost(plaintiff);
            if (plaintiffCost <= 0m)
            {
                AddReasonEntry($"{pTitle} {plaintiff.Name}, {nameof(CalcInformationCost)} cost is {plaintiffCost}");
                return false;
            }

            if (subjCost > FreeRidingMoneyValue)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(CalcInformationCost)} cost is {subjCost} which is " +
                               $"less than {nameof(FreeRidingMoneyValue)} value of {FreeRidingMoneyValue}");
                return false;
            }

            if (!IsInformationTimeSensitive(SubjectProperty))
            {
                AddReasonEntry($"{nameof(SubjectProperty)} named '{SubjectProperty.Name}', {nameof(IsInformationTimeSensitive)} is false");
                return false;
            }

            if (!IsInformationUseDirectCompetition(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsInformationUseDirectCompetition)} is false");
                return false;
            }

            if (persons.Any(p => IsInformationIncentiveLost(p)))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsInformationUseDirectCompetition)} is false");
                return false;
            }

            return true;
        }
    }
}
