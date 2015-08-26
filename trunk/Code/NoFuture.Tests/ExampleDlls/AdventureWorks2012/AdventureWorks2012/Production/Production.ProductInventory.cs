using System;
using System.Collections.Generic;

namespace AdventureWorks.Production
{
    [Serializable]
    public class ProductInventory
    {
        #region Id
        public virtual AdventureWorks.Production.ProductInventoryCompositeId Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(ProductInventory other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
			return System.Object.ReferenceEquals(this, other) || Id.Equals(other.Id);
        }
		
		public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
			return obj.GetType() == typeof (ProductInventory) && Equals((ProductInventory) obj);
        }
		
		public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return Id == null ? newRandom.Next() : Id.GetHashCode();
		}


        #endregion

        #region ValueTypes

       public virtual string Shelf { get; set; }

       public virtual byte Bin { get; set; }

       public virtual short Quantity { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

       #endregion


    }//end ProductInventory
}//end AdventureWorks.Production


