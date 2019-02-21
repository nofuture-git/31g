using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.Homicide.US.Elements
{
    /// <summary>
    /// A death or deaths that result from the commission of some other felony
    /// </summary>
    public class FelonyMurder : Murder, ITempore
    {
        public FelonyMurder(Felony felony)
        {
            SourceFelony = felony;
        }

        public DateTime Inception { get; set; }
        public DateTime? Terminus { get; set; }
        public Felony SourceFelony { get; }

        public DateTime? TimeOfTheDeath { get; set; }

        public bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception <= dt;
            var beforeOrOnToDt = Terminus == null || Terminus.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (SourceFelony == null)
            {
                AddReasonEntry($"{nameof(FelonyMurder)} implies a death which " +
                               "occurred during the commission of another felony");
                return false;
            }

            var defendant = GetDefendant(persons) ?? SourceFelony.GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!SourceFelony.IsValid(persons))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(SourceFelony)} is invalid");
                AddReasonEntryRange(SourceFelony.GetReasonEntries());
                return false;
            }

            if (TimeOfTheDeath != null && !IsInRange(TimeOfTheDeath.Value))
            {

                AddReasonEntry($"defendant, {defendant.Name}, crime started " +
                               $"at {Inception.ToString("O")} and ended at {Terminus?.ToString("O")}, " +
                               $"{nameof(TimeOfTheDeath)} at {TimeOfTheDeath?.ToString("O")} is outside this range");
                return false;
            }

            return base.IsValid(persons);
        }

        public override bool CompareTo(IMensRea criminalIntent)
        {
            if (criminalIntent is StrictLiability)
            {
                AddReasonEntry($"{nameof(FelonyMurder)} is not applicable to {nameof(StrictLiability)} intent");
                return false;
            }

            return true;
        }
    }
}
