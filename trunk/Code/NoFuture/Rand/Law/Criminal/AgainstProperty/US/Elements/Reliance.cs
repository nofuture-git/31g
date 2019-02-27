using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements
{
    /// <summary>
    /// An attendant circumstance in which the <see cref="IVictim"/> was not only 
    /// deceived but also relied upon that deception.
    /// </summary>
    public class Reliance : AttendantCircumstances
    {
        public Predicate<ILegalPerson> IsReliantOnFalseRepresentation { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            return false;
        }

        public override bool IsValid(IActusReus criminalAct, params ILegalPerson[] persons)
        {
            var theft = criminalAct as ConsolidatedTheft;
            if(theft == null || persons == null || !persons.Any())
                return base.IsValid(criminalAct, persons);

            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var victims = persons.Where(lp => lp is IVictim).ToList();
            if (!victims.Any())
                return false;

            foreach (var victim in victims)
            {
                var isReliance = IsReliantOnFalseRepresentation(victim);
                AddReasonEntry($"victim, {victim.Name}, {nameof(IsReliantOnFalseRepresentation)} is {isReliance}");

                if (isReliance && (IsLarcenyByTrick(criminalAct, persons) || IsFalsePretenses(criminalAct, persons)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// When a thief tricks a victim to hand over ownwership of personal property
        /// </summary>
        /// <param name="criminalAct"></param>
        /// <param name="persons"></param>
        /// <returns></returns>
        protected virtual bool IsLarcenyByTrick(IActusReus criminalAct, ILegalPerson[] persons)
        {
            if (persons == null || !persons.Any())
                return base.IsValid(criminalAct, persons);

            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var larceny = criminalAct as ByTaking;
            var personalProperty = larceny?.SubjectOfTheft as PersonalProperty;
            var isTrick = (larceny?.IsTransferUnlawful(defendant) ?? false);
            var isLarcenyByTrick = isTrick && personalProperty != null;

            if(isLarcenyByTrick)
                AddReasonEntry($"{nameof(isLarcenyByTrick)} is {isLarcenyByTrick} since the " +
                               $"{nameof(IActusReus)} is {nameof(ByTaking)} with " +
                               $"{nameof(ByTaking.IsTransferUnlawful)} being true for " +
                               $"{nameof(PersonalProperty)} named {personalProperty?.Name}");

            return isLarcenyByTrick;
        }

        /// <summary>
        /// When a thief pretends to perform some service on which the victim was reliant
        /// </summary>
        /// <param name="criminalAct"></param>
        /// <param name="persons"></param>
        /// <returns></returns>
        protected virtual bool IsFalsePretenses(IActusReus criminalAct, ILegalPerson[] persons)
        {
            if (persons == null || !persons.Any())
                return base.IsValid(criminalAct, persons);

            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var deception = criminalAct as ByDeception;
            var service = deception?.SubjectOfTheft as ActOfService;
            var isFalsePretenses = (deception?.IsFalseImpression(defendant) ?? false)
                                    && service != null;

            if(isFalsePretenses)
                AddReasonEntry($"{nameof(isFalsePretenses)} is {isFalsePretenses} since the {nameof(IActusReus)} " +
                               $"is {nameof(ByDeception)} with {nameof(ByDeception.IsFalseImpression)} being " +
                               $"true for {nameof(ActOfService)} named {service?.Name}");

            return isFalsePretenses;
        }
    }
}
