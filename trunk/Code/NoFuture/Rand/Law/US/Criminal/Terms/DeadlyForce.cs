namespace NoFuture.Rand.Law.US.Criminal.Terms
{
    /// <inheritdoc />
    /// <summary>
    /// Any force which could potentially kill.
    /// This is appropriate in self-defense when the attacker threatens <see cref="Death"/> or <see cref="SeriousBodilyInjury"/>
    /// </summary>
    public class DeadlyForce : NondeadlyForce
    {
        protected override string CategoryName { get; } = "deadly force";
        public override int GetCategoryRank()
        {
            return base.GetCategoryRank() + 1;
        }
    }
}