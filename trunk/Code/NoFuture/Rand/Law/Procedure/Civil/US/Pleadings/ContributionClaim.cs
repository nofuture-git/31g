using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// Where a defendant adds a third-party as in,
    /// &quot;I am only liable because this person&quot;.
    /// That third-party may then do the same concerning
    /// some forth-party, etc.
    /// </summary>
    /// <remarks>
    /// Fed Civil Procedure Rule 14:
    /// (a)(1) allows defendant to implead third-party
    /// (a)(2)(D) allows third-party to make counter-claim
    ///           against original plaintiff
    /// (a)(5) third-party to forth-party
    /// </remarks>
    [Aka("impleaded")]
    public class ContributionClaim : Complaint
    {
        public Predicate<ILegalPerson> IsShareInLiability { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var thirdParty = this.ThirdParty(persons);
            if (thirdParty == null)
                return false;

            var isShareLiability = IsShareInLiability(thirdParty);
            if (!isShareLiability)
            {
                AddReasonEntry($"{thirdParty.GetLegalPersonTypeName()} {thirdParty.Name}, " +
                               $"{nameof(IsShareInLiability)} is false");
                return false;
            }

            return base.IsValid(persons);
        }
    }
}
