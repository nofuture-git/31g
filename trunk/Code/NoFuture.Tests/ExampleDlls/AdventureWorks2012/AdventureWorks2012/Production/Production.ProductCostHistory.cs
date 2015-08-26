using System;
using System.Collections.Generic;

namespace AdventureWorks.Production
{
    [Serializable]
    public class ProductCostHistory
    {
        #region Id
        public virtual AdventureWorks.Production.ProductCostHistoryCompositeId Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(ProductCostHistory other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
			return System.Object.ReferenceEquals(this, other) || Id.Equals(other.Id);
        }
		
		public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
			return obj.GetType() == typeof (ProductCostHistory) && Equals((ProductCostHistory) obj);
        }
		
		public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return Id == null ? newRandom.Next() : Id.GetHashCode();
		}


        #endregion

        #region ValueTypes

       public virtual System.DateTime? EndDate { get; set; }

       public virtual decimal StandardCost { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

       #endregion


    }//end ProductCostHistory
}//end AdventureWorks.Production


