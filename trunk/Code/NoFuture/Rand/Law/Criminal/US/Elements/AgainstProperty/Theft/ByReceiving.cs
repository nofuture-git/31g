using System;
using System.Linq;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstProperty.Theft
{
    /// <summary>
    /// Model Penal Code 223.6.
    /// </summary>
    [Aka("receiving stolen property")]
    public class ByReceiving : ConsolidatedTheft
    {
        public Predicate<ILegalPerson> IsPresentStolen { get; set; } = lp => false;

        /// <summary>
        /// The person receiving it believes its probably stolen
        /// </summary>
        public Predicate<ILegalPerson> IsApparentStolen { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            var isStolen = IsPresentStolen(defendant);
            var isBelievedStolen = IsApparentStolen(defendant);

            if (!isStolen && !isBelievedStolen)
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsPresentStolen)} is false");
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsApparentStolen)} is false");
                return false;
            }

            if (!InPossessionOfDefendant(persons))
                return false;

            return true;
        }

        public override bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }

        protected virtual bool InPossessionOfDefendant(ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            var possess = persons.FirstOrDefault(p => SubjectProperty.IsInPossessionOf(p));

            if (!defendant.Equals(possess) && !ReferenceEquals(defendant, possess))
            {
                
                AddReasonEntry($"{title} {defendant.Name}, does not " +
                               $"possess {SubjectProperty?.GetType().Name} " +
                               $"named {SubjectProperty?.Name} - it is possessed " +
                               $"by {possess?.Name}");
                return false;
            }

            return true;
        }
    }
}
