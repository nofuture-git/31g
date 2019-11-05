using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    public class Summons : PleadingBase
    {
        /// <summary>
        /// Federal Rules Civil Procedure Rule 4(D)
        /// </summary>
        public Func<ILegalPerson, DateTime?> GetDateOfAppearance { get; set; } = dt => null;

        public LegalConcept ServingProcess { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (IsSignedByCourtOfficial(persons))
                return false;

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var title = defendant.GetLegalPersonTypeName();

            if (GetDateOfAppearance(defendant) == null)
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(GetDateOfAppearance)} is null");
                return false;
            }

            if (ServingProcess == null)
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(ServingProcess)} is unassigned");
                return false;
            }

            var result = ServingProcess.IsValid(persons);

            AddReasonEntryRange(ServingProcess.GetReasonEntries());

            return result;
        }
    }
}
