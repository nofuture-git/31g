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
    /// Fed Civil Proc Rule 19(a)
    /// </remarks>
    public class OrderJoiner : Replaint
    {
        public Predicate<ILegalPerson> IsFeasible { get; set; } = lp => false;

        /// <summary>
        /// &quot;the court cannot accord complete relief among existing parties&quot;
        /// </summary>
        /// <remarks>
        /// Fed Civil Proc 19(a)(1)(A)
        /// </remarks>
        public Predicate<ILegalPerson> IsRequiredForCompleteRelief { get; set; } = lp => false;

        /// <summary>
        /// the person has an interest in the cause-of-action (subject matter) and will be effected by the outcome
        /// </summary>
        public Predicate<ILegalPerson> IsRequiredToProtectSelf { get; set; } = lp => false;

        /// <summary>
        /// one of the other parties may be exposed if they are not included
        /// </summary>
        public Predicate<ILegalPerson> IsRequiredToProtectOther { get; set; } = lp => false;


        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (persons == null)
                return false;

            var absentees = persons.Absentees().ToList();
            if (!absentees.Any())
            {
                var nameTitles = persons.GetTitleNamePairs();
                AddReasonEntry($"No one is a {nameof(IAbsentee)} among {nameTitles}");
                return false;
            }

            var rules = new List<Tuple<Predicate<ILegalPerson>, string>>
            {
                Tuple.Create(IsRequiredForCompleteRelief, nameof(IsRequiredForCompleteRelief)),
                Tuple.Create(IsRequiredToProtectSelf, nameof(IsRequiredToProtectSelf)),
                Tuple.Create(IsRequiredToProtectOther, nameof(IsRequiredToProtectOther)),
            };

            foreach (var absentee in absentees)
            {
                if (absentee == null)
                    continue;

                if (rules.All(rt => rt.Item1(absentee) == false))
                {
                    AddReasonEntry($"{absentee.GetLegalPersonTypeName()} {absentee.Name}, " +
                                   $"for {string.Join(",", rules.Select(rt => rt.Item2))} are all false");
                    return false;
                }
            }

            return base.IsValid(persons);
        }
    }
}
