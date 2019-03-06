using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstGov.US.Elements
{
    /// <summary>
    /// the incitement of insurrection or revolution by seditious speech or
    /// writings, as such, is subject to the restrictions of the First Amendment
    /// </summary>
    public class Sedition : CriminalBase, IActusReus, IAssault
    {
        /// <summary>
        /// either advocating, aiding, teaching, organizing or printing, publishing,
        /// or circulating written matter that advocates, aids, or
        /// teaches [...] by force or violence
        /// </summary>
        public Predicate<ILegalPerson> IsByThreatOfViolence { get; set; } = lp => false;

        /// <summary>
        /// the overthrow of the US government or any state, district, or territory
        /// </summary>
        public Predicate<ILegalPerson> IsToOverthrowGovernment { get; set; } = lp => false;

        /// <summary>
        /// specific intent or purposely to print, publish, or circulate
        /// written matter that advocates, aids, or teaches the violent
        /// government overthrow
        /// </summary>
        public Predicate<ILegalPerson> IsInWrittenForm { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsByThreatOfViolence(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByThreatOfViolence)} is false");
                return false;
            }

            if (!IsToOverthrowGovernment(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsToOverthrowGovernment)} is false");
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

            //Yates v. U.S., 354 U.S. 298 (1957), different intent based on which predicate
            if (IsInWrittenForm(defendant))
            {
                isValid = criminalIntent is Purposely || criminalIntent is SpecificIntent;

                if (!isValid)
                {
                    AddReasonEntry($"{nameof(Sedition)}, when {nameof(IsInWrittenForm)} is true," +
                                   $" requires intent of {nameof(Purposely)} or {nameof(SpecificIntent)}");
                    return false;
                }

                return true;
            }

            isValid = criminalIntent is Knowingly || criminalIntent is GeneralIntent;

            if (!isValid)
            {
                AddReasonEntry($"{nameof(Treason)}, when {nameof(IsInWrittenForm)} is false," +
                               $" requires intent of {nameof(Knowingly)} or {nameof(GeneralIntent)}");
                return false;
            }

            return true;
        }
    }
}
