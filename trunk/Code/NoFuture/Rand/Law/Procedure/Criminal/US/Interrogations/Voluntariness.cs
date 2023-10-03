using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Procedure.Criminal.US.Interrogations
{
    /// <summary>
    /// a statement must be obtained as a product of voluntary choice
    /// </summary>
    public class Voluntariness : LegalConcept
    {
        public Func<ILegalPerson[], ILegalPerson> GetSuspect { get; set; } = lps => lps.Suspect();

        /// <summary>
        /// if the police subject the suspect to coercive conduct
        /// </summary>
        public Predicate<ILegalPerson> IsSubjectedToCoercivePoliceConduct { get; set; } = lp => false;

        /// <summary>
        /// conduct was sufficient to overcome the will of the suspect with
        /// respect for their particular circumstances 
        /// </summary>
        public Predicate<ILegalPerson> IsSufficientToOvercomeWillpower { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var suspect = this.Suspect(persons, GetSuspect);
            if (suspect == null)
                return false;

            var suspectTitle = suspect.GetLegalPersonTypeName();

            if (IsSubjectedToCoercivePoliceConduct(suspect)
                && IsSufficientToOvercomeWillpower(suspect))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(IsSubjectedToCoercivePoliceConduct)} is true");
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(IsSufficientToOvercomeWillpower)} is true");
                return false;
            }

            return true;
        }
    }
}
