using System;
using System.Collections.Generic;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class Currency
    {
        #region Id
        public virtual string Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(Currency other)
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
            if (obj.GetType() != typeof (Currency)) return false;
            return this.Equals((Currency) obj);
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

        public virtual IList<AdventureWorks.Sales.CountryRegionCurrency> CountryRegionCurrencies { get; set; }

        public virtual IList<AdventureWorks.Sales.CurrencyRate> CurrencyRates00 { get; set; }

        public virtual IList<AdventureWorks.Sales.CurrencyRate> CurrencyRates01 { get; set; }

       #endregion


    }//end Currency
}//end AdventureWorks.Sales


