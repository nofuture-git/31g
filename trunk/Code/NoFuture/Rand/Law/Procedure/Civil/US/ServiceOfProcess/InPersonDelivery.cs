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
    public class InPersonDelivery : CivilProcedureBase
    {
        /// <summary>
        /// Directs who is authorized to hand out summons 
        /// </summary>
        /// <remarks>
        /// Federal Civil Procedure Rule 4(c)(2) allows for any one who is
        /// over 18 years old and is not a party to the action.
        /// </remarks>
        public Predicate<ILegalPerson> IsToDeliverAuthorized { get; set; } = lp => lp is ICourtOfficial;

        /// <summary>
        /// Directs who is authorized to receive a summons on behalf of a defendant
        /// </summary>
        public Predicate<ILegalPerson> IsToReceiveAuthorized { get; set; } = lp => lp is IDefendant;

        /// <summary>
        /// Defines who delivered to who.
        /// </summary>
        /// <remarks>
        /// Simplest example being a U.S. marshal delivering directly to defendant.
        /// Others are delivery to appropriate 3rd party at defendant&apos;s home,
        /// defendant&apos;s attorney, etc.
        /// </remarks>
        public Func<ILegalPerson, ILegalPerson> GetDeliveredTo { get; set; } = lp => null;

        public override bool IsValid(params ILegalPerson[] persons)
        {

            if (!IsCourtAssigned())
                return false;

            var nameTitles = persons.GetTitleNamePairs();
            var authPersons = persons.Where(p => IsToDeliverAuthorized(p)).ToList();
            if (!authPersons.Any())
            {
                AddReasonEntry($"{nameof(IsToDeliverAuthorized)} for all {nameTitles} is false");
                return false;
            }

            var deliveredTo = authPersons.Select(c => GetDeliveredTo(c)).ToList();

            if (!deliveredTo.Any())
            {
                nameTitles = authPersons.GetTitleNamePairs();
                AddReasonEntry($"{nameTitles}, {nameof(GetDeliveredTo)} returned no one ");
                return false;
            }

            if (deliveredTo.All(df => !IsToReceiveAuthorized(df)))
            {
                nameTitles = deliveredTo.GetTitleNamePairs();
                AddReasonEntry($"{nameof(IsToReceiveAuthorized)} for all {nameTitles} is false");
                return false;
            }

            return true;
        }
    }
}
