using System;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class SalesTerritoryHistoryCompositeId
    {

        public virtual AdventureWorks.Sales.SalesPerson SalesPerson { get; set; }

        public virtual AdventureWorks.Sales.SalesTerritory SalesTerritory { get; set; }

        public virtual System.DateTime StartDate { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (SalesTerritoryHistoryCompositeId)obj;

            if(compareTo.SalesPerson != this.SalesPerson){return false;}

            if(compareTo.SalesTerritory != this.SalesTerritory){return false;}

            if(compareTo.StartDate != this.StartDate){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (SalesPerson != null ? SalesPerson.GetHashCode() : 1);

            sumValue += (SalesTerritory != null ? SalesTerritory.GetHashCode() : 1);

            sumValue += (StartDate != null ? StartDate.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end SalesTerritoryHistoryCompositeId
}//end AdventureWorks.Sales
