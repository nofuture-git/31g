namespace NoFuture.Rand.Law.Criminal.US.Terms.Violence
{
    /// <inheritdoc />
    /// <summary>
    /// Any force which could potentially kill.
    /// This is appropriate in self-defense when the attacker threatens <see cref="Death"/> or <see cref="SeriousBodilyInjury"/>
    /// </summary>
    public class DeadlyForce : SeriousBodilyInjury
    {
        protected override string CategoryName { get; } = "deadly force";
    }
}