using System;
using NoFuture.Law.Attributes;

namespace NoFuture.Law.Procedure.Criminal.US.SearchReasons
{
    /// <summary>
    /// For immediate search\seizure in emergency situation
    /// </summary>
    /// <remarks>
    /// immediate to prevent physical harm, destruction of evidence, escape, or some fourth thing
    /// </remarks>
    [Aka("the emergency exception", "hot pursuit")]
    public class ExigentCircumstances : ProbableCause
    {
        public Predicate<ILegalPerson> IsCompellingUrgency { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var officerTuple = GetLawEnforcementAndTitle(persons);

            if (officerTuple?.Item1 == null)
            {
                return false;
            }

            var officer = officerTuple.Item1;
            var officerTitle = officerTuple.Item2;

            if (!IsCompellingUrgency(officer))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsCompellingUrgency)} is false");
                return false;
            }

            return base.IsValid(persons);
        }
    }
}
