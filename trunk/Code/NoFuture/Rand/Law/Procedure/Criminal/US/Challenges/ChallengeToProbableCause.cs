using System;
using NoFuture.Rand.Law.Procedure.Criminal.US.SearchReasons;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Challenges
{
    /// <summary>
    /// A way to override the probable cause
    /// </summary>
    /// <typeparam name="T">
    /// The affidavit type or form on which probable cause is based
    /// </typeparam>
    public class ChallengeToProbableCause<T> : LegalConcept, IRankable
    {
        /// <summary>
        /// so lacking in indicia of probable cause as to
        /// render belief in its existence entirely unreasonable
        /// States v. Leon, 468 U.S. 897 (1984)
        /// </summary>
        public Predicate<Affidavit<T>> IsInvalidOnItsFace { get; set; } = lc => false;

        public Affidavit<T> Affidavit { get; set; }

        public Predicate<Affidavit<T>> IsContainFalseStatement { get; set; } = lc => false;

        public Predicate<ILegalPerson> IsAffiantLawEnforcement { get; set; } = lc => lc is ILawEnforcement;

        /// <summary>
        /// either knowingly and intentionally or with reckless disregard for the truth
        /// </summary>
        public Predicate<ILegalPerson> IsDisregardOfTruth { get; set; } = lc => false;

        /// <summary>
        /// the false statement was necessary to the finding of probable cause
        /// </summary>
        public Predicate<Affidavit<T>> IsFalseStatementCritical { get; set; } = lc => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (Affidavit == null)
            {
                AddReasonEntry($"{nameof(Affidavit)} is unassigned");
                return false;
            }

            if (IsInvalidOnItsFace(Affidavit))
            {
                AddReasonEntry($"{nameof(IsInvalidOnItsFace)} is true");
                return false;
            }

            if (!IsContainFalseStatement(Affidavit))
            {
                AddReasonEntry($"{nameof(IsContainFalseStatement)} is false");
                return false;
            }

            var affiant = Affidavit.GetAffiant(persons);
            if (affiant == null)
            {
                AddReasonEntry($"{nameof(Affidavit)} {nameof(Affidavit.GetAffiant)} returned nothing");
                return false;
            }

            var affiantTitle = affiant.GetLegalPersonTypeName();

            if (!IsAffiantLawEnforcement(affiant))
            {
                AddReasonEntry($"{affiantTitle} {affiant.Name}, {nameof(IsAffiantLawEnforcement)} is false");
                return false;
            }

            if (!IsDisregardOfTruth(affiant))
            {
                AddReasonEntry($"{affiantTitle} {affiant.Name}, {nameof(IsDisregardOfTruth)} is false");
                return false;
            }

            if (!IsFalseStatementCritical(Affidavit))
            {
                AddReasonEntry($"{nameof(IsFalseStatementCritical)} is false");
                return false;
            }

            return true;
        }

        public virtual int GetRank()
        {
            return new ProbableCause().GetRank() + 1;
        }
    }
}
