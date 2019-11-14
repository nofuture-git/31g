﻿using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// When another party has a right or makes a request to be added to a claim.
    /// </summary>
    /// <remarks>
    /// Fed Civil Proc Rule 24
    /// </remarks>
    [Aka("Intervention")]
    public class InterveneJoiner : OrderJoiner
    {
        public InterveneJoiner()
        {
            AnyOrRules = new List<Tuple<Predicate<ILegalPerson>, string>>
            {
                Tuple.Create(IsRequiredForCompleteRelief, nameof(IsRequiredForCompleteRelief)),
                Tuple.Create(IsRequiredToProtectSelfInterest, nameof(IsRequiredToProtectSelfInterest)),
                Tuple.Create(IsRequiredToProtectOthersExposure, nameof(IsRequiredToProtectOthersExposure)),
                Tuple.Create(IsTimely, nameof(IsTimely)),
            };
        }

        /// <summary>
        /// When the legislature has created a right of some absentee to intervene
        /// </summary>
        public Predicate<ILegalPerson> IsStatueAuthorizedRight { get; set; } = lp => false;

        /// <summary>
        /// intervention is allowed for any person
        /// who &quot;shares with the main action a common question of law or fact&quot;
        /// </summary>
        /// <remarks>
        /// The much broader rule of Fed Civil Proc 24(b)(1)(B)
        /// </remarks>
        public Predicate<ILegalPerson> IsSharesQuestionOfLawOrFact { get; set; } = lp => false;

        /// <summary>
        /// The intervening absentee must be making this request in at
        /// the appropriate time of the litigation process
        /// </summary>
        public Predicate<ILegalPerson> IsTimely { get; set; } = lp => false;

        protected override List<Tuple<Predicate<ILegalPerson>, string>> AnyOrRules { get; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (persons == null)
                return false;

            var absentees = this.Absentees(persons).ToList();
            if (!absentees.Any())
            {
                return false;
            }

            if (absentees.Any(a => IsStatueAuthorizedRight(a)))
                return true;

            if (absentees.Any(a => IsSharesQuestionOfLawOrFact(a)))
                return true;

            return base.IsValid(persons);
        }
    }
}
