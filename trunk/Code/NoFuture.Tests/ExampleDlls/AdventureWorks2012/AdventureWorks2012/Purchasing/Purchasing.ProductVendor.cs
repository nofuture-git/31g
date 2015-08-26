using System;
using System.Collections.Generic;

namespace AdventureWorks.Purchasing
{
    [Serializable]
    public class ProductVendor
    {
        #region Id
        public virtual AdventureWorks.Purchasing.ProductVendorCompositeId Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(ProductVendor other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
			return System.Object.ReferenceEquals(this, other) || Id.Equals(other.Id);
        }
		
		public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
			return obj.GetType() == typeof (ProductVendor) && Equals((ProductVendor) obj);
        }
		
		public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return Id == null ? newRandom.Next() : Id.GetHashCode();
		}


        #endregion

        #region ValueTypes

       public virtual int AverageLeadTime { get; set; }

       public virtual decimal StandardPrice { get; set; }

       public virtual decimal? LastReceiptCost { get; set; }

       public virtual System.DateTime? LastReceiptDate { get; set; }

       public virtual int MinOrderQty { get; set; }

       public virtual int MaxOrderQty { get; set; }

       public virtual int? OnOrderQty { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Production.UnitMeasure UnitMeasureByUnitMeasureCode { get; set; }

       #endregion

       #region Lists

       #endregion


    }//end ProductVendor
}//end AdventureWorks.Purchasing


