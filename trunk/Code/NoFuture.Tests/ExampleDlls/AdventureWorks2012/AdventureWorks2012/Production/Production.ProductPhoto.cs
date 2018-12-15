using System;
using System.Collections.Generic;

namespace AdventureWorks.Production
{
    [Serializable]
    public class ProductPhoto
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(ProductPhoto other)
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
            if (obj.GetType() != typeof (ProductPhoto)) return false;
            return this.Equals((ProductPhoto) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string ThumbNailPhoto { get; set; }

       public virtual string ThumbnailPhotoFileName { get; set; }

       public virtual string LargePhoto { get; set; }

       public virtual string LargePhotoFileName { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Production.ProductProductPhoto> ProductProductPhotoes { get; set; }

       #endregion


    }//end ProductPhoto
}//end AdventureWorks.Production


