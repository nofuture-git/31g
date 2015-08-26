using System;
using System.Collections.Generic;

namespace AdventureWorks.Sales
{
    [Serializable]
    public class SpecialOffer
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(SpecialOffer other)
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
            if (obj.GetType() != typeof (SpecialOffer)) return false;
            return this.Equals((SpecialOffer) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string Description { get; set; }

       public virtual decimal DiscountPct { get; set; }

       public virtual string Type { get; set; }

       public virtual string Category { get; set; }

       public virtual System.DateTime EndDate { get; set; }

       public virtual int MinQty { get; set; }

       public virtual int? MaxQty { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Sales.SpecialOfferProduct> SpecialOfferProducts { get; set; }

       #endregion


    }//end SpecialOffer
}//end AdventureWorks.Sales


