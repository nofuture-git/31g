using System;
using System.Linq;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.Act
{
    /// <summary>
    /// The substantial steps toward the completion of a crime
    /// </summary>
    public class SubstantialSteps : LegalConcept, IActusReus
    {
        public Predicate<ILegalPerson> IsLyingInWait { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsEnticingVictimToScene { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsInvestigatingPotentialScene { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsUnlawfulEntry { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsPossessCriminalTools { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsPossessCriminalMaterial { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            var lyingInWait = IsLyingInWait(defendant);
            if(lyingInWait)
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsLyingInWait)} is true");
            var entice = IsEnticingVictimToScene(defendant);
            if (entice)
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsEnticingVictimToScene)} is true");
            var investigate = IsInvestigatingPotentialScene(defendant);
            if (investigate)
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsInvestigatingPotentialScene)} is true");
            var isEntry = IsUnlawfulEntry(defendant);
            if (isEntry)
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsUnlawfulEntry)} is true");
            var isTools = IsPossessCriminalTools(defendant);
            if (isTools)
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsPossessCriminalTools)} is true");
            var isMatl = IsPossessCriminalMaterial(defendant);
            if (isMatl)
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsPossessCriminalMaterial)} is true");

            return new[] {lyingInWait, entice, investigate, isEntry, isTools, isMatl}.Any(p => p);
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
