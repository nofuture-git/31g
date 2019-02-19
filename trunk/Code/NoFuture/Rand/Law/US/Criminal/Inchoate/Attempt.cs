using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US.Criminal.Elements;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent;
using NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.US.Criminal.Inchoate
{
    /// <summary>
    /// a crime that has only just begun which may be defined by statute or common law
    /// </summary>
    public class Attempt: CrimeBase
    {
        private readonly ICrime _crime;

        public Attempt(ICrime crime)
        {
            _crime = crime;
        }

        /// <summary>
        /// A measure of criminal effort left to be done, not a measure of what is already done.
        /// </summary>
        public Predicate<ILegalPerson> IsProximity { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[ "thing itself speaks," meaning its clear that there is not other purpose but criminal ]]>
        /// </summary>
        [Aka("unequivocality test")]
        public Predicate<ILegalPerson> IsResIpsaLoquitur { get; set; } = lp => false;

        /// <summary>
        /// a point of progress where its probable that the crime will happen unless some exogenous force intervenes
        /// </summary>
        public Predicate<ILegalPerson> IsProbableDesistance { get; set; } = lp => false;

        /// <summary>
        /// Some kind of action which corroborates a proposistion of intent - evidence of a suspicion
        /// </summary>
        /// <remarks>
        /// <![CDATA[
        /// Model Penal Code examples: 
        /// * lying in wait; 
        /// * enticing victem to scene; 
        /// * investigating potential scene; 
        /// * unlawful entry; 
        /// * possessing materials specially designed for unlawful use; 
        /// * possessing, collecting, or fabricating materials to be used in crime
        /// ]]>
        /// </remarks>
        public Predicate<ILegalPerson> IsSubstantial { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (MensRea is Recklessly || MensRea is Negligently)
            {
                AddReasonEntry($"defendant, {defendant.Name}, " +
                               "generally, no such thing exists as reckless or negligent attempt");
                return false;
            }

            if (ActusReus?.IsValid(persons) ?? false)
            {
                AddReasonEntry($"defendant, {defendant.Name}, has completed " +
                               "the crime therefore it cannot be an attempt");
                AddReasonEntryRange(ActusReus?.GetReasonEntries());
                return false;
            }

            if (!MensRea?.IsValid(persons) ?? false)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(MensRea)} is false");
                AddReasonEntryRange(MensRea?.GetReasonEntries());
                return false;
            }

            var ispx = IsProximity(defendant);
            var ispd = IsProbableDesistance(defendant);
            var isril = IsResIpsaLoquitur(defendant);
            var issub = IsSubstantial(defendant);

            var isAttempt = ispx || ispd || isril || issub;
            if (!isAttempt)
            {
                AddReasonEntry($"defendant, {defendant.Name}, is attempt false");
                return false;
            }

            return true;
        }

        public override Concurrence Concurrence
        {
            get => _crime?.Concurrence;
            set
            {
                if (_crime != null)
                    _crime.Concurrence = value;
            }
        }

        public override ActusReus ActusReus
        {
            get => _crime?.Concurrence?.ActusReus;
            set
            {
                if (_crime?.Concurrence?.ActusReus != null)
                    _crime.Concurrence.ActusReus = value;
            }
        }

        public override MensRea MensRea
        {
            get => _crime?.Concurrence.MensRea;
            set
            {
                if (_crime?.Concurrence?.MensRea != null)
                    _crime.Concurrence.MensRea = value;
            }
        }

        public override IList<IElement> AdditionalElements => _crime.AdditionalElements;

        public override int CompareTo(object obj)
        {
            return _crime.CompareTo(obj);
        }
    }
}
