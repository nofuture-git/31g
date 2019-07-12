using System.Linq;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Contract.US.Ucc
{
    /// <inheritdoc />
    /// <summary>
    /// Goods which are in-the-earth see Section 2-107(1)
    /// </summary>
    public abstract class GoodsInTerra : Goods
    {
        public virtual bool IsSeveredBySeller { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

            if (!base.IsValid(offeror, offeree))
                return false;
            if (!IsSeveredBySeller)
            {
                AddReasonEntry("sale of minerals or the like (including oil " +
                              "and gas) [...] are to be severed by the seller");
                return false;
            }
            return true;
        }
    }
}