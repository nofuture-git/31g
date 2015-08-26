using System;
using System.Collections.Generic;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class SalesOrderHeader
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(SalesOrderHeader other)
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
            if (obj.GetType() != typeof (SalesOrderHeader)) return false;
            return this.Equals((SalesOrderHeader) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual byte RevisionNumber { get; set; }

       public virtual System.DateTime OrderDate { get; set; }

       public virtual System.DateTime DueDate { get; set; }

       public virtual System.DateTime? ShipDate { get; set; }

       public virtual byte Status { get; set; }

       public virtual bool OnlineOrderFlag { get; set; }

       public virtual string PurchaseOrderNumber { get; set; }

       public virtual string AccountNumber { get; set; }

       public virtual string CreditCardApprovalCode { get; set; }

       public virtual decimal SubTotal { get; set; }

       public virtual decimal TaxAmt { get; set; }

       public virtual decimal Freight { get; set; }

       public virtual string Comment { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Person.Address AddressByBillToAddressID { get; set; }

        public virtual AdventureWorks.Person.Address AddressByShipToAddressID { get; set; }

        public virtual AdventureWorks.Sales.CreditCard CreditCardByCreditCardID { get; set; }

        public virtual AdventureWorks.Sales.CurrencyRate CurrencyRateByCurrencyRateID { get; set; }

        public virtual AdventureWorks.Sales.Customer CustomerByCustomerID { get; set; }

        public virtual AdventureWorks.Sales.SalesPerson SalesPersonBySalesPersonID { get; set; }

        public virtual AdventureWorks.Sales.SalesTerritory SalesTerritoryByTerritoryID { get; set; }

        public virtual AdventureWorks.Purchasing.ShipMethod ShipMethodByShipMethodID { get; set; }

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Sales.SalesOrderDetail> SalesOrderDetails { get; set; }

        public virtual IList<AdventureWorks.Sales.SalesOrderHeaderSalesReason> SalesOrderHeaderSalesReasons { get; set; }

       #endregion


    }//end SalesOrderHeader
}//end AdventureWorks.Sales


