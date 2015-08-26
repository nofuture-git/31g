using System;

namespace AdventureWorks.Purchasing
{
    [Serializable]
    public class ProductVendorCompositeId
    {

        public virtual AdventureWorks.Production.Product Product { get; set; }

        public virtual AdventureWorks.Purchasing.Vendor Vendor { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (ProductVendorCompositeId)obj;

            if(compareTo.Product != this.Product){return false;}

            if(compareTo.Vendor != this.Vendor){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (Product != null ? Product.GetHashCode() : 1);

            sumValue += (Vendor != null ? Vendor.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end ProductVendorCompositeId
}//end AdventureWorks.Purchasing
