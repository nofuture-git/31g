using System;
using System.Collections.Generic;

namespace AdventureWorks.Person
{
    [Serializable]
    public class BusinessEntity
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(BusinessEntity other)
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
            if (obj.GetType() != typeof (BusinessEntity)) return false;
            return this.Equals((BusinessEntity) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Person.BusinessEntityAddress> BusinessEntityAddress { get; set; }

        public virtual IList<AdventureWorks.Person.BusinessEntityContact> BusinessEntityContacts { get; set; }

        public virtual IList<AdventureWorks.Person.Person> People { get; set; }

        public virtual IList<AdventureWorks.Purchasing.Vendor> Vendors { get; set; }

        public virtual IList<AdventureWorks.Sales.Store> Stores { get; set; }

       #endregion


    }//end BusinessEntity
}//end AdventureWorks.Person


