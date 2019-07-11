using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.IntentionalTort
{
    /// <summary>
    /// The misappropriation of a person&apos;s name and\or likeness.
    /// </summary>
    /// <remarks>
    /// Every individual has a right to control commercial use of their name, image and likeness.
    /// </remarks>
    [Aka("publicity rights", "personality rights")]
    public class FalseEndorsement : UnoHomine
    {
        public FalseEndorsement(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// Use of identity for artistic expression is not protected
        /// </summary>
        public Predicate<ILegalPerson> IsCommercialUse { get; set; } = lp => false;

        /// <summary>
        /// When the source of the use is based in artistic expression then its a protected use
        /// </summary>
        public Predicate<ILegalPerson> IsFirstAmendmentProtected { get; set; } = lp => false;

        /// <summary>
        /// By direct name, photo or any kind of distinguishing symbol
        /// which would lead a person to believe it is such and such.
        /// </summary>
        /// <remarks>
        /// <![CDATA[
        /// A rule which says that the right of publicity can be infringed
        /// only through the use of nine different methods of appropriating
        /// identity merely challenges the clever advertising strategist to
        /// come up with the tenth.
        /// ]]>
        /// White v. Samsung Electronics America, Inc.,971 F.2d 1395 (9th Cir. 1992).
        /// </remarks>
        public Func<ILegalPerson, ILegalPerson, bool> IsAppropriatedPersonIdentity { get; set; } = (l1, l2) => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;

            var title = subj.GetLegalPersonTypeName();

            if (IsFirstAmendmentProtected(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsFirstAmendmentProtected)} is true");
                return false;
            }

            if (!IsCommercialUse(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsCommercialUse)} is false");
                return false;
            }

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
            {
                return false;
            }

            var isAppropriation = IsAppropriatedPersonIdentity(subj, plaintiff) ||
                                  IsAppropriatedPersonIdentity(plaintiff, subj);

            if (!isAppropriation)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsAppropriatedPersonIdentity)} " +
                               $"of {plaintiff.GetLegalPersonTypeName()} {plaintiff.Name} is false");
                return false;
            }

            return true;
        }
    }
}
