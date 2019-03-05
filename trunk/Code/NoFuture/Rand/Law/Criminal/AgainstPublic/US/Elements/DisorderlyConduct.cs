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
    /// criminal conduct that negatively impacts the quality of
    /// life for citizens in any given city, county or state.
    /// </summary>
    [Aka("disturbing the peace")]
    public class DisorderlyConduct: CriminalBase, IActusReus
    {
        /// <summary>
        /// a loud and unreasonable noise given the context-setting (e.g. library v. city street)
        /// </summary>
        public Predicate<ILegalPerson> IsUnreasonablyLoud { get; set; } = lp => false;

        /// <summary>
        /// obscene utterance or gesture
        /// </summary>
        public Predicate<ILegalPerson> IsObscene { get; set; } = lp => false;

        /// <summary>
        /// engage in fighting or threatening, or state fighting words
        /// </summary>
        public Predicate<ILegalPerson> IsCombative { get; set; } = lp => false;

        /// <summary>
        /// A situation that is dangerous and poses a risk to others in
        /// the vicinity of the defendant&apos;s conduct
        /// </summary>
        public Predicate<ILegalPerson> IsIllegitimateHazardous { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var loud = IsUnreasonablyLoud(defendant);
            var obscene = IsObscene(defendant);
            var combative = IsCombative(defendant);
            var hazard = IsIllegitimateHazardous(defendant);

            if (new[] {loud, obscene, combative, hazard}.All(p => false))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsUnreasonablyLoud)}, " +
                               $"{nameof(IsObscene)}, {nameof(IsCombative)} " +
                               $"and {nameof(IsIllegitimateHazardous)} is false");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var validIntent = criminalIntent is Recklessly || criminalIntent is Purposely ||
                              criminalIntent is SpecificIntent;
            if (!validIntent)
            {
                AddReasonEntry($"{nameof(DisorderlyConduct)} requires intent " +
                               $"of {nameof(Recklessly)}, {nameof(Purposely)}, " +
                               $"{nameof(SpecificIntent)}");
                return false;
            }

            return true;
        }
    }
}
