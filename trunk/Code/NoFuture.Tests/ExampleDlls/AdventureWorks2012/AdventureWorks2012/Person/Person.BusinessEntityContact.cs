using System;
using System.Collections.Generic;

namespace AdventureWorks.Person
{
    [Serializable]
    public class BusinessEntityContact
    {
        #region Id
        public virtual AdventureWorks.Person.BusinessEntityContactCompositeId Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(BusinessEntityContact other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
			return System.Object.ReferenceEquals(this, other) || Id.Equals(other.Id);
        }
		
		public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
			return obj.GetType() == typeof (BusinessEntityContact) && Equals((BusinessEntityContact) obj);
        }
		
		public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return Id == null ? newRandom.Next() : Id.GetHashCode();
		}


        #endregion

        #region ValueTypes

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

       #endregion


    }//end BusinessEntityContact
}//end AdventureWorks.Person


