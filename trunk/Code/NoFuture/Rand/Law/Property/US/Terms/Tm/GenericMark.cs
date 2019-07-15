using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.Terms.Tm
{
    /// <summary>
    /// Refer to class or groups of indefinite articles.
    /// </summary>
    /// <remarks>
    /// Represent the furthest you can get from the definitive in that
    /// identity is only of a group (by definition).  In other words, the
    /// word is only used to refer to a class or group.
    /// </remarks>
    [Eg("book", "cat", "boy")]
    public class GenericMark : StrengthOfMark
    {
        protected override string CategoryName => "generic mark";

        public override int GetRank()
        {
            return 0;
        }
    }
}
