namespace NoFuture.Rand.Law.US.Criminal.Terms
{
    /// <inheritdoc />
    /// <summary>
    /// Any force which could potentially kill.
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