using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Property.US;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    public class ProductStrictLiability : PropertyConsent, IProximateCause<ILegalProperty>
    {
        public ProductStrictLiability(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// As measured by the reasonable expectations of the user - consumer&apos;s expectations
        /// </summary>
        /// <remarks>
        /// The defect or defectiveness concept has embraced a great variety of injury-producing deficiencies,
        /// </remarks>
        [Aka("unreasonably dangerous to the user", "design defect", "warning defect", "manufacturing defect")]
        public Predicate<ILegalProperty> IsDefectiveAtTimeOfSale { get; set; } = p => false;

        /// <summary>
        /// given what we know now, we might well conclude that the product is substandard
        /// </summary>
        public Predicate<ILegalProperty> IsDeprecatedDesign { get; set; } = p => false;

        public Predicate<ILegalProperty> IsDirectCause { get; set; } = p => false;

        public IInjury Injury { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }

            var title = subj.GetLegalPersonTypeName();
            if (SubjectProperty == null)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(SubjectProperty)} is unassigned");
                return false;
            }

            if (!IsDefectiveAtTimeOfSale(SubjectProperty) && !IsDeprecatedDesign(SubjectProperty))
            {
                AddReasonEntry($"{title} {subj.Name}, both {nameof(IsDefectiveAtTimeOfSale)} " +
                               $"and {nameof(IsDeprecatedDesign)} are false " +
                               $"for the property {SubjectProperty.Name}");
                return false;
            }

            if (!IsDirectCause(SubjectProperty))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsDirectCause)} is false " +
                               $"for the property {SubjectProperty.Name}");
                return false;
            }

            if (Injury == null)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(Injury)} is unassigned");
                return false;
            }

            var result = Injury.IsValid(persons);
            AddReasonEntryRange(Injury.GetReasonEntries());
            return result;
        }

    }
}
