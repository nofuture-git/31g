using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft
{
    /// <summary>
    /// Model Penal Code 223.3. Theft by Decption
    /// </summary>
    [Aka("false pretense")]
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
        public Predicate<ILegalPerson> IsUndiscloseLegalImpediment { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;
            var ifi = IsFalseImpression(defendant);
            var ipot = IsPreventionOfTruth(defendant);
            var iftc = IsFailureToCorrect(defendant);
            var iuli = IsUndiscloseLegalImpediment(defendant);

            if (new[] {ifi, ipot, iftc, iuli}.All(p => p == false))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsFalseImpression)} is {ifi}");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsPreventionOfTruth)} is {ipot}");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsFailureToCorrect)} is {iftc}");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsUndiscloseLegalImpediment)} is {iuli}");
                return false;
            }

            return true;
        }
    }
}
