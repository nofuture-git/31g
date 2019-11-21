using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.ServiceOfProcess
{
    public abstract class ProcessServiceBase : CivilProcedureBase, IProcessService
    {
        /// <summary>
        /// Some states use this date, instead of complaint&apos;s date-of-filing, to determine statute of limitations
        /// </summary>
        public Func<ILegalPerson, DateTime?> GetToDateOfService { get; set; } = lp => null;

        protected internal virtual bool IsValidDateOfService(IList<ILegalPerson> persons)
        {
            if (persons == null || !persons.Any())
                return false;

            var defendant = this.Defendant(persons);

            if (defendant == null)
                return false;

            var defendantTitle = defendant.GetLegalPersonTypeName();

            var dtOfService = GetToDateOfService(defendant);

            if (dtOfService == null)
            {
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(GetToDateOfService)} returned nothing");
                return false;
            }

            if (dtOfService.Value < new DateTime(1776, 7, 4)) //close enough
            {
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(GetToDateOfService)} " +
                               $"returned invalid value of {dtOfService.Value}");
                return false;
            }

            return true;
        }
    }
}
