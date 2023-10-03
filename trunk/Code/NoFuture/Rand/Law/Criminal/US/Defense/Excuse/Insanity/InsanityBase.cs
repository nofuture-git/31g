using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Defense.Excuse.Insanity
{
    /// <summary>
    /// <![CDATA[
    /// Without the ability to control conduct, or the understanding that conduct is 
    /// evil or wrong by society's standards, in insane defendant presumably will 
    /// commit crimes again and again.  Thus no deterrent effect is served by punishment[...]
    /// ]]>
    /// </summary>
    public abstract class InsanityBase : DefenseBase
    {
        protected InsanityBase() : base(ExtensionMethods.Defendant) { }

        protected InsanityBase(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        /// <summary>
        /// cognitively impaired to the level of not knowing the nature and 
        /// quality of the criminal act or that the act is wrong
        /// </summary>
        [Aka("defect of reason", "disease of the mind")]
        public Predicate<ILegalPerson> IsMentalDefect { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = this.Defendant(persons);
            if (legalPerson == null)
                return false;

            if (!IsMentalDefect(legalPerson))
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, {nameof(IsMentalDefect)} is false");
                return false;
            }

            return true;
        }
    }
}
