using NoFuture.Rand.Law.Procedure.Civil.US.Pleadings;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// A response to a <see cref="Summons"/> in which defendant
    /// objects to court&apos;s personal jurisdiction over them 
    /// </summary>
    public class SpecialAppearance : PersonalJurisdiction
    {
        public SpecialAppearance(ICourt name) : base(name)
        {
        }
    }
}
