using System;

namespace AdventureWorks.Purchasing
{
    [Serializable]
    public class PurchaseOrderDetailCompositeId
    {

        public virtual AdventureWorks.Purchasing.PurchaseOrderHeader PurchaseOrderHeader { get; set; }

        public virtual int PurchaseOrderDetailID { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (PurchaseOrderDetailCompositeId)obj;

            if(compareTo.PurchaseOrderHeader != this.PurchaseOrderHeader){return false;}

            if(compareTo.PurchaseOrderDetailID != this.PurchaseOrderDetailID){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (PurchaseOrderHeader != null ? PurchaseOrderHeader.GetHashCode() : 1);

            sumValue += (PurchaseOrderDetailID != null ? PurchaseOrderDetailID.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end PurchaseOrderDetailCompositeId
}//end AdventureWorks.Purchasing
