using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Civil.US.ServiceOfProcess
{
    public abstract class DeliveryBase : ProcessServiceBase
    {
        /// <summary>
        /// Defines who delivered to who.
        /// </summary>
        /// <remarks>
        /// Simplest example being a U.S. marshal delivering directly to defendant.
        /// Others are delivery to appropriate 3rd party at defendant&apos;s home,
        /// defendant&apos;s attorney, etc.
        /// </remarks>
        public Func<ILegalPerson, ILegalPerson> GetDeliveredTo { get; set; } = lp => null;

        /// <summary>
        /// Directs who is authorized to receive a summons on behalf of a defendant
        /// </summary>
        public Predicate<ILegalPerson> IsToReceiveAuthorized { get; set; } = lp => lp is IDefendant;

        protected virtual bool IsDeliveredToAuthorizedPerson(IList<ILegalPerson> persons)
        {
            var deliveredTo = persons.Select(c => GetDeliveredTo(c)).ToList();

            var nameTitles = persons.GetTitleNamePairs();

            if (!deliveredTo.Any())
            {
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
