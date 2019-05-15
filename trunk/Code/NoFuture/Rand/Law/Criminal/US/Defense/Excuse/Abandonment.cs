﻿using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Inchoate;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <summary>
    /// A defense against Attempt which was motivated by goodness
    /// </summary>
    public class Abandonment : InchoateDefenseBase
    {
        public Abandonment(ICrime crime) : base(crime)
        {
        }

        public Predicate<ILegalPerson> IsMotivatedByFearOfGettingCaught { get; set; } = lp => true;

        public Predicate<ILegalPerson> IsMotivatedByNewDifficulty { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!TestIsActusReusOfType(typeof(Attempt)))
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
