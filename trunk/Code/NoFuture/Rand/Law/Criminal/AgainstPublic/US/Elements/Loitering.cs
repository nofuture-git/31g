using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements
{
    /// <summary>
    /// wandering or remaining at some location with intent to gamble, beg or prostitute
    /// </summary>
    public class Loitering : CriminalBase, IActusReus
    {
        [Aka("panhandling")] public Predicate<ILegalPerson> IsBegging { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsGambling { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsProstituting { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var beg = IsBegging(defendant);
            var gamble = IsGambling(defendant);
            var prostitue = IsProstituting(defendant);

            if (new[] {beg, gamble, prostitue}.All(p => p == false))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsBegging)}, " +
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
