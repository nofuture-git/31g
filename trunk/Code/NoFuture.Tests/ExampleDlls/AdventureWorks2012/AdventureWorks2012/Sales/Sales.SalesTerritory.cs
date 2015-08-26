using System;
using System.Collections.Generic;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class SalesTerritory
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(SalesTerritory other)
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
            if (obj.GetType() != typeof (SalesTerritory)) return false;
            return this.Equals((SalesTerritory) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string Name { get; set; }

       public virtual string Group { get; set; }

       public virtual decimal SalesYTD { get; set; }

       public virtual decimal SalesLastYear { get; set; }

       public virtual decimal CostYTD { get; set; }

       public virtual decimal CostLastYear { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Person.CountryRegion CountryRegionByCountryRegionCode { get; set; }

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Person.StateProvince> StateProvinces { get; set; }

        public virtual IList<AdventureWorks.Sales.Customer> Customers { get; set; }

        public virtual IList<AdventureWorks.Sales.SalesOrderHeader> SalesOrderHeaders { get; set; }

        public virtual IList<AdventureWorks.Sales.SalesPerson> SalesPersons { get; set; }

        public virtual IList<AdventureWorks.Sales.SalesTerritoryHistory> SalesTerritoryHistories { get; set; }

       #endregion


    }//end SalesTerritory
}//end AdventureWorks.Sales


