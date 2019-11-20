using System;
using System.Linq;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Civil.US.ServiceOfProcess
{
    /// <summary>
    /// The form of service performed by some person authorized by the court to deliver summons to
    /// defendants.
    /// </summary>
    public class InPersonDelivery : DeliveryBase
    {
        /// <summary>
        /// Directs who is authorized to hand out summons 
        /// </summary>
        /// <remarks>
        /// Federal Civil Procedure Rule 4(c)(2) allows for any one who is
        /// over 18 years old and is not a party to the action.
        /// </remarks>
        public Predicate<ILegalPerson> IsToDeliverAuthorized { get; set; } = lp => lp is ICourtOfficial;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (!IsValidDateOfService(persons))
                return false;

            var nameTitles = persons.GetTitleNamePairs();
            var authPersons = persons.Where(p => IsToDeliverAuthorized(p)).ToList();
            if (!authPersons.Any())
            {
                AddReasonEntry($"{nameof(IsToDeliverAuthorized)} for all {nameTitles} is false");
                return false;
            }

            return IsDeliveredToAuthorizedPerson(authPersons);
        }
    }
}
