using System;
using System.Collections.Generic;

namespace AdventureWorks.Purchasing
{
    [Serializable]
    public class ShipMethod
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(ShipMethod other)
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
            if (obj.GetType() != typeof (ShipMethod)) return false;
            return this.Equals((ShipMethod) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string Name { get; set; }

       public virtual decimal ShipBase { get; set; }

       public virtual decimal ShipRate { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Purchasing.PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }

        public virtual IList<AdventureWorks.Sales.SalesOrderHeader> SalesOrderHeaders { get; set; }

       #endregion


    }//end ShipMethod
}//end AdventureWorks.Purchasing


