using System;
using NoFuture.Law.US;
using NoFuture.Law.US.Courts;

namespace NoFuture.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// The ability for a defendant to have the court moved to federal when
    /// the plaintiff chose state court 28 U.S.C. §1441(a).
    /// </summary>
    /// <remarks>
    /// Idea is that both plaintiff and defendant should have the chance to opt for federal court
    /// </remarks>
    public class Removal : JurisdictionBase
    {
        public Removal(IFederalJurisdiction fedJurisdiction) : base(null)
        {
            FederalJurisdiction = fedJurisdiction;
        }

        public IFederalJurisdiction FederalJurisdiction { get; }

        public Predicate<ILegalPerson> IsRequestRemoval { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (FederalJurisdiction == null)
            {
                AddReasonEntry($"{nameof(FederalJurisdiction)} is unassigned");
                return false;
            }

            if (Court is FederalCourt)
            {
                AddReasonEntry($"{nameof(Court)}, '{Court?.Name}' is type " +
                               $"already type {nameof(FederalCourt)}");
                return false;
            }

            var jurisdictionBase = FederalJurisdiction as JurisdictionBase;
            jurisdictionBase?.CopyTo(this);

            //test if this is even in scope for federal court based on ctor arg
            var isFederalIssue = FederalJurisdiction.IsValidAsFederalCourt(persons);
            AddReasonEntryRange(FederalJurisdiction.GetReasonEntries());
            if (!isFederalIssue)
            {
                return false;
            }

            var defendant = this.Defendant(persons);

            if (defendant == null)
                return false;

            var defendantTitle = defendant.GetLegalPersonTypeName();

            if (!IsRequestRemoval(defendant))
            {
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(IsRequestRemoval)} is false");
                return false;
            }

            if (IsCourtDomicileLocationOfDefendant(defendant, defendantTitle))
            {
                return false;
            }

            return true;
        }
    }
}
