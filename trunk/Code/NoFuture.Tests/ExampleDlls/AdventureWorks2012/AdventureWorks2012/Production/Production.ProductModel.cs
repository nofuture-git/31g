using System;
using System.Collections.Generic;

namespace AdventureWorks.Production
{
    [Serializable]
    public class ProductModel
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(ProductModel other)
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
            if (obj.GetType() != typeof (ProductModel)) return false;
            return this.Equals((ProductModel) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string Name { get; set; }

       public virtual string CatalogDescription { get; set; }

       public virtual string Instructions { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Production.Product> Products { get; set; }

        public virtual IList<AdventureWorks.Production.ProductModelIllustration> ProductModelIllustrations { get; set; }

        public virtual IList<AdventureWorks.Production.ProductModelProductDescriptionCulture> ProductModelProductDescriptionCultures { get; set; }

       #endregion

    }//end ProductModel
}//end AdventureWorks.Production


