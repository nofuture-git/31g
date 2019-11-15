using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// A base type for cross, counter and contribution claims -
    /// which are all related in that they are reactive to a original complaint
    /// </summary>
    /// <remarks>
    /// *neologism
    /// </remarks>
    public abstract class Replaint : Complaint
    {
        protected internal virtual bool TestFuncEnclosure(Func<ILegalConcept, ILegalConcept, bool> test, string testName,
            ILegalPerson[] persons)
        {
            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var plaintiffTitle = plaintiff.GetLegalPersonTypeName();
            var defendantTitle = defendant.GetLegalPersonTypeName();

            if (!TryGetCauseOfAction(plaintiff, out var causesOfAction))
                return false;

            if (!TryGetCauseOfAction(defendant, out var oppositionCausesOfAction))
                return false;

            if (!test(oppositionCausesOfAction, causesOfAction))
            {
                AddReasonEntry($"{nameof(GetCausesOfAction)} for {plaintiffTitle} {plaintiff.Name} and " +
                               $"{defendantTitle} {defendant.Name}, {testName} is false");
                return false;
            }

            return true;
        }
    }
}
