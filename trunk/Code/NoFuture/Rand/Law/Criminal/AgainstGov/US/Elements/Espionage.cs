using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.AgainstGov.US.Terms;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.AgainstGov.US.Elements
{
    /// <summary>
    /// to actually or attempt to gather, transmit defense information 
    /// </summary>
    [Aka("spying")]
    public class Espionage : LegalConcept, ICapitalOffense, ITermCategory
    {
        private readonly ITermCategory _information;

        public Espionage(ITermCategory information = null)
        {
            _information = information ?? new NationalDefenseInformation();
        }

        public Espionage(string name) : this(new NationalDefenseInformation(name)) { }

        /// <summary>
        /// Actually or attempt to gather of defense information 
        /// </summary>
        public Predicate<ILegalPerson> IsGatherer { get; set; } = lp => false;

        /// <summary>
        /// Actually or attempt to transmit of defense information 
        /// </summary>
        public Predicate<ILegalPerson> IsTransmitor { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            var gather = IsGatherer(defendant);
            var trasmit = IsTransmitor(defendant);

            if (!(_information is NationalDefenseInformation))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {_information.GetCategory()} " +
                               $"is not {nameof(NationalDefenseInformation)}");
                return false;
            }

            if (!gather && !trasmit)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsGatherer)} " +
                               $"and {nameof(IsTransmitor)} of {_information.GetCategory()} are false");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var validIntent = _information is MilitaryActivities
                ? new[] {typeof(Purposely), typeof(SpecificIntent)}
                : new[] {typeof(Purposely), typeof(SpecificIntent), typeof(Knowingly), typeof(GeneralIntent)};

            if (validIntent.All(i => criminalIntent.GetType() != i))
            {
                var nms = string.Join(", ", validIntent.Select(t => t.Name));
                AddReasonEntry($"{nameof(Espionage)} for {_information.GetCategory()} requires intent {nms}");
                return false;
            }

            return true;
        }

        #region ITermCategory HAS-A IS-A
        public string GetCategory()
        {
            return _information.GetCategory();
        }

        public bool IsCategory(ITermCategory category)
        {
            return _information.IsCategory(category);
        }

        public ITermCategory As(ITermCategory category)
        {
            return _information.As(category);
        }

        public bool IsCategory(Type category)
        {
            return _information.IsCategory(category);
        }

        public int GetCategoryRank()
        {
            return _information.GetCategoryRank();
        }
        #endregion
    }
}
