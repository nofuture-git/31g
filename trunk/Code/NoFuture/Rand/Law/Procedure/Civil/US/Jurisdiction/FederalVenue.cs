using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// Defined in 28 U.S.C. section 1391
    /// </summary>
    public class FederalVenue : PersonalJurisdiction, IVenue
    {
        public FederalVenue(ICourt name) : base(name)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (!IsFederalCourt())
                return false;

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;

            if (IsCourtDomicileLocationOfDefendant(defendant))
                return true;

            if (IsCourtInjuryLocationOfPlaintiff(plaintiff))
                return true;

            return base.IsValid(persons);
        }
    }
}
