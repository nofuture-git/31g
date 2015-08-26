using System;
using System.Collections.Generic;

namespace AdventureWorks.Purchasing
{
    [Serializable]
    public class Vendor
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(Vendor other)
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
            if (obj.GetType() != typeof (Vendor)) return false;
            return this.Equals((Vendor) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string AccountNumber { get; set; }

       public virtual string Name { get; set; }

       public virtual byte CreditRating { get; set; }

       public virtual bool PreferredVendorStatus { get; set; }

       public virtual bool ActiveFlag { get; set; }

       public virtual string PurchasingWebServiceURL { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Person.BusinessEntity BusinessEntityByBusinessEntityID { get; set; }

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Purchasing.ProductVendor> ProductVendors { get; set; }

        public virtual IList<AdventureWorks.Purchasing.PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }

       #endregion


    }//end Vendor
}//end AdventureWorks.Purchasing


