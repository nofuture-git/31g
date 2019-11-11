using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// Defined in 28 U.S.C. section 1391
    /// </summary>
    /// <remarks>
    /// In addition, other laws may set venue based on subject matter
    /// (e.g. patents, copyrights) or may be agreed in advance as
    /// a &quot;forum selection clauses&quot;
    /// </remarks>
    /// <remarks>
    /// Note, section 1391 does not apply to <see cref="Removal"/>
    /// </remarks>
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
