using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.Inchoate
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
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            if (IsInduceAnotherToCrime(defendant))
            {
                return true;
            }

            var title = defendant.GetLegalPersonTypeName();
            AddReasonEntry($"{title} {defendant.Name}, {nameof(IsInduceAnotherToCrime)} is false");
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
