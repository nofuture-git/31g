using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.Criminal.US;
using NoFuture.Law.Criminal.US.Elements.Intent;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.Homicide
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

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!SourceCrime.IsValid(persons))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(SourceCrime)} is invalid");
                AddReasonEntryRange(SourceCrime.GetReasonEntries());
                return false;
            }

            if (!IsHomicideConcurrance(this, this, defendant.Name, title))
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
