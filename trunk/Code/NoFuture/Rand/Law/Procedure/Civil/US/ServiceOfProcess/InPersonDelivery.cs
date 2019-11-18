using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Civil.US.ServiceOfProcess
{
    /// <summary>
    /// The form of service performed by court-official, such as sheriff, U.S. Marshal, constable
    /// </summary>
    public class InPersonDelivery : CivilProcedureBase
    {
        /// <summary>
        /// State procedure directs who is authorized to hand out summons 
        /// </summary>
        /// <remarks>
        /// Federal Civil Procedure Rule 4(c)(2) allows for any one who is
        /// over 18 years old and is not a party to the action.
        /// </remarks>
        public Predicate<ILegalPerson> IsAuthorizedPerson { get; set; } = lp => false;

        /// <summary>
        /// Defines who delivered to who where expectation is law enforcement delivered to the the defendant
        /// </summary>
        public Func<ILegalPerson, ILegalPerson> GetDeliveredTo { get; set; } = lp => null;

        public override bool IsValid(params ILegalPerson[] persons)
        {

            if (!IsCourtAssigned())
                return false;

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var nameTitles = persons.GetTitleNamePairs();
            var authPersons = persons.Where(p => IsAuthorizedPerson(p)).ToList();
            if (!authPersons.Any())
            {
                AddReasonEntry($"{nameof(IsAuthorizedPerson)} for all {nameTitles} is false");
                return false;
            }

            var deliveredDefendant = authPersons.Where(c => GetDeliveredTo(c) is IDefendant)
                                                .Select(c => GetDeliveredTo(c))
                                                .Cast<IDefendant>()
                                                .ToList();

            if (!deliveredDefendant.Any())
            {
                nameTitles = authPersons.GetTitleNamePairs();
                AddReasonEntry($"{nameof(ICourtOfficial)} in {nameTitles}, " +
                               $"{nameof(GetDeliveredTo)} returned no one who " +
                               $"is {nameof(IDefendant)}");
                return false;
            }

            if (deliveredDefendant.All(df => !NamesEqual(df, defendant)))
            {
                nameTitles = deliveredDefendant.GetTitleNamePairs();
                AddReasonEntry($"{defendant.GetLegalPersonTypeName()} {defendant.Name}, {nameof(NamesEqual)} for all " +
                               $"{nameTitles} is false");
                return false;
            }

            return true;
        }
    }
}
