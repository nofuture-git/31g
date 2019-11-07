using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Courts;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// The ability for a defendant to have the court moved to federal when
    /// the plaintiff chose state court
    /// </summary>
    /// <remarks>
    /// Idea is that both plaintiff and defendant should have the chance to opt for federal court
    /// </remarks>
    public class Removal : JurisdictionBase
    {
        public Removal(ICourt name, FederalJurisdictionBase fedJurisdiction) : base(name)
        {
            FederalJurisdiction = fedJurisdiction;
        }

        public FederalJurisdictionBase FederalJurisdiction { get; }

        public Predicate<ILegalPerson> IsRequestRemoval { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (FederalJurisdiction == null)
            {
                AddReasonEntry($"{nameof(FederalJurisdiction)} is unassigned");
                return false;
            }

            //test if this is even in scope for federal court based on ctor arg
            var isFederalIssue = FederalJurisdiction.IsValidWithTestCourtType(persons);
            AddReasonEntryRange(FederalJurisdiction.GetReasonEntries());
            if (!isFederalIssue)
            {
                return false;
            }

            if (IsFederalCourt())
            {
                AddReasonEntry($"{nameof(Court)}, '{Court?.Name}' is type " +
                               $"already type {nameof(FederalCourt)}");
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

            var defendantHome = GetDomicileLocation(defendant);
            if (defendantHome == null)
            {
                AddReasonEntry($"{defendantTitle} {defendant.Name}, " +
                               $"{nameof(GetDomicileLocation)} returned nothing");
                return false;
            }

            if (NamesEqual(Court, defendantHome))
            {
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(GetDomicileLocation)} returned '{defendantHome.Name}'");
                AddReasonEntry($"'{defendantHome.Name}' & {nameof(Court)} '{Court.Name}', {nameof(NamesEqual)} is true");
                return false;
            }

            return true;
        }
    }
}
