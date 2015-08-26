using System;
using System.Collections.Generic;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class SalesPerson
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(SalesPerson other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
            if (System.Object.ReferenceEquals(this, other)) return true;
			if (Id == 0 || other.Id == 0) return false;			
			return other.Id == Id;
        }

        public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return Id == 0 ? newRandom.Next() : Id.GetHashCode();			
		}


        public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SalesPerson)) return false;
            return this.Equals((SalesPerson) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual decimal? SalesQuota { get; set; }

       public virtual decimal Bonus { get; set; }

       public virtual decimal CommissionPct { get; set; }

       public virtual decimal SalesYTD { get; set; }

       public virtual decimal SalesLastYear { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.HumanResources.Employee EmployeeByBusinessEntityID { get; set; }

        public virtual AdventureWorks.Sales.SalesTerritory SalesTerritoryByTerritoryID { get; set; }

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Sales.SalesOrderHeader> SalesOrderHeaders { get; set; }

        public virtual IList<AdventureWorks.Sales.SalesPersonQuotaHistory> SalesPersonQuotaHistories { get; set; }

        public virtual IList<AdventureWorks.Sales.SalesTerritoryHistory> SalesTerritoryHistories { get; set; }

        public virtual IList<AdventureWorks.Sales.Store> Stores { get; set; }

       #endregion


    }//end SalesPerson
}//end AdventureWorks.Sales


