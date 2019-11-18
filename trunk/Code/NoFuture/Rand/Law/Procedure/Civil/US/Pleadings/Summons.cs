using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    public class Summons : PleadingBase
    {
        /// <summary>
        /// Federal Rules Civil Procedure Rule 4(a)(1)(D)
        /// </summary>
        public Func<ILegalPerson, DateTime?> GetDateOfAppearance { get; set; } = dt => null;

        /// <summary>
        ///  Federal Rules Civil Procedure Rule 4(c)
        /// </summary>
        public Func<ILegalPerson, LegalConcept> GetServingProcess { get; set; } = lp => null;

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

            var servingProcess = GetServingProcess(defendant);
            if (servingProcess == null)
            {
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(GetServingProcess)} returned nothing");
                return false;
            }

            var result = servingProcess.IsValid(persons);

            AddReasonEntryRange(servingProcess.GetReasonEntries());

            return result;
        }
    }
}
