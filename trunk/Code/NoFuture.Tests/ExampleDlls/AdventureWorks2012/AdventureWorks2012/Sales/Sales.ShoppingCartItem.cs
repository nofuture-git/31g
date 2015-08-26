using System;
using System.Collections.Generic;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class ShoppingCartItem
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(ShoppingCartItem other)
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
            if (obj.GetType() != typeof (ShoppingCartItem)) return false;
            return this.Equals((ShoppingCartItem) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string ShoppingCartID { get; set; }

       public virtual int Quantity { get; set; }

       public virtual System.DateTime DateCreated { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Production.Product ProductByProductID { get; set; }

       #endregion

       #region Lists

       #endregion


    }//end ShoppingCartItem
}//end AdventureWorks.Sales


