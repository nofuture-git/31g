using System;

namespace AdventureWorks.Production
{
    [Serializable]
    public class ProductListPriceHistoryCompositeId
    {

        public virtual AdventureWorks.Production.Product Product { get; set; }

        public virtual System.DateTime StartDate { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (ProductListPriceHistoryCompositeId)obj;

            if(compareTo.Product != this.Product){return false;}

            if(compareTo.StartDate != this.StartDate){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (Product != null ? Product.GetHashCode() : 1);

            sumValue += (StartDate != null ? StartDate.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end ProductListPriceHistoryCompositeId
}//end AdventureWorks.Production
