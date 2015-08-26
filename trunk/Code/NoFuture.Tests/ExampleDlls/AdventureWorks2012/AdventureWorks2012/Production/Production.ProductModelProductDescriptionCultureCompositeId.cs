using System;

namespace AdventureWorks.Production
{
    [Serializable]
    public class ProductModelProductDescriptionCultureCompositeId
    {

        public virtual AdventureWorks.Production.Culture Culture { get; set; }

        public virtual AdventureWorks.Production.ProductDescription ProductDescription { get; set; }

        public virtual AdventureWorks.Production.ProductModel ProductModel { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (ProductModelProductDescriptionCultureCompositeId)obj;

            if(compareTo.Culture != this.Culture){return false;}

            if(compareTo.ProductDescription != this.ProductDescription){return false;}

            if(compareTo.ProductModel != this.ProductModel){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (Culture != null ? Culture.GetHashCode() : 1);

            sumValue += (ProductDescription != null ? ProductDescription.GetHashCode() : 1);

            sumValue += (ProductModel != null ? ProductModel.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end ProductModelProductDescriptionCultureCompositeId
}//end AdventureWorks.Production
