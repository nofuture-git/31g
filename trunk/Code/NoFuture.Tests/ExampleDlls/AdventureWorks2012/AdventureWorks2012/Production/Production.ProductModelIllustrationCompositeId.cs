using System;

namespace AdventureWorks.Production
{
    [Serializable]
    public class ProductModelIllustrationCompositeId
    {

        public virtual AdventureWorks.Production.Illustration Illustration { get; set; }

        public virtual AdventureWorks.Production.ProductModel ProductModel { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (ProductModelIllustrationCompositeId)obj;

            if(compareTo.Illustration != this.Illustration){return false;}

            if(compareTo.ProductModel != this.ProductModel){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (Illustration != null ? Illustration.GetHashCode() : 1);

            sumValue += (ProductModel != null ? ProductModel.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end ProductModelIllustrationCompositeId
}//end AdventureWorks.Production
