using System;

namespace NoFuture.Rand.Law
{
    /// <summary>
    /// To grant permission for some action often with reluctance
    /// </summary>
    public interface IConsent : IAssent
    {
        Predicate<ILegalPerson> IsCapableThereof { get; set; }
    }
}