using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    /// <summary>
    /// Critics objections to the utilitarian philosophy of the formula.
    /// </summary>
    public class CritiqueLearnedHandsFormula : LearnedHandsFormula
    {
        public CritiqueLearnedHandsFormula(Func<IEnumerable<ILegalPerson>, ILegalPerson> getSubjectPerson) : base(
            getSubjectPerson)
        {

        }

        public CritiqueLearnedHandsFormula() : this(ExtensionMethods.Tortfeasor) { }

        /// <summary>
        /// Idea that the cost-benefit analysis may lead to obvious morally wrong decisions.
        /// </summary>
        /// <example>
        /// Idea that five lives can be saved for one life but the sacrifice of the one is murder
        /// [Judith Jarvis Thomson, The Trolley Problem, 94 YALE L.J. 1395, 1409 (1985).]
        /// </example>
        public Predicate<ILegalPerson> IsMorallyWrong { get; set; } = lp => false;

        /// <summary>
        /// Idea that actually getting the cost-benefit data is itself impossibly difficult
        /// </summary>
        public Func<ILegalPerson, decimal> GetCostOfMethodology { get; set; } = lp => 0m;

        /// <summary>
        /// Idea that the precaution would impose a duty which is mutually exclusive of and existing one
        /// </summary>
        public Predicate<ILegalPerson> IsNegateExistingDuty { get; set; } = lp => false;

        public override decimal Calculate(ILegalPerson subj)
        {
            if (subj == null)
                return 0m;

            var title = subj.GetLegalPersonTypeName();

            if (IsMorallyWrong(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsMorallyWrong)} is true");
                return -1m;
            }

            if (IsNegateExistingDuty(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsNegateExistingDuty)} is true");
                return 0m;
            }

            var lhfCalc = base.Calculate(subj);

            var methodCost = GetCostOfMethodology(subj);
            AddReasonEntry($"{title} {subj.Name}, {nameof(GetCostOfMethodology)} is {methodCost}");

            var result = lhfCalc - methodCost;

            return result;
        }
    }
}
