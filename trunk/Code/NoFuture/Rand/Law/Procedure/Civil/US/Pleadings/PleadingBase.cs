using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    public abstract class PleadingBase : CivilProcedureBase
    {
        public ICourt NameOfCourt { get; set; }

        public Predicate<ILegalPerson> IsSigned { get; set; } = lp => false;

        protected bool IsCourtAssigned()
        {
            if (NameOfCourt != null && !string.IsNullOrWhiteSpace(NameOfCourt.Name)) 
                return true;

            AddReasonEntry($"{nameof(NameOfCourt)} is unassigned or an empty string");
            return false;
        }

        protected bool IsSignedByCourtOfficial(ILegalPerson[] persons)
        {
            var courtOfficial = persons.CourtOfficial() as ICourtOfficial;
            if (courtOfficial == null)
            {
                var nameTitles = persons.GetTitleNamePairs();
                AddReasonEntry($"No one is the {nameof(ICourtOfficial)} in {nameTitles}");
                return false;
            }

            if (!IsSigned(courtOfficial))
            {
                AddReasonEntry($"{courtOfficial.GetLegalPersonTypeName()} {courtOfficial.Name}, {nameof(IsSigned)} is false");
                return false;
            }

            return true;
        }
    }
}
