using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Procedure.Civil.US.ServiceOfProcess
{
    public abstract class ProcessServiceBase : CivilProcedureBase, IProcessService
    {
        /// <summary>
        /// Some states use this date, instead of complaint&apos;s date-of-filing, to determine statute of limitations
        /// </summary>
        public Func<ILegalPerson, DateTime?> GetToDateOfService { get; set; } = lp => null;

        public DateTime? CurrentTime { get; set; }

        public static bool IsDateOfServiceValid(IProcessService serviceOfProcess, ILegalPerson person, out DateTime? dtOfService)
        {
            dtOfService = null;
            if (person == null)
                return false;

            var defendantTitle = person.GetLegalPersonTypeName();

            dtOfService = serviceOfProcess.GetToDateOfService(person);

            if (dtOfService == null)
            {
                serviceOfProcess.AddReasonEntry($"{defendantTitle} {person.Name}, {nameof(GetToDateOfService)} returned nothing");
                return false;
            }

            if (dtOfService.Value < new DateTime(1776, 7, 4)) //close enough
            {
                serviceOfProcess.AddReasonEntry($"{defendantTitle} {person.Name}, {nameof(GetToDateOfService)} " +
                               $"returned invalid value of {dtOfService.Value}");
                return false;
            }

            return true;
        }
    }
}
