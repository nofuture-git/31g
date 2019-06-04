using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Tort.US.Terms;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    /// <summary>
    /// liabilities in tort arise from failure to comply with fixed and
    /// uniform standards of external conduct, which every man is presumed
    /// and required to know
    /// [OLIVER WENDELL HOLMES, JR., THE COMMON LAW 111, 123-4 (1881)]
    /// </summary>
    public class Negligence : UnoHomine
    {
        public Negligence(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// defendant&apos;s conduct was morally wrong according to prevailing community norms
        /// </summary>
        public Predicate<ILegalPerson> IsConductMorallyWrong { get; set; } = lp => false;

        /// <summary>
        /// Where the plaintiff assumed the risk therefore the defendant has no duty otherwise
        /// </summary>
        [EtymologyNote("Latin", "volenti non fit injuria", "willing No injury is")]
        [Aka("primary assumption of risk")]
        public Predicate<ILegalPerson> IsNoDuty { get; set; } = lp => false;

        /// <summary>
        /// whether the defendant breached a safety convention commonly understood
        /// in the community to protect the kinds of people like the plaintiff
        /// </summary>
        public CustomsTerm SafetyConvention { get; set; }

        /// <summary>
        /// The connection of a person to the cause in both fact and law
        /// </summary>
        public Causation Causation { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            if (IsConductMorallyWrong(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsConductMorallyWrong)} is true");
                return true;
            }

            if (IsNoDuty(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsNoDuty)} is true");
                return false;
            }

            if (Causation != null)
            {
                if (!Causation.IsForeseeable(subj))
                {
                    AddReasonEntry($"{title} {subj.Name}, {nameof(Causation)} " +
                                   $"{nameof(Causation.IsForeseeable)} is false");
                    return false;
                }

                //it must not be due to any voluntary action on the part of plaintiff.
                var plaintiff = persons.Plaintiff() as IPlaintiff;
                if (plaintiff == null)
                {
                    var nameTitles = persons.Select(p => Tuple.Create(p.GetLegalPersonTypeName(), p.Name));
                    AddReasonEntry($"No one is the {nameof(IPlaintiff)} in {nameTitles}");
                    return false;
                }

                if (Causation.IsButForCaused(plaintiff))
                {
                    AddReasonEntry($"{plaintiff.GetLegalPersonTypeName()} {plaintiff.Name}, " +
                                   $"{nameof(Causation)} {nameof(Causation.IsButForCaused)} is true");
                    return false;
                }

                return true;
            }

            if (SafetyConvention != null && SafetyConvention.IsValid(persons) == false)
            {
                AddReasonEntryRange(SafetyConvention.GetReasonEntries());
                AddReasonEntry($"{title} {subj.Name}, {nameof(SafetyConvention)} {nameof(IsValid)} is false");
                return true;
            }

            return false;
        }
    }
}
