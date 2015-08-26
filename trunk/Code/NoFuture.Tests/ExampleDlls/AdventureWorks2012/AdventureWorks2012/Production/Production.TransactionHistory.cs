using System;
using System.Collections.Generic;

namespace AdventureWorks.Production
{
    [Serializable]
    public class TransactionHistory
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(TransactionHistory other)
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
            if (obj.GetType() != typeof (TransactionHistory)) return false;
            return this.Equals((TransactionHistory) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual int ReferenceOrderID { get; set; }

       public virtual int ReferenceOrderLineID { get; set; }

       public virtual System.DateTime TransactionDate { get; set; }

       public virtual string TransactionType { get; set; }

       public virtual int Quantity { get; set; }

       public virtual decimal ActualCost { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Production.Product ProductByProductID { get; set; }

       #endregion

       #region Lists

       #endregion


    }//end TransactionHistory
}//end AdventureWorks.Production


