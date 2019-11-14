using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// When another party is order by the court to be added in order to fairly make a judgment
    /// </summary>
    /// <remarks>
    /// Fed Civil Proc Rule 19
    /// </remarks>
    public class OrderJoiner : Replaint
    {
        public Predicate<ILegalPerson> IsFeasible { get; set; } = lp => true;

        /// <summary>
        /// &quot;the court cannot accord complete relief among existing parties&quot;
        /// </summary>
        public Predicate<ILegalPerson> IsRequiredForCompleteRelief { get; set; } = lp => false;

        /// <summary>
        /// the person has an interest in the cause-of-action (subject matter) and will be effected by the outcome
        /// </summary>
        public Predicate<ILegalPerson> IsRequiredToProtectSelfInterest { get; set; } = lp => false;

        /// <summary>
        /// one of the other parties may be exposed if they are not included
        /// </summary>
        public Predicate<ILegalPerson> IsRequiredToProtectOthersExposure { get; set; } = lp => false;


        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (persons == null)
                return false;

            var absentees = this.Absentees(persons).ToList();
            if (!absentees.Any())
            {
                return false;
            }

            var rules = new List<Tuple<Predicate<ILegalPerson>, string>>
            {
                Tuple.Create(IsRequiredForCompleteRelief, nameof(IsRequiredForCompleteRelief)),
                Tuple.Create(IsRequiredToProtectSelfInterest, nameof(IsRequiredToProtectSelfInterest)),
                Tuple.Create(IsRequiredToProtectOthersExposure, nameof(IsRequiredToProtectOthersExposure)),
            };

            foreach (var absentee in absentees)
            {
                if (absentee == null)
                    continue;

                var title = absentee.GetLegalPersonTypeName();

                //when an absentee cannot be joined but they are indispensable
                if (!IsFeasible(absentee) && absentee.IsIndispensable)
                {
                    AddReasonEntry($"{title} {absentee.Name}, {nameof(IsFeasible)} is false " +
                                   $"while {nameof(IAbsentee)} {nameof(IAbsentee.IsIndispensable)} is true");
                    return false;
                }
                
                var absenteeShouldBeJoined = rules.Any(rt => rt.Item1(absentee));

                if (!absenteeShouldBeJoined)
                {
                    AddReasonEntry($"{title} {absentee.Name}, " +
                                   $"for {string.Join(",", rules.Select(rt => rt.Item2))} are all false");
                    return false;
                }
            }

            return base.IsValid(persons);
        }
    }
}
