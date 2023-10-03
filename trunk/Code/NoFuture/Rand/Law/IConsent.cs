using System;

namespace NoFuture.Law
{
    /// <summary>
    /// A form of agreement that is distinctly the act of the will 
    /// </summary>
    public interface IConsent : IAssent
    {
        Predicate<ILegalPerson> IsCapableThereof { get; set; }
    }
}