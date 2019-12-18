using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstPublic
{
    /// <summary>
    /// wandering or remaining at some location with intent to gamble, beg or prostitute
    /// </summary>
    public class Loitering : LegalConcept, IActusReus
    {
        [Aka("panhandling")] public Predicate<ILegalPerson> IsBegging { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsGambling { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsProstituting { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            var beg = IsBegging(defendant);
            var gamble = IsGambling(defendant);
            var prostitue = IsProstituting(defendant);

            if (new[] {beg, gamble, prostitue}.All(p => p == false))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsBegging)}, " +
                               $"{nameof(IsGambling)} and {nameof(IsProstituting)} are false");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var isvalid = criminalIntent is Purposely || criminalIntent is SpecificIntent;

            if (!isvalid)
            {
                AddReasonEntry($"{nameof(Loitering)} requires intent " +
                               $"of {nameof(Recklessly)}, {nameof(Purposely)}, " +
                               $"{nameof(SpecificIntent)}");
                return false;
            }

            return true;
        }
    }
}
