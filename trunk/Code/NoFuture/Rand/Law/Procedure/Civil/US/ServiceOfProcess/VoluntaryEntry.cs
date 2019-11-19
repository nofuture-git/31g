using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Civil.US.ServiceOfProcess
{
    /// <summary>
    /// When the defendant acknowledges receipt of court summons
    /// </summary>
    public class VoluntaryEntry : CivilProcedureBase
    {
        public bool IsNotaryPublicSignatureRequired { get; set; } = true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var defendantTitle = defendant.GetLegalPersonTypeName();

            if (!IsSigned(defendant))
            {
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(IsSigned)} is false.");
                return false;
            }

            if (!IsNotaryPublicSignatureRequired)
                return true;

            var notary = persons.NotaryPublic();
            if (notary == null)
            {
                var nameTitles = persons.GetTitleNamePairs();
                AddReasonEntry($"No one is the {nameof(INotaryPublic)} in {nameTitles}");
                return false;
            }

            if (!IsSigned(notary))
            {
                AddReasonEntry($"{notary.GetLegalPersonTypeName()} {notary.Name}, {nameof(IsSigned)} is false");
                return false;
            }

            return true;
        }
    }
}
