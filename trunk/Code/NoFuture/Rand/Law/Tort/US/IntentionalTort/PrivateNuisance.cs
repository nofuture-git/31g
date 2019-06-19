using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Trespass;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.US.Property;

namespace NoFuture.Rand.Law.Tort.US.IntentionalTort
{
    /// <summary>
    /// unreasonable interferences with the use and enjoyment of land
    /// </summary>
    public class PrivateNuisance : PropertyConsent
    {
        public PrivateNuisance(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<IPlaintiff> IsInvasionOfProtectableInterest { get; set; } = pl => false;

        public Predicate<ILegalPerson> IsIntentionalInvasion { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsUnreasonableInvasion { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsNegligentInvasion { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsRecklessInvasion { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsAbnormallyDangerous { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            if (SubjectProperty == null)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(SubjectProperty)} is unassigned");
            }

            if (!WithoutConsent(persons))
            {
                return false;
            }

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;
            var pTitle = plaintiff.GetLegalPersonTypeName();

            if (!IsInvasionOfProtectableInterest(plaintiff))
            {
                AddReasonEntry($"{pTitle} {plaintiff.Name}, {nameof(IsInvasionOfProtectableInterest)} is false");
                return false;
            }

            var predicates = new List<Tuple<bool, string>>
            {
                Tuple.Create(IsIntentionalInvasion(subj), nameof(IsIntentionalInvasion)),
                Tuple.Create(IsUnreasonableInvasion(subj), nameof(IsUnreasonableInvasion)),
                Tuple.Create(IsNegligentInvasion(subj), nameof(IsNegligentInvasion)),
                Tuple.Create(IsRecklessInvasion(subj), nameof(IsRecklessInvasion)),
                Tuple.Create(IsAbnormallyDangerous(subj), nameof(IsAbnormallyDangerous)),
            };

            if (predicates.All(t => t.Item1 == false))
            {
                var predicateNames = string.Join(" ", predicates.Select(t => t.Item2));
                AddReasonEntry($"{title} {subj.Name}, {predicateNames} are all false");
                return false;
            }

            return true;
        }
    }
}
