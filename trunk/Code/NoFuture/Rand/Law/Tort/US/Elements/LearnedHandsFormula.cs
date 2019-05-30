using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    /// <summary>
    /// the Learned Hand test is an ex ante, reasonable person formula for evaluating by
    /// reference to precautions identified by the parties the social advisability of risky conduct
    /// </summary>
    [Aka("cost-benefit formula")]
    public class LearnedHandsFormula : Negligence
    {
        public LearnedHandsFormula(Func<IEnumerable<ILegalPerson>, ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public LearnedHandsFormula() : this(ExtensionMethods.Tortfeasor) { }

        /// <summary>
        /// a reasonable person&apos;s forecast estimation of the social cost of precautions
        /// </summary>
        /// <remarks>
        /// the party charging negligence will point out what
        /// this is by stating what should have been done to avoid the injury
        /// </remarks>
        public Func<ILegalPerson, decimal> GetCostOfPrecaution { get; set; } = lp => 0m;

        /// <summary>
        /// a reasonable person&apos;s forecast estimation of the risk of loss
        /// </summary>
        public Func<ILegalPerson, double> GetRiskOfLoss { get; set; } = lp => 0d;

        /// <summary>
        /// a reasonable person&apos;s forecast estimation of the social cost of a loss
        /// </summary>
        [Aka("injury cost")]
        public Func<ILegalPerson, decimal> GetCostOfLoss { get; set; } = lp => 0m;

        public virtual decimal Calculate(ILegalPerson subj)
        {
            if (subj == null)
                return 0m;

            var title = subj.GetLegalPersonTypeName();

            var burden = GetCostOfPrecaution(subj);
            AddReasonEntry($"{title} {subj.Name}, {nameof(GetCostOfPrecaution)} is {burden}");

            var probability = GetRiskOfLoss(subj);
            AddReasonEntry($"{title} {subj.Name}, {nameof(GetRiskOfLoss)} is {probability}");

            var injury = GetCostOfLoss(subj);
            AddReasonEntry($"{title} {subj.Name}, {nameof(GetCostOfLoss)} is {injury}");

            var result = (double) burden - (probability * (double) injury);

            return Convert.ToDecimal(result);
        }

        /// <summary>
        /// being true means there is negligence and therefore one is liable
        /// </summary>
        /// <param name="persons"></param>
        /// <returns></returns>
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;

            var title = subj.GetLegalPersonTypeName();

            var calc = Calculate(subj);
            AddReasonEntry($"{title} {subj.Name}, {nameof(Calculate)} is {calc}");
            return calc < 0;
        }
    }
}
