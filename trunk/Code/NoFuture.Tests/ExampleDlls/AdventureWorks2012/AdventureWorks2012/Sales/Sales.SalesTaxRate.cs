using System;
using System.Collections.Generic;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class SalesTaxRate
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(SalesTaxRate other)
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
            if (obj.GetType() != typeof (SalesTaxRate)) return false;
            return this.Equals((SalesTaxRate) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual byte TaxType { get; set; }

       public virtual decimal TaxRate { get; set; }

       public virtual string Name { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Person.StateProvince StateProvinceByStateProvinceID { get; set; }

       #endregion

       #region Lists

       #endregion


    }//end SalesTaxRate
}//end AdventureWorks.Sales


