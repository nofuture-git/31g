using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// One of the categories that may be held in federal courts is between citizens of different states
    /// </summary>
    public class FederalDiversityJurisdiction : JurisdictionBase, IFederalJurisdiction
    {
        public FederalDiversityJurisdiction(ICourt name) : base(name)
        {
        }

        /// <summary>
        /// St. Paul Mercury Indemnity Co. v. Red Cab Co., 303 U.S. 283 (1938)
        /// - will take the plaintiff&apos;s claim on good-faith unless its obviously wrong
        /// </summary>
        public Func<ILegalPerson, decimal> GetInjuryClaimDollars { get; set; } = lp => 0m;

        /// <summary>
        /// A dollar amount set by Congress which is the minimum - currently its $75,000
        /// </summary>
        public Func<DateTime, decimal> GetMinimumClaimDollars { get; set; } = dt => 75000m;

        /// <summary>
        /// Allows caller to specify some past or future date, default is current system UTC time
        /// </summary>
        public DateTime CurrentTime { get; set; } = DateTime.UtcNow;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (!IsFederalCourt())
                return false;

            return IsValidAsFederalCourt(persons);

        }

        public virtual bool IsValidAsFederalCourt(ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons) as ILegalPerson;

            if (defendant == null)
                return false;

            var defendantTitle = defendant.GetLegalPersonTypeName();

            var plaintiff = this.Plaintiff(persons) as ILegalPerson;
            if (plaintiff == null)
                return false;

            var plaintiffTitle = plaintiff.GetLegalPersonTypeName();

            if (IsEitherPartyForeigner(defendant, plaintiff, defendantTitle, plaintiffTitle))
            {
                return true;
            }

            if (!IsDiversityInDomicileLocations(defendant, plaintiff, defendantTitle, plaintiffTitle))
            {
                return false;
            }

            if (!IsMinimumClaimSufficient(plaintiff, plaintiffTitle))
            {
                return false;
            }

            return true;
        }

        protected internal virtual bool IsEitherPartyForeigner(ILegalPerson defendant, ILegalPerson plaintiff,
            string defendantTitle = null, string plaintiffTitle = null)
        {
            defendantTitle = defendantTitle ?? defendant.GetLegalPersonTypeName();
            plaintiffTitle = plaintiffTitle ?? plaintiff.GetLegalPersonTypeName();

            if (defendant is IForeigner)
            {
                AddReasonEntry($"{defendantTitle} {defendant.Name}, is type {nameof(IForeigner)}");
                return true;
            }

            if (plaintiff is IForeigner)
            {
                AddReasonEntry($"{plaintiffTitle} {plaintiff.Name}, is type {nameof(IForeigner)}");
                return true;
            }

            return false;
        }

        protected internal virtual bool IsDiversityInDomicileLocations(ILegalPerson defendant, ILegalPerson plaintiff,
            string defendantTitle = null, string plaintiffTitle = null)
        {
            defendantTitle = defendantTitle ?? defendant.GetLegalPersonTypeName();
            plaintiffTitle = plaintiffTitle ?? plaintiff.GetLegalPersonTypeName();

            var plaintiffHome = GetDomicileLocation(plaintiff);
            if (plaintiffHome == null)
            {
                AddReasonEntry($"{plaintiffTitle} {plaintiff.Name}, " +
                               $"{nameof(GetDomicileLocation)} returned nothing");
                return false;
            }

            var defendantHome = GetDomicileLocation(defendant);
            if (defendantHome == null)
            {
                AddReasonEntry($"{defendantTitle} {defendant.Name}, " +
                               $"{nameof(GetDomicileLocation)} returned nothing");
                return false;
            }

            if (NamesEqual(plaintiffHome, defendantHome))
            {
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(GetDomicileLocation)} returned '{defendantHome.Name}'");
                AddReasonEntry($"{plaintiffTitle} {plaintiff.Name}, {nameof(GetDomicileLocation)} returned '{plaintiffHome.Name}'");
                AddReasonEntry($"'{defendantHome.Name}' & '{plaintiffHome.Name}', {nameof(NamesEqual)} is true");
                return false;
            }

            return true;
        }

        protected internal virtual bool IsMinimumClaimSufficient(ILegalPerson plaintiff, string plaintiffTitle = null)
        {
            plaintiffTitle = plaintiffTitle ?? plaintiff.GetLegalPersonTypeName();

            var plaintiffClaim = GetInjuryClaimDollars(plaintiff);

            var currentMin = GetMinimumClaimDollars(CurrentTime);

            if (plaintiffClaim < currentMin)
            {
                AddReasonEntry($"{plaintiffTitle} {plaintiff.Name}, {nameof(GetInjuryClaimDollars)} returned ${plaintiffClaim}");
                AddReasonEntry($"{nameof(GetMinimumClaimDollars)} at {CurrentTime} returned ${currentMin}");
                AddReasonEntry($"${plaintiffClaim} is less than ${currentMin}");
                return false;
            }

            return true;
        }
    }
}
