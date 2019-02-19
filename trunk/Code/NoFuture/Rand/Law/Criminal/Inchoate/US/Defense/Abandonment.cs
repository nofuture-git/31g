﻿using System;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;

namespace NoFuture.Rand.Law.Criminal.Inchoate.US.Defense
{
    /// <summary>
    /// A defense against <see cref="Attempt"/> which was motivated by goodness
    /// </summary>
    public class Abandonment : DefenseBase
    {
        public Abandonment(ICrime crime) : base(crime)
        {
        }

        public Predicate<ILegalPerson> IsMotivatedByFearOfGettingCaught { get; set; } = lp => true;

        public Predicate<ILegalPerson> IsMotivatedByNewDifficulty { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = Crime.GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!Impossibility.TestIsActusReusAttemptType(this))
                return false;

            if (IsMotivatedByFearOfGettingCaught(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsMotivatedByFearOfGettingCaught)} is true");
                return false;
            }

            if (IsMotivatedByNewDifficulty(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsMotivatedByNewDifficulty)} is true");
                return false;
            }

            return true;
        }
    }
}