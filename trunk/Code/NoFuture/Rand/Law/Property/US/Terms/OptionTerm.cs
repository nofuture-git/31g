namespace NoFuture.Law.Property.US.Terms
{
    /// <summary>
    /// unilateral contract which gives the option holder the right to purchase under the terms and conditions of the option agreement.
    /// </summary>
    public class OptionTerm : RightOfFirstRefusalTerm
    {
        protected override string CategoryName => "option to purchase";

        public override bool IsCompelOwnerToSell => true;
    }
}
