using System;
using System.Collections.Generic;

namespace AdventureWorks.Person
{
    [Serializable]
    public class Password
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(Password other)
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
            if (obj.GetType() != typeof (Password)) return false;
            return this.Equals((Password) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string PasswordHash { get; set; }

       public virtual string PasswordSalt { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Person.Person PersonByBusinessEntityID { get; set; }

       #endregion

       #region Lists

       #endregion


    }//end Password
}//end AdventureWorks.Person


