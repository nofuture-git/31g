using System;
using System.Collections.Generic;

namespace AdventureWorks.Production
{
    [Serializable]
    public class ProductSubcategory
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(ProductSubcategory other)
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
            if (obj.GetType() != typeof (ProductSubcategory)) return false;
            return this.Equals((ProductSubcategory) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string Name { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Production.ProductCategory ProductCategoryByProductCategoryID { get; set; }

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Production.Product> Products { get; set; }

       #endregion


    }//end ProductSubcategory
}//end AdventureWorks.Production


