using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// A response to a <see cref="Summons"/> in which defendant
    /// objects to court&apos;s personal jurisdiction over them 
    /// </summary>
    public class SpecialAppearance : PleadingBase
    {
        public ILegalConcept Jurisdiction { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var title = defendant.GetLegalPersonTypeName();

            if (Jurisdiction == null)
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(Jurisdiction)} is unassigned");
                return false;
            }

            var result = Jurisdiction.IsValid(persons);

            AddReasonEntryRange(Jurisdiction.GetReasonEntries());

            return result;
        }
    }
}
