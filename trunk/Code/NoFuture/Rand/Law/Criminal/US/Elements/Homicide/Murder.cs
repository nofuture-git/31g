using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.Homicide
{
    /// <summary>
    /// unlawful conduct that causes the victims death
    /// </summary>
    [Aka("criminal homicide")]
    public abstract class Murder : LegalConcept, IActusReus
    {
        /// <summary>
        /// death of the victim caused by defendant in an unlawful manner
        /// </summary>
        /// <remarks>
        /// <![CDATA[
        /// is designed to guard against the "hasty and unguarded character 
        /// which is often attached to confessions and admissions and the 
        /// consequent danger of a conviction where no crime has in fact 
        /// been committed."
        /// ]]>
        /// </remarks>
        public Predicate<ILegalPerson> IsCorpusDelicti { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!IsCorpusDelicti(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsCorpusDelicti)} is false");
                return false;
            }

            return true;
        }

        public virtual bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var adequateProvocation = criminalIntent as AdequateProvocation;
            var defendant = this.Defendant(persons);
            var title = defendant.GetLegalPersonTypeName();
            if (adequateProvocation != null && defendant != null && adequateProvocation.IsValid(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, is criminal intent {nameof(AdequateProvocation)} " +
                               "which is only applicable to manslaughter");
                return false;
            }

            return true;
        }


        internal static bool IsHomicideConcurrance(IHomicideConcurrance hc, IRationale rationale, string defendantName = null, string title = null)
        {
            if (hc == null || rationale == null)
                return true;
            title = title ?? "defendant";
            if (hc.TimeOfTheDeath != null && !hc.IsInRange(hc.TimeOfTheDeath.Value))
            {

                rationale.AddReasonEntry($"{title} {defendantName}, crime started " +
                                         $"at {hc.Inception.ToString("O")} and ended at {hc.Terminus?.ToString("O")}, " +
                                         $"{nameof(IHomicideConcurrance.TimeOfTheDeath)} at {hc.TimeOfTheDeath?.ToString("O")} is outside this range");
                return false;
            }

            return true;
        }
    }
}
