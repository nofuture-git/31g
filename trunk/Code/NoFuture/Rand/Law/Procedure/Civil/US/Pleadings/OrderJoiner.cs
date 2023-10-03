using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;

namespace NoFuture.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// When another party is order by the court to be added in order to fairly act
    /// </summary>
    /// <remarks>
    /// Fed Civil Proc Rule 19
    /// </remarks>
    public class OrderJoiner : Complaint
    {
        public Predicate<ILegalPerson> IsFeasible { get; set; } = lp => true;

        /// <summary>
        /// &quot;the court cannot accord complete relief among existing parties&quot;
        /// </summary>
        public Predicate<ILegalPerson> IsRequiredForCompleteRelief { get; set; } = lp => false;

        /// <summary>
        /// the absentee may be subject to contradictory or inconsistent court orders
        /// </summary>
        /// <remarks>
        /// No the same as court judgments, the idea is to avoid inconsistent obligations
        /// </remarks>
        public Predicate<ILegalPerson> IsRequiredToAvoidContradictoryObligations { get; set; } = lp => false;

        /// <summary>
        /// one of the other parties may be exposed if they are not included
        /// </summary>
        public Predicate<ILegalPerson> IsRequiredToProtectOthersExposure { get; set; } = lp => false;

        protected virtual List<Tuple<Predicate<ILegalPerson>, string>> AnyOrRules { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            AnyOrRules = new List<Tuple<Predicate<ILegalPerson>, string>>
            {
                Tuple.Create(IsRequiredForCompleteRelief, nameof(IsRequiredForCompleteRelief)),
                Tuple.Create(IsRequiredToAvoidContradictoryObligations, nameof(IsRequiredToAvoidContradictoryObligations)),
                Tuple.Create(IsRequiredToProtectOthersExposure, nameof(IsRequiredToProtectOthersExposure)),
            };

            if (persons == null)
                return false;

            var absentees = this.Absentees(persons).ToList();
            if (!absentees.Any())
            {
                return false;
            }

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
                
                var absenteeShouldBeJoined = AnyOrRules.Any(rt => rt.Item1(absentee));

                if (!absenteeShouldBeJoined)
                {
                    AddReasonEntry($"{title} {absentee.Name}, " +
                                   $"for {string.Join(",", AnyOrRules.Select(rt => rt.Item2))} are all false");
                    return false;
                }
            }

            return base.IsValid(persons);
        }
    }
}
