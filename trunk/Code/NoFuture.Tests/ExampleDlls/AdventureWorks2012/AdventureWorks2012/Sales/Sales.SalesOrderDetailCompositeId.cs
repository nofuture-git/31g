using System;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class SalesOrderDetailCompositeId
    {

        public virtual AdventureWorks.Sales.SalesOrderHeader SalesOrderHeader { get; set; }

        public virtual int SalesOrderDetailID { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (SalesOrderDetailCompositeId)obj;

            if(compareTo.SalesOrderHeader != this.SalesOrderHeader){return false;}

            if(compareTo.SalesOrderDetailID != this.SalesOrderDetailID){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (SalesOrderHeader != null ? SalesOrderHeader.GetHashCode() : 1);

            sumValue += (SalesOrderDetailID != null ? SalesOrderDetailID.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end SalesOrderDetailCompositeId
}//end AdventureWorks.Sales
