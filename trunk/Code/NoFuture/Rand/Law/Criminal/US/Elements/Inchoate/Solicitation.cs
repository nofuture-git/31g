using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Inchoate
{
    /// <summary>
    /// precursor to <see cref="Conspiracy"/> that instigates an agreement to commit a crime.
    /// </summary>
    public class Solicitation : LegalConcept, IActusReus
    {
        /// <summary>
        /// <![CDATA[
        /// Typically some kind of verbal communication but 
        /// also, "conduct was designed to effect such communication"
        /// ]]>
        /// </summary>
        [Aka("request", "command", "encourage", "hire", "procure", "entice", "advise")]
        public Predicate<ILegalPerson> IsInduceAnotherToCrime { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (IsInduceAnotherToCrime(defendant))
            {
                return true;
            }

            AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsInduceAnotherToCrime)} is false");
            return false;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var isRequiredForConspriacy = criminalIntent is Purposely || criminalIntent is SpecificIntent;
            if (!isRequiredForConspriacy)
            {
                AddReasonEntry($"criminal intent element required for {nameof(Solicitation)} is specific intent or purposely");
                return false;
            }

            return true;
        }
    }
}
