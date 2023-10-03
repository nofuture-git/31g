using System;
using System.Linq;
using NoFuture.Law.Attributes;
using NoFuture.Law.Criminal.US.Elements.Act;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Law.Criminal.US.Terms;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstGov
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
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            var gather = IsGatherer(defendant);
            var trasmit = IsTransmitor(defendant);

            if (!(_information is NationalDefenseInformation))
            {
                AddReasonEntry($"{title} {defendant.Name}, {_information.GetCategory()} " +
                               $"is not {nameof(NationalDefenseInformation)}");
                return false;
            }

            if (!gather && !trasmit)
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsGatherer)} " +
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

        public int GetRank()
        {
            return _information.GetRank();
        }
        #endregion
    }
}
