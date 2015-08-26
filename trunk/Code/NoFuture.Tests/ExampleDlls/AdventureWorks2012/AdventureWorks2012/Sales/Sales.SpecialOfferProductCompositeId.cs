using System;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class SpecialOfferProductCompositeId
    {

        public virtual AdventureWorks.Production.Product Product { get; set; }

        public virtual AdventureWorks.Sales.SpecialOffer SpecialOffer { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (SpecialOfferProductCompositeId)obj;

            if(compareTo.Product != this.Product){return false;}

            if(compareTo.SpecialOffer != this.SpecialOffer){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (Product != null ? Product.GetHashCode() : 1);

            sumValue += (SpecialOffer != null ? SpecialOffer.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end SpecialOfferProductCompositeId
}//end AdventureWorks.Sales
