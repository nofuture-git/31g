using System;

namespace AdventureWorks.Production
{
    [Serializable]
    public class ProductProductPhotoCompositeId
    {

        public virtual AdventureWorks.Production.Product Product { get; set; }

        public virtual AdventureWorks.Production.ProductPhoto ProductPhoto { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (ProductProductPhotoCompositeId)obj;

            if(compareTo.Product != this.Product){return false;}

            if(compareTo.ProductPhoto != this.ProductPhoto){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (Product != null ? Product.GetHashCode() : 1);

            sumValue += (ProductPhoto != null ? ProductPhoto.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end ProductProductPhotoCompositeId
}//end AdventureWorks.Production
