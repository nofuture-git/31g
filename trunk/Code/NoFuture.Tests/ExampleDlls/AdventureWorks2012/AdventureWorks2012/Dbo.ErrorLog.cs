using System;
using System.Collections.Generic;

namespace AdventureWorks.Dbo
{
    [Serializable]
    public class ErrorLog
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(ErrorLog other)
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
            if (obj.GetType() != typeof (ErrorLog)) return false;
            return this.Equals((ErrorLog) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual System.DateTime ErrorTime { get; set; }

       public virtual string UserName { get; set; }

       public virtual int ErrorNumber { get; set; }

       public virtual int? ErrorSeverity { get; set; }

       public virtual int? ErrorState { get; set; }

       public virtual string ErrorProcedure { get; set; }

       public virtual int? ErrorLine { get; set; }

       public virtual string ErrorMessage { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

       #endregion


    }//end ErrorLog
}//end AdventureWorks.Dbo


