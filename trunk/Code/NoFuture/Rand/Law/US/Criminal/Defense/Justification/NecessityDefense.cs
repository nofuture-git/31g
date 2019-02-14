﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Justification
{
    /// <summary>
    /// protects defendant from criminal responsibility when the defendant commits a crime to avoid a greater, imminent harm
    /// </summary>
    [Aka("choice of evils defense")]
    public class NecessityDefense<T> : DefenseBase where T : ITermCategory
    {
        public NecessityDefense(ICrime crime) : base(crime)
        {
            //Proportionality = new Proportionality<ITermCategory>(crime);
            Imminence = new Imminence(crime);
        }

        /// <summary>
        /// (1) there must be more than one harm that will occur under the circumstances
        /// </summary>
        public Predicate<ILegalPerson> IsMultipleInHarm { get; set; } = lp => false;

        /// <summary>
        /// (2) the harms must be ranked, with one of the harms ranked more severe than the other
        /// </summary>
        public Proportionality<ITermCategory> Proportionality { get; set; }

        public Func<ILegalPerson, T> GetActualFinalChoice { get; set; } = lp => default(T);

        public Func<ILegalPerson, IEnumerable<T>> GetOtherPossibleChoices { get; set; } = lp => new List<T>();

        /// <summary>
        /// (3) the defendant must have objectively reasonable belief that the greater harm is imminent
        /// </summary>
        public Imminence Imminence { get; set; }

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;
            if (Imminence != null && !Imminence.IsValid(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(Imminence)} is false");
                AddReasonEntryRange(Imminence.GetReasonEntries());
                return false;
            }
            //if (Proportionality != null && !Proportionality.IsValid(defendant))
            //{
            //    AddReasonEntry($"defendant, {defendant.Name}, {nameof(Proportionality)} is false");
            //    AddReasonEntryRange(Proportionality.GetReasonEntries());
            //    return false;
            //}

            if (!IsMultipleInHarm(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsMultipleInHarm)} is false");
                return false;
            }

            AddReasonEntryRange(Imminence?.GetReasonEntries());
            AddReasonEntryRange(Proportionality?.GetReasonEntries());

            return true;
        }
    }
}
