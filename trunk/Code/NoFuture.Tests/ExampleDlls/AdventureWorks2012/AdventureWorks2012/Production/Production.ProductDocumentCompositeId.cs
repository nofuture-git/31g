using System;

namespace AdventureWorks.Production
{
    [Serializable]
    public class ProductDocumentCompositeId
    {

        public virtual AdventureWorks.Production.Document Document { get; set; }

        public virtual AdventureWorks.Production.Product Product { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (ProductDocumentCompositeId)obj;

            if(compareTo.Document != this.Document){return false;}

            if(compareTo.Product != this.Product){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (Document != null ? Document.GetHashCode() : 1);

            sumValue += (Product != null ? Product.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end ProductDocumentCompositeId
}//end AdventureWorks.Production
