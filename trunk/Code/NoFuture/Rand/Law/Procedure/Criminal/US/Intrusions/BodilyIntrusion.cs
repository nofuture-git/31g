using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Intrusions
{
    /// <summary>
    /// An intrusion into a suspect&apos;s body or its functions
    /// </summary>
    public class BodilyIntrusion : ImplicatingFourthAmendment<ILegalPerson>
    {
        /// <summary>
        /// When the conduct of law enforcement is simply outrageous.
        /// Rochin v. California, 342 U.S. 165 (1952) 
        /// </summary>
        public Predicate<ILegalPerson> IsShockingToConscience { get; set; } = lp => false;

        /// <summary>
        /// Notwithstanding the existence of probable cause, a search for evidence
        /// of a crime may be unjustifiable if it endangers the life or health of a suspect
        /// Winston v. Lee, 470 U.S. 753, 761 (1985)
        /// </summary>
        public Predicate<ILegalPerson> IsEndangerLifeHealthOfSuspect { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var suspect = this.Suspect(persons, GetSuspect);
            if (suspect == null)
                return false;

            var suspectTitle = suspect.GetLegalPersonTypeName();
            if (IsShockingToConscience(suspect))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(IsShockingToConscience)} is true");
                return false;
            }

            if (IsEndangerLifeHealthOfSuspect(suspect))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(IsEndangerLifeHealthOfSuspect)} is true");
                return false;
            }

            var lawEnforcement = this.LawEnforcement(persons, GetLawEnforcement);
            if (lawEnforcement == null)
                return false;
            var lawEnforcementTitle = lawEnforcement.GetLegalPersonTypeName();


            if (IsShockingToConscience(lawEnforcement))
            {
                AddReasonEntry($"{lawEnforcementTitle} {lawEnforcement.Name}, {nameof(IsShockingToConscience)} is true");
                return false;
            }

            return IsWarrantValid(persons) || IsProbableCauseExigentCircumstances(persons);
        }
    }

}
