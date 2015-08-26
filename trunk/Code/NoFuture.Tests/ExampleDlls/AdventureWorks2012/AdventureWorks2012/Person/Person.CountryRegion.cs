using System;
using System.Collections.Generic;

namespace AdventureWorks.Person
{
    [Serializable]
    public class CountryRegion
    {
        #region Id
        public virtual string Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(CountryRegion other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
            if (System.Object.ReferenceEquals(this, other)) return true;
			if (System.String.IsNullOrEmpty(Id) || System.String.IsNullOrEmpty(other.Id)) return false;			
			return other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (CountryRegion)) return false;
            return this.Equals((CountryRegion) obj);
        }
        
        public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return System.String.IsNullOrEmpty(Id) ? newRandom.Next() : Id.GetHashCode();			
		}

        #endregion

        #region ValueTypes

       public virtual string Name { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Person.StateProvince> StateProvinces { get; set; }

        public virtual IList<AdventureWorks.Sales.CountryRegionCurrency> CountryRegionCurrencies { get; set; }

        public virtual IList<AdventureWorks.Sales.SalesTerritory> SalesTerritories { get; set; }

       #endregion


    }//end CountryRegion
}//end AdventureWorks.Person


