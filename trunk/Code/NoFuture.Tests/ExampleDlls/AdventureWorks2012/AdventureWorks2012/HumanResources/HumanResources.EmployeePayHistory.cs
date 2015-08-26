using System;
using System.Collections.Generic;

namespace AdventureWorks.HumanResources
{
    [Serializable]
    public class EmployeePayHistory
    {
        #region Id
        public virtual AdventureWorks.HumanResources.EmployeePayHistoryCompositeId Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(EmployeePayHistory other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
			return System.Object.ReferenceEquals(this, other) || Id.Equals(other.Id);
        }
		
		public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
			return obj.GetType() == typeof (EmployeePayHistory) && Equals((EmployeePayHistory) obj);
        }
		
		public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return Id == null ? newRandom.Next() : Id.GetHashCode();
		}


        #endregion

        #region ValueTypes

       public virtual decimal Rate { get; set; }

       public virtual byte PayFrequency { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

       #endregion


    }//end EmployeePayHistory
}//end AdventureWorks.HumanResources


