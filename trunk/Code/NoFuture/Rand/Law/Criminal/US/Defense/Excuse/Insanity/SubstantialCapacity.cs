using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse.Insanity
{
    /// <summary>
    /// A combination of <see cref="MNaghten"/> and <see cref="IrresistibleImpulse"/> 
    /// where each predicate is scaled down to only be substantial capacity instead of 
    /// total capacity see (Model Penal Code § 4.01(1)) 
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// was in greater use but after John Hinckley's assassination attempt 
    /// of President Reagan - states and fed went back to M'Naghten.
    /// ]]>
    /// </remarks>
    [Aka("ALI defense")]
    public class SubstantialCapacity : InsanityBase
    {
        public SubstantialCapacity(Func<ILegalPerson[], ILegalPerson> getSubjectPerson): base(getSubjectPerson) { }

        public SubstantialCapacity() : this(ExtensionMethods.Defendant) { }

        /// <summary>
        /// Must lack substantial, not total, capacity to know the difference between right and wrong.
        /// <![CDATA[
        /// When appreciate is the standard, must analyze the defendant’s emotional state, 
        /// character or personality as relevant and admissible.
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsMostlyWrongnessOfAware { get; set; } = lp => true;

        /// <summary>
        /// Must lack substantial, not total, ability to conform conduct to the requirements of the law.
        /// </summary>
        public Predicate<ILegalPerson> IsMostlyVolitional { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;
            var legalPerson = this.Defendant(persons);
            if (legalPerson == null)
                return false;

            if (IsMostlyVolitional(legalPerson))
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, {nameof(IsMostlyVolitional)} is true");
                return false;
            }

            if (IsMostlyWrongnessOfAware(legalPerson))
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, {nameof(IsMostlyWrongnessOfAware)} is true");
                return false;
            }

            return true;
        }
    }
}
