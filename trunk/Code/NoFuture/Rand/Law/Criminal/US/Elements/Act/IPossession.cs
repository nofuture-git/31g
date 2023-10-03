using System;

namespace NoFuture.Law.Criminal.US.Elements.Act
{
    public interface IPossession : IActusReus
    {
        Predicate<ILegalPerson> IsKnowinglyProcured { get; set; }

        Predicate<ILegalPerson> IsKnowinglyReceived { get; set; }
    }
}
