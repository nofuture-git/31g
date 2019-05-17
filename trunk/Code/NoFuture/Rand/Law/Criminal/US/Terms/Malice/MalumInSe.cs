using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Criminal.US.Terms.Malice
{
    [Aka("evil in itself")]
    [Note("is inherently wrong independent of any regulations")]
    public class MalumInSe : MalumProhibitum
    {
        protected override string CategoryName => "malum in se";

        public override int GetCategoryRank()
        {
            return base.GetCategoryRank() + 1;
        }
    }
}
