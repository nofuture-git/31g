using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.Homicide.US.Elements
{
    /// <summary>
    /// A death or deaths that result from the commission of some other crime
    /// </summary>
    [Aka("felony murder", "misdemeanor manslaughter")]
    public class HomicideInOther : Murder, IHomicideConcurrance
    {
        public HomicideInOther(ICrime crime)
        {
            SourceCrime = crime;
        }

        public DateTime Inception { get; set; }

        /// <summary>
        /// in criminal law, the crime ends when the defendant reaches a place of temporary safety
        /// </summary>
        public DateTime? Terminus { get; set; }

        public ICrime SourceCrime { get; }

        /// <summary>
        /// The time at which the person died as a result of the commission of the given <see cref="SourceCrime"/>
        /// </summary>
        public DateTime? TimeOfTheDeath { get; set; }

        public bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception <= dt;
            var beforeOrOnToDt = Terminus == null || Terminus.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (SourceCrime == null)
            {
                AddReasonEntry($"{nameof(HomicideInOther)} implies a death which " +
                               "occurred during the commission of another crime");
                return false;
            }

            var defendant = GetDefendant(persons) ?? SourceCrime.GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!SourceCrime.IsValid(persons))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(SourceCrime)} is invalid");
                AddReasonEntryRange(SourceCrime.GetReasonEntries());
                return false;
            }

            if (!IsHomicideConcurrance(this, this, defendant.Name))
                return false;

            return base.IsValid(persons);
        }

        public override bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            if (criminalIntent is StrictLiability)
            {
                AddReasonEntry($"{nameof(HomicideInOther)} is not applicable to {nameof(StrictLiability)} intent");
                return false;
            }

            return true;
        }
    }
}
