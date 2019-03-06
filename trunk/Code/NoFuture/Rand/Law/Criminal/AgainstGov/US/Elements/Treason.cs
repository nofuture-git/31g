using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstGov.US.Elements
{
    /// <summary>
    /// Article III Section 3 of the US Constitution
    /// </summary>
    public class Treason : CriminalBase, ICapitalOffense, IBattery
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
            var defendant = GetDefendant(persons);
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
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;
            bool isValid = false;

            //Cramer v. U.S., 1945, different intent based on which predicate
            if (IsByViolence(defendant))
            {
                isValid = criminalIntent is Purposely || criminalIntent is SpecificIntent;

                if (!isValid)
                {
                    AddReasonEntry($"{nameof(Treason)}, when {nameof(IsByViolence)} is true," +
                                   $" requires intent of {nameof(Purposely)} or {nameof(SpecificIntent)}");
                    return false;
                }

                return true;
            }

            isValid = criminalIntent is Knowingly || criminalIntent is GeneralIntent;

            if (!isValid)
            {
                AddReasonEntry($"{nameof(Treason)}, when {nameof(IsByViolence)} is false," +
                               $" requires intent of {nameof(Knowingly)} or {nameof(GeneralIntent)}");
                return false;
            }

            return true;
        }
    }
}
