namespace NoFuture.Rand.Law.US.Contracts.Ucc
{
    /// <inheritdoc />
    /// <summary>
    /// Goods which are in-the-earth see Section 2-107(1)
    /// </summary>
    public abstract class GoodsInTerra : Goods
    {
        public virtual bool IsSeveredBySeller { get; set; }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!base.IsValid(offeror, offeree))
                return false;
            if (!IsSeveredBySeller)
            {
                AddAuditEntry("sale of minerals or the like (including oil " +
                              "and gas) [...] are to be severed by the seller");
                return false;
            }
            return true;
        }
    }
}