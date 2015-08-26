using System;
using System.Collections.Generic;

namespace AdventureWorks.Purchasing
{
    [Serializable]
    public class PurchaseOrderHeader
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(PurchaseOrderHeader other)
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
            if (obj.GetType() != typeof (PurchaseOrderHeader)) return false;
            return this.Equals((PurchaseOrderHeader) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual byte RevisionNumber { get; set; }

       public virtual byte Status { get; set; }

       public virtual System.DateTime OrderDate { get; set; }

       public virtual System.DateTime? ShipDate { get; set; }

       public virtual decimal SubTotal { get; set; }

       public virtual decimal TaxAmt { get; set; }

       public virtual decimal Freight { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.HumanResources.Employee EmployeeByEmployeeID { get; set; }

        public virtual AdventureWorks.Purchasing.ShipMethod ShipMethodByShipMethodID { get; set; }

        public virtual AdventureWorks.Purchasing.Vendor VendorByVendorID { get; set; }

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Purchasing.PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

       #endregion


    }//end PurchaseOrderHeader
}//end AdventureWorks.Purchasing


