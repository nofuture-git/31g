﻿using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    /// <summary>
    /// A reasoning less than <see cref="ProbableCause"/> but above something like &quot;a hunch&quot;
    /// </summary>
    public class ReasonableSuspicion : ProbableCause
    {
        protected override bool DefaultIsInformationSourceCredible(ILegalPerson lp)
        {
            if (lp is ILawEnforcement)
            {
                AddReasonEntry($"{lp.GetLegalPersonTypeName()} {lp.Name}, is type {nameof(ILawEnforcement)}");
                return true;
            }

            var informant = lp as IInformant;
            if (informant == null)
            {
                AddReasonEntry($"{lp.GetLegalPersonTypeName()} {lp.Name}, is not " +
                               $"of type {nameof(ILawEnforcement)} nor {nameof(IInformant)}");
                return false;
            }

            return informant.IsInformationSufficientlyReliable;
        }

        public override int GetRank()
        {
            return base.GetRank() - 1;
        }
    }
}
