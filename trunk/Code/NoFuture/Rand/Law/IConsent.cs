using System;

namespace NoFuture.Rand.Law.Criminal.US.Elements
{
    public interface IConsent
    {
        Predicate<ILegalPerson> IsFirmDenial { get; set; }
    }
}