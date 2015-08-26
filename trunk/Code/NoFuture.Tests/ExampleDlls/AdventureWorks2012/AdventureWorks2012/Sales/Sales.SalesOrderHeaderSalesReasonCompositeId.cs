using System;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class SalesOrderHeaderSalesReasonCompositeId
    {

        public virtual AdventureWorks.Sales.SalesOrderHeader SalesOrderHeader { get; set; }

        public virtual AdventureWorks.Sales.SalesReason SalesReason { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (SalesOrderHeaderSalesReasonCompositeId)obj;

            if(compareTo.SalesOrderHeader != this.SalesOrderHeader){return false;}

            if(compareTo.SalesReason != this.SalesReason){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (SalesOrderHeader != null ? SalesOrderHeader.GetHashCode() : 1);

            sumValue += (SalesReason != null ? SalesReason.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end SalesOrderHeaderSalesReasonCompositeId
}//end AdventureWorks.Sales
