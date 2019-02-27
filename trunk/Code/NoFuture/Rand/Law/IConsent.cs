using System;

namespace NoFuture.Rand.Law
{
    public interface IConsent
    {
        Predicate<ILegalPerson> IsFirmDenial { get; set; }
    }
}