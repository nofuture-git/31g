using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.AgainstGov.US.Elements
{
    /// <summary>
    /// Article III Section 3 of the US Constitution
    /// </summary>
    public class Treason : LegalConcept, ICapitalOffense, IBattery, IElement
    {
        /// <summary>
        /// Active belligerent or enemy-combative [levying War against them]
        /// </summary>
        public Predicate<ILegalPerson> IsByViolence { get; set; } = lp => false;

        /// <summary>
        /// Joining the enemy 
        /// </summary>
        public Predicate<ILegalPerson> IsAdheringToEnemy { get; set; } = lp => false;

        /// <summary>
        /// Providing for the enemy 
        /// </summary>
        public Predicate<ILegalPerson> IsGivingAidComfort { get; set; } = lp => false;

        public ILegalPerson WitnessOne { get; set; }
        public ILegalPerson WitnessTwo { get; set; }

        public Predicate<ILegalPerson> IsCourtConfession { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            var levy = IsByViolence(defendant);
            var adhere = IsAdheringToEnemy(defendant);
            var aid = IsGivingAidComfort(defendant);
            if (new[] {levy, adhere, aid}.All(p => p == false))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByViolence)}, " +
                               $"{nameof(IsAdheringToEnemy)}, {nameof(IsGivingAidComfort)} are all false");
                return false;
            }

            var confess = IsCourtConfession(defendant);
            if (confess)
                return true;

            var one = WitnessOne == null;
            var two = WitnessTwo == null;

            if (one || two)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(WitnessOne)} being null is {one}");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(WitnessTwo)} being null is {two}");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            var isByViolence = IsByViolence(defendant);
            var validIntent = isByViolence
                ? new[] {typeof(Purposely), typeof(SpecificIntent)}
                : new[] {typeof(Knowingly), typeof(GeneralIntent)};

            //Cramer v. U.S., 1945, different intent based on which predicate
            if (validIntent.All(i => criminalIntent.GetType() != i))
            {
                var nms = string.Join(", ", validIntent.Select(t => t.Name));
                AddReasonEntry($"{nameof(Treason)} for {nameof(IsByViolence)} as {isByViolence} requires intent {nms}");
                return false;
            }

            return true;
        }
    }
}
