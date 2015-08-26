using System;
using System.Collections.Generic;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class CurrencyRate
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(CurrencyRate other)
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
            if (obj.GetType() != typeof (CurrencyRate)) return false;
            return this.Equals((CurrencyRate) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual System.DateTime CurrencyRateDate { get; set; }

       public virtual decimal AverageRate { get; set; }

       public virtual decimal EndOfDayRate { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Sales.Currency CurrencyByFromCurrencyCode { get; set; }

        public virtual AdventureWorks.Sales.Currency CurrencyByToCurrencyCode { get; set; }

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Sales.SalesOrderHeader> SalesOrderHeaders { get; set; }

       #endregion


    }//end CurrencyRate
}//end AdventureWorks.Sales


