using System;
using System.Collections.Generic;

namespace AdventureWorks.Purchasing
{
    [Serializable]
    public class PurchaseOrderDetail
    {
        #region Id
        public virtual AdventureWorks.Purchasing.PurchaseOrderDetailCompositeId Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(PurchaseOrderDetail other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
			return System.Object.ReferenceEquals(this, other) || Id.Equals(other.Id);
        }
		
		public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
			return obj.GetType() == typeof (PurchaseOrderDetail) && Equals((PurchaseOrderDetail) obj);
        }
		
		public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return Id == null ? newRandom.Next() : Id.GetHashCode();
		}


        #endregion

        #region ValueTypes

       public virtual System.DateTime DueDate { get; set; }

       public virtual short OrderQty { get; set; }

       public virtual decimal UnitPrice { get; set; }

       public virtual decimal ReceivedQty { get; set; }

       public virtual decimal RejectedQty { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Production.Product ProductByProductID { get; set; }

       #endregion

       #region Lists

       #endregion


    }//end PurchaseOrderDetail
}//end AdventureWorks.Purchasing


