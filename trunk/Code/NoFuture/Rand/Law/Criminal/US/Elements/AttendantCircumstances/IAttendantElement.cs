namespace NoFuture.Law.Criminal.US.Elements.AttendantCircumstances
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
