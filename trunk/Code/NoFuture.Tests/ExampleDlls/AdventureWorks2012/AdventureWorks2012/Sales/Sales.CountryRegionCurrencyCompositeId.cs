using System;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class CountryRegionCurrencyCompositeId
    {

        public virtual AdventureWorks.Person.CountryRegion CountryRegion { get; set; }

        public virtual AdventureWorks.Sales.Currency Currency { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (CountryRegionCurrencyCompositeId)obj;

            if(compareTo.CountryRegion != this.CountryRegion){return false;}

            if(compareTo.Currency != this.Currency){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (CountryRegion != null ? CountryRegion.GetHashCode() : 1);

            sumValue += (Currency != null ? Currency.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end CountryRegionCurrencyCompositeId
}//end AdventureWorks.Sales
