using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.US.Elements
{
    /// <summary>
    /// Any element other than <see cref="IMensRea"/> and <see cref="IActusReus"/>
    /// </summary>
    public interface IAttendantElement : IElement
    {
        bool IsValid(IMensRea intent, params ILegalPerson[] persons);
        bool IsValid(IActusReus criminalAct, params ILegalPerson[] persons);
    }
}
