using System;
using System.Collections.Generic;

namespace AdventureWorks.Person
{
    [Serializable]
    public class StateProvince
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(StateProvince other)
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
            if (obj.GetType() != typeof (StateProvince)) return false;
            return this.Equals((StateProvince) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string StateProvinceCode { get; set; }

       public virtual bool IsOnlyStateProvinceFlag { get; set; }

       public virtual string Name { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Person.CountryRegion CountryRegionByCountryRegionCode { get; set; }

        public virtual AdventureWorks.Sales.SalesTerritory SalesTerritoryByTerritoryID { get; set; }

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Person.Address> Address { get; set; }

        public virtual IList<AdventureWorks.Sales.SalesTaxRate> SalesTaxRates { get; set; }

       #endregion


    }//end StateProvince
}//end AdventureWorks.Person


