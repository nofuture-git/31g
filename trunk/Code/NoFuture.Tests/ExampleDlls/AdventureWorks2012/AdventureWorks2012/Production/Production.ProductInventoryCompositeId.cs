using System;

namespace AdventureWorks.Production
{
    [Serializable]
    public class ProductInventoryCompositeId
    {

        public virtual AdventureWorks.Production.Location Location { get; set; }

        public virtual AdventureWorks.Production.Product Product { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (ProductInventoryCompositeId)obj;

            if(compareTo.Location != this.Location){return false;}

            if(compareTo.Product != this.Product){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (Location != null ? Location.GetHashCode() : 1);

            sumValue += (Product != null ? Product.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end ProductInventoryCompositeId
}//end AdventureWorks.Production
