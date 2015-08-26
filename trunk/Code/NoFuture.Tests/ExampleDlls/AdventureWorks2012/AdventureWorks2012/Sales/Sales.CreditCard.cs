using System;
using System.Collections.Generic;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class CreditCard
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(CreditCard other)
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
            if (obj.GetType() != typeof (CreditCard)) return false;
            return this.Equals((CreditCard) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string CardType { get; set; }

       public virtual string CardNumber { get; set; }

       public virtual byte ExpMonth { get; set; }

       public virtual short ExpYear { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Sales.PersonCreditCard> PersonCreditCards { get; set; }

        public virtual IList<AdventureWorks.Sales.SalesOrderHeader> SalesOrderHeaders { get; set; }

       #endregion


    }//end CreditCard
}//end AdventureWorks.Sales


