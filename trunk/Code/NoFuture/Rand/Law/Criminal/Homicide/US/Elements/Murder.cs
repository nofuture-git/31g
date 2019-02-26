using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.Homicide.US.Elements
{
    /// <summary>
    /// unlawful conduct that causes the victims death
    /// </summary>
    [Aka("criminal homicide")]
    public abstract class Murder : CriminalBase, IActusReus
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
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsCorpusDelicti(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsCorpusDelicti)} is false");
                return false;
            }

            return true;
        }

        public virtual bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var adequateProvation = criminalIntent as AdequateProvocation;
            var defendant = GetDefendant(persons);
            if (adequateProvation != null && defendant != null && adequateProvation.IsValid(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, is criminal intent {nameof(AdequateProvocation)} " +
                               "which is only applicable to manslaughter");
                return false;
            }

            return true;
        }


        internal static bool IsHomicideConcurrance(IHomicideConcurrance hc, IRationale rationale, string defendantName = null)
        {
            if (hc == null || rationale == null)
                return true;

            if (hc.TimeOfTheDeath != null && !hc.IsInRange(hc.TimeOfTheDeath.Value))
            {

                rationale.AddReasonEntry($"defendant {defendantName}, crime started " +
                                         $"at {hc.Inception.ToString("O")} and ended at {hc.Terminus?.ToString("O")}, " +
                                         $"{nameof(IHomicideConcurrance.TimeOfTheDeath)} at {hc.TimeOfTheDeath?.ToString("O")} is outside this range");
                return false;
            }

            return true;
        }
    }
}
