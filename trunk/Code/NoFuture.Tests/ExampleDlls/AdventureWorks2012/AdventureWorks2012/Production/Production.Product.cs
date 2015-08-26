using System;
using System.Collections.Generic;

namespace AdventureWorks.Production
{
    [Serializable]
    public class Product
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(Product other)
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
            if (obj.GetType() != typeof (Product)) return false;
            return this.Equals((Product) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string Name { get; set; }

       public virtual string ProductNumber { get; set; }

       public virtual bool MakeFlag { get; set; }

       public virtual bool FinishedGoodsFlag { get; set; }

       public virtual string Color { get; set; }

       public virtual short SafetyStockLevel { get; set; }

       public virtual short ReorderPoint { get; set; }

       public virtual decimal StandardCost { get; set; }

       public virtual decimal ListPrice { get; set; }

       public virtual string Size { get; set; }

       public virtual decimal? Weight { get; set; }

       public virtual int DaysToManufacture { get; set; }

       public virtual string ProductLine { get; set; }

       public virtual string Class { get; set; }

       public virtual string Style { get; set; }

       public virtual System.DateTime SellStartDate { get; set; }

       public virtual System.DateTime? SellEndDate { get; set; }

       public virtual System.DateTime? DiscontinuedDate { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Production.ProductModel ProductModelByProductModelID { get; set; }

        public virtual AdventureWorks.Production.ProductSubcategory ProductSubcategoryByProductSubcategoryID { get; set; }

        public virtual AdventureWorks.Production.UnitMeasure UnitMeasureBySizeUnitMeasureCode { get; set; }

        public virtual AdventureWorks.Production.UnitMeasure UnitMeasureByWeightUnitMeasureCode { get; set; }

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Production.BillOfMaterials> BillOfMaterials00 { get; set; }

        public virtual IList<AdventureWorks.Production.BillOfMaterials> BillOfMaterials01 { get; set; }

        public virtual IList<AdventureWorks.Production.ProductCostHistory> ProductCostHistories { get; set; }

        public virtual IList<AdventureWorks.Production.ProductDocument> ProductDocuments { get; set; }

        public virtual IList<AdventureWorks.Production.ProductInventory> ProductInventories { get; set; }

        public virtual IList<AdventureWorks.Production.ProductListPriceHistory> ProductListPriceHistories { get; set; }

        public virtual IList<AdventureWorks.Production.ProductProductPhoto> ProductProductPhotoes { get; set; }

        public virtual IList<AdventureWorks.Production.ProductReview> ProductReviews { get; set; }

        public virtual IList<AdventureWorks.Production.TransactionHistory> TransactionHistories { get; set; }

        public virtual IList<AdventureWorks.Production.WorkOrder> WorkOrders { get; set; }

        public virtual IList<AdventureWorks.Purchasing.ProductVendor> ProductVendors { get; set; }

        public virtual IList<AdventureWorks.Purchasing.PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public virtual IList<AdventureWorks.Sales.ShoppingCartItem> ShoppingCartItems { get; set; }

        public virtual IList<AdventureWorks.Sales.SpecialOfferProduct> SpecialOfferProducts { get; set; }

       #endregion

    }//end Product
}//end AdventureWorks.Production


