using System;
using System.Collections.Generic;

namespace AdventureWorks.Person
{
    [Serializable]
    public class EmailAddress
    {
        #region Id
        public virtual AdventureWorks.Person.EmailAddressCompositeId Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(EmailAddress other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
			return System.Object.ReferenceEquals(this, other) || Id.Equals(other.Id);
        }
		
		public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
			return obj.GetType() == typeof (EmailAddress) && Equals((EmailAddress) obj);
        }
		
		public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return Id == null ? newRandom.Next() : Id.GetHashCode();
		}


        #endregion

        #region ValueTypes

       public virtual string EmailAddress00 { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

       #endregion


    }//end EmailAddress
}//end AdventureWorks.Person


