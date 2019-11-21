using System;

namespace NoFuture.Rand.Law.Procedure.Civil.US.ServiceOfProcess
{
    public interface IProcessService : ILegalConcept
    {
        ICourt Court { get; set; }

        Func<ILegalPerson, DateTime?> GetToDateOfService { get; set; }

        DateTime? CurrentTime { get; set; }
    }
}