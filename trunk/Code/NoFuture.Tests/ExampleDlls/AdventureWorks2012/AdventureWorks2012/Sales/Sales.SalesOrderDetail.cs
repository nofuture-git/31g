using System;
using System.Collections.Generic;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class SalesOrderDetail
    {
        #region Id
        public virtual AdventureWorks.Sales.SalesOrderDetailCompositeId Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(SalesOrderDetail other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
			return System.Object.ReferenceEquals(this, other) || Id.Equals(other.Id);
        }
		
		public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
			return obj.GetType() == typeof (SalesOrderDetail) && Equals((SalesOrderDetail) obj);
        }
		
		public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return Id == null ? newRandom.Next() : Id.GetHashCode();
		}


        #endregion

        #region ValueTypes

       public virtual string CarrierTrackingNumber { get; set; }

       public virtual short OrderQty { get; set; }

       public virtual decimal UnitPrice { get; set; }

       public virtual decimal UnitPriceDiscount { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Sales.SpecialOfferProduct SpecialOfferProductByProductIDAndSpecialOfferID { get; set; }

       #endregion

       #region Lists

       #endregion


    }//end SalesOrderDetail
}//end AdventureWorks.Sales


