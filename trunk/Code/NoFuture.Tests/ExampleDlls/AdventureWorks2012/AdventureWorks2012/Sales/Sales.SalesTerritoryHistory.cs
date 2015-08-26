using System;
using System.Collections.Generic;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class SalesTerritoryHistory
    {
        #region Id
        public virtual AdventureWorks.Sales.SalesTerritoryHistoryCompositeId Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(SalesTerritoryHistory other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
			return System.Object.ReferenceEquals(this, other) || Id.Equals(other.Id);
        }
		
		public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
			return obj.GetType() == typeof (SalesTerritoryHistory) && Equals((SalesTerritoryHistory) obj);
        }
		
		public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return Id == null ? newRandom.Next() : Id.GetHashCode();
		}


        #endregion

        #region ValueTypes

       public virtual System.DateTime? EndDate { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

       #endregion


    }//end SalesTerritoryHistory
}//end AdventureWorks.Sales


