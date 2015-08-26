using System;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class SalesPersonQuotaHistoryCompositeId
    {

        public virtual AdventureWorks.Sales.SalesPerson SalesPerson { get; set; }

        public virtual System.DateTime QuotaDate { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (SalesPersonQuotaHistoryCompositeId)obj;

            if(compareTo.SalesPerson != this.SalesPerson){return false;}

            if(compareTo.QuotaDate != this.QuotaDate){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (SalesPerson != null ? SalesPerson.GetHashCode() : 1);

            sumValue += (QuotaDate != null ? QuotaDate.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end SalesPersonQuotaHistoryCompositeId
}//end AdventureWorks.Sales
