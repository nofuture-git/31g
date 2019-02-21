using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.Homicide.US.Elements
{
    /// <summary>
    /// conduct that causes the victems death
    /// </summary>
    public abstract class Murder : CriminalBase, IActusReus
    {
        /// <summary>
        /// death of the victem caused by defendant in an unlawful manner
        /// </summary>
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

        public abstract bool CompareTo(IMensRea criminalIntent);
    }
}
