using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse.Insanity
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
        protected InsanityBase(ICrime crime) : base(crime)
        {
        }

        /// <summary>
        /// cognitively impaired to the level of not knowing the nature and 
        /// quality of the criminal act or that the act is wrong
        /// </summary>
        [Aka("defect of reason", "disease of the mind")]
        public Predicate<ILegalPerson> IsMentalDefect { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!IsMentalDefect(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsMentalDefect)} is false");
                return false;
            }

            return true;
        }
    }
}
