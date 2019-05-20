using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Theft
{
    /// <summary>
    /// Model Penal Code 223.3. Theft by Deception
    /// </summary>
    public class ByDeception : ConsolidatedTheft
    {
        /// <summary>
        /// Model Penal Code 223.3.(1)
        /// </summary>
        public Predicate<ILegalPerson> IsFalseImpression { get; set; } = lp => false;

        /// <summary>
        /// Model Penal Code 223.3.(2)
        /// </summary>
        public Predicate<ILegalPerson> IsPreventionOfTruth { get; set; } = lp => false;

        /// <summary>
        /// Model Penal Code 223.3.(3)
        /// </summary>
        public Predicate<ILegalPerson> IsFailureToCorrect { get; set; } = lp => false;

        /// <summary>
        /// Model Penal Code 223.3.(4)
        /// </summary>
        public Predicate<ILegalPerson> IsUndisclosedLegalImpediment { get; set; } = lp => false;

        /// <summary>
        /// When only possession has been gotten by deception, not legal entitlement
        /// </summary>
        public bool IsLarcenyByTrick { get; private set; }

        /// <summary>
        /// When both possession and legal entitlement have been gotten by deception
        /// </summary>
        public bool IsFalsePretense { get; private set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var defendant = persons.Defendant();
            if (defendant == null)
                return false;
            var ifi = IsFalseImpression(defendant);
            var ipot = IsPreventionOfTruth(defendant);
            var iftc = IsFailureToCorrect(defendant);
            var iuli = IsUndisclosedLegalImpediment(defendant);

            if (new[] {ifi, ipot, iftc, iuli}.All(p => p == false))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsFalseImpression)} is {ifi}");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsPreventionOfTruth)} is {ipot}");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsFailureToCorrect)} is {iftc}");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsUndisclosedLegalImpediment)} is {iuli}");
                return false;
            }

            var isPossess = VocaBase.Equals(SubjectProperty.InPossessionOf,defendant);
            var isTitle = VocaBase.Equals(SubjectProperty.EntitledTo,defendant);

            IsLarcenyByTrick = isPossess && !isTitle;
            IsFalsePretense = isPossess && isTitle;

            if(IsLarcenyByTrick)
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsLarcenyByTrick)} is true");
            if(IsFalsePretense)
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsFalsePretense)} is true");
            return true;
        }
    }
}
