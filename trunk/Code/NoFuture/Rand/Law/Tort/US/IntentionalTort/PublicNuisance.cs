using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Trespass;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.US.IntentionalTort
{
    /// <summary>
    /// an unreasonable interference with a right common to the general public
    /// </summary>
    public class PublicNuisance : TrespassBase
    {
        public PublicNuisance(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalPerson> IsPublicInterference { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsHealth { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsSafety { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsPeace { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsComfort { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsConvenience { get; set; } = lp => false;

        /// <summary>
        /// proscribed by a statute, ordinance or administrative regulation
        /// </summary>
        public Predicate<ILegalPerson> IsProscribedByGovernment { get; set; } = lp => false;

        /// <summary>
        /// of a continuing nature or has produced a permanent or long-lasting effect.
        /// </summary>
        public Predicate<ILegalPerson> IsOfPermanentEffect { get; set; } = lp => false;

        /// <summary>
        /// for a private individual they must have suffered harm of a kind different from that
        /// suffered by other members of the public
        /// </summary>
        public Predicate<IPlaintiff> IsPrivatePeculiarInjury { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;

            var title = subj.GetLegalPersonTypeName();

            if (IsProscribedByGovernment(subj))
                return true;

            var interferesWithSomething = new[]
                {IsHealth(subj), IsSafety(subj), IsPeace(subj), IsComfort(subj), IsConvenience(subj)};

            if (IsPublicInterference(subj) && !interferesWithSomething.Any())
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsHealth)}, {nameof(IsSafety)}, {nameof(IsPeace)}, " +
                               $"{nameof(IsPeace)}, {nameof(IsConvenience)} are all false.");
                return false;
            }

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
            {
                return false;
            }

            if (plaintiff is IGovernment)
                return true;

            if (!IsPrivatePeculiarInjury(plaintiff))
            {
                AddReasonEntry($"{title} {plaintiff.Name}, {nameof(IsPrivatePeculiarInjury)} is false");
                return false;
            }

            return true;
        }
    }
}
