using NoFuture.Rand.Law.Criminal.US.Elements;

namespace NoFuture.Rand.Law.Criminal.AgainstGov.US
{
    /// <summary>
    /// A declaration by the President per  National Emergencies Act (NEA)
    /// </summary>
    public class NationalEmergency : AttendantCircumstanceBase
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
