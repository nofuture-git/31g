using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.US.Elements
{
    /// <summary>
    /// The facts surrounding an event
    /// </summary>
    public abstract class AttendantCircumstances: CriminalBase, IAttendantElement
    {
        public virtual bool IsValid(IMensRea intent, params ILegalPerson[] persons)
        {
            return false;
        }

        public virtual bool IsValid(IActusReus criminalAct, params ILegalPerson[] persons)
        {
            return false;
        }
    }
}
