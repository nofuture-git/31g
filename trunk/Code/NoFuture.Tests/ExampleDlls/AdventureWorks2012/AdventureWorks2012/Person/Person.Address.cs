﻿using System;
using System.Collections.Generic;

namespace AdventureWorks.Person
{
    [Serializable]
    public class Address
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(Address other)
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
            if (obj.GetType() != typeof (Address)) return false;
            return this.Equals((Address) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string AddressLine1 { get; set; }

       public virtual string AddressLine2 { get; set; }

       public virtual string City { get; set; }

       public virtual string PostalCode { get; set; }

       public virtual NoFuture.Hbm.Sid.BinaryId SpatialLocation { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Person.StateProvince StateProvinceByStateProvinceID { get; set; }

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Person.BusinessEntityAddress> BusinessEntityAddress { get; set; }

        public virtual IList<AdventureWorks.Sales.SalesOrderHeader> SalesOrderHeaders00 { get; set; }

        public virtual IList<AdventureWorks.Sales.SalesOrderHeader> SalesOrderHeaders01 { get; set; }

       #endregion


    }//end Address
}//end AdventureWorks.Person


