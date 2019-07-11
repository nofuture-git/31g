using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Property.US.FormsOf.Intellectus;
using NoFuture.Rand.Law.Property.US.Terms;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.IntentionalTort
{
    /// <summary>
    /// Infringement on the trademark or celebrity rights of another 
    /// </summary>
    /// <remarks>
    /// Polaroid Corp. v. Polarad Elect. Corp., 287 F.2d 492 (2d Cir. 1961)
    /// <![CDATA[
    /// - Strength of the plaintiff's trademark;
    /// - Degree of similarity between the two marks at issue;
    /// - Similarity of the goods and services at issue;
    /// - Evidence of actual confusion;
    /// - Purchaser sophistication;
    /// - Quality of the defendant's goods or services;
    /// ]]>
    /// </remarks>
    [Aka("false endorsement")]
    public class TrademarkInfringement : Proportionality<Trademark>
    {
        public TrademarkInfringement(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// If consumers are actually being confused
        /// </summary>
        public bool? IsActualConfusionExist { get; set; }

        /// <summary>
        /// Similarity of the goods and services at issue
        /// </summary>
        public bool? IsProximityOfProducts { get; set; }

        /// <summary>
        /// Quality of the defendant's goods or services
        /// </summary>
        public bool? IsDefendantProductsPoorQuality { get; set; }

        /// <summary>
        /// Purchaser sophistication
        /// </summary>
        public bool? IsPurchaserSophisticated { get; set; }

        /// <summary>
        /// Defendant&apos;s intent in adopting the mark
        /// </summary>
        public IIntent Intent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();
            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;
            var pTitle = plaintiff.GetLegalPersonTypeName();

            var defendantProperty = GetChoice(subj);
            if (defendantProperty == null)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(GetChoice)} did not return a {nameof(Trademark)}");
                return false;
            }

            var plaintiffProperty = GetChoice(plaintiff);
            if (plaintiffProperty == null)
            {
                AddReasonEntry($"{pTitle} {plaintiff.Name}, {nameof(GetChoice)}  did not return a {nameof(Trademark)}");
                return false;
            }

            // first test 'the strength of his make'
            var defendantMarkStrength = defendantProperty.GetStrengthOfMark() ?? new GenericMark();
            var plaintiffMarkStrength = plaintiffProperty.GetStrengthOfMark() ?? new GenericMark();

            var isPlaintiffsMarkStronger = plaintiffMarkStrength > defendantMarkStrength;
            if (!isPlaintiffsMarkStronger)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(Trademark)} '{defendantProperty.Name}' is " +
                               $"{defendantMarkStrength.GetCategory()}");
                AddReasonEntry($"{pTitle} {plaintiff.Name}, {nameof(Trademark)} '{plaintiffProperty.Name}' is " +
                               $"{plaintiffMarkStrength.GetCategory()}");
                return false;
            }

            //second test 'the degree of similarity between the two marks'
            if (!IsProportional(defendantProperty, plaintiffProperty))
            {
                AddReasonEntry(
                    $"{title} {subj.Name}, {nameof(IsProportional)} between " +
                    $"{nameof(Trademark)} '{defendantProperty.Name}' and " +
                    $"{nameof(Trademark)} '{plaintiffProperty.Name}' is false");
                return false;
            }

            if (IsActualConfusionExist != null && IsActualConfusionExist.Value == false)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsActualConfusionExist)} is false");
                return false;
            }

            if (IsProximityOfProducts != null && IsProximityOfProducts.Value == false)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsProximityOfProducts)} is false");
                return false;
            }
            if (IsDefendantProductsPoorQuality != null && IsDefendantProductsPoorQuality.Value == false)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsDefendantProductsPoorQuality)} is false");
                return false;
            }
            if (IsPurchaserSophisticated != null && IsPurchaserSophisticated.Value == false)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsPurchaserSophisticated)} is false");
                return false;
            }

            if (Intent != null && !Intent.IsValid(persons))
            {
                AddReasonEntryRange(Intent.GetReasonEntries());
                return false;
            }

            return true;
        }

    }
}
