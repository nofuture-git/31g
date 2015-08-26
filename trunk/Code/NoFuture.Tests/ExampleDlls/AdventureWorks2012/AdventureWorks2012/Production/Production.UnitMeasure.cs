using System;
using System.Collections.Generic;

namespace AdventureWorks.Production
{
    [Serializable]
    public class UnitMeasure
    {
        #region Id
        public virtual string Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(UnitMeasure other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
            if (System.Object.ReferenceEquals(this, other)) return true;
			if (System.String.IsNullOrEmpty(Id) || System.String.IsNullOrEmpty(other.Id)) return false;			
			return other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UnitMeasure)) return false;
            return this.Equals((UnitMeasure) obj);
        }
        
        public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return System.String.IsNullOrEmpty(Id) ? newRandom.Next() : Id.GetHashCode();			
		}

        #endregion

        #region ValueTypes

       public virtual string Name { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Production.BillOfMaterials> BillOfMaterials { get; set; }

        public virtual IList<AdventureWorks.Production.Product> Products00 { get; set; }

        public virtual IList<AdventureWorks.Production.Product> Products01 { get; set; }

        public virtual IList<AdventureWorks.Purchasing.ProductVendor> ProductVendors { get; set; }

       #endregion


    }//end UnitMeasure
}//end AdventureWorks.Production


