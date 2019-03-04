﻿using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft
{
    /// <summary>
    /// Model Penal Code 223.6.
    /// </summary>
    [Aka("receiving stolen property")]
    public class ByReceiving : ConsolidatedTheft
    {
        public Predicate<ILegalPerson> IsPresentStolen { get; set; } = lp => false;

        /// <summary>
        /// The person receiving it believes its probably stolen
        /// </summary>
        public Predicate<ILegalPerson> IsApparentStolen { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var isStolen = IsPresentStolen(defendant);
            var isBelievedStolen = IsApparentStolen(defendant);

            if (!isStolen && !isBelievedStolen)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsPresentStolen)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsApparentStolen)} is false");
                return false;
            }

            if (!InPossessionOfDefendant(persons))
                return false;

            return true;
        }

        public override bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }

        protected virtual bool InPossessionOfDefendant(ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!TryGetPossesorOfProperty(out var possess))
                return false;

            if (!defendant.Equals(possess) && !ReferenceEquals(defendant, possess))
            {
                
                AddReasonEntry($"defendant, {defendant.Name}, does not " +
                               $"possess {SubjectOfTheft?.GetType().Name} " +
                               $"named {SubjectOfTheft?.Name} - it is possessed " +
                               $"by {SubjectOfTheft?.InPossessionOf.Name}");
                return false;
            }

            return true;
        }
    }
}