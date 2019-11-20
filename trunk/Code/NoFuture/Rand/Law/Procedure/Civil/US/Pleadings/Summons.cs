using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// An official court document commanding the defendant to respond at some time and place
    /// </summary>
    public class Summons : PleadingBase
    {
        /// <summary>
        /// Federal Rules Civil Procedure Rule 4(a)(1)(D)
        /// </summary>
        public Func<ILegalPerson, DateTime?> GetDateOfAppearance { get; set; } = dt => null;

        public Predicate<ILegalPerson> IsIdentifiedParty { get; set; } = lp => lp is IPlaintiff || lp is IDefendant;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (IsSignedByCourtOfficial(persons))
                return false;

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;

            var plaintiffTitle = plaintiff.GetLegalPersonTypeName();

            if (!IsIdentifiedParty(plaintiff))
            {
                AddReasonEntry($"{plaintiffTitle} {plaintiff.Name}, {nameof(IsIdentifiedParty)} is false");
                return false;
            }

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var defendantTitle = defendant.GetLegalPersonTypeName();

            if (!IsIdentifiedParty(defendant))
            {
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(IsIdentifiedParty)} is false");
                return false;
            }

            if (GetDateOfAppearance(defendant) == null)
            {
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(GetDateOfAppearance)} is null");
                return false;
            }
            return true;
        }
    }
}
