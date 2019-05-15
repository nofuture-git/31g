using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstGov
{
    /// <summary>
    /// the incitement of insurrection or revolution by seditious speech or
    /// writings, as such, is subject to the restrictions of the First Amendment
    /// </summary>
    public class Sedition : LegalConcept, IActusReus, IAssault, IElement
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
            var defendant = persons.Defendant();
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
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            //Yates v. U.S., 354 U.S. 298 (1957), different intent based on which predicate
            var inWrittenForm = IsInWrittenForm(defendant);
            var validIntent = inWrittenForm
                ? new[] { typeof(Purposely), typeof(SpecificIntent) }
                : new[] { typeof(Knowingly), typeof(GeneralIntent) };

            if (validIntent.All(i => criminalIntent.GetType() != i))
            {
                var nms = string.Join(", ", validIntent.Select(t => t.Name));
                AddReasonEntry($"{nameof(Sedition)} for {nameof(IsInWrittenForm)} as {inWrittenForm} requires intent {nms}");
                return false;
            }

            return true;
        }
    }
}
