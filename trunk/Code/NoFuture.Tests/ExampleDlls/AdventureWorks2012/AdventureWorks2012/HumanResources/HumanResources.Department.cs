using System;
using System.Collections.Generic;

namespace AdventureWorks.HumanResources
{
    [Serializable]
    public class Department
    {
        #region Id
        public virtual short Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(Department other)
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
            if (obj.GetType() != typeof (Department)) return false;
            return this.Equals((Department) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string Name { get; set; }

       public virtual string GroupName { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.HumanResources.EmployeeDepartmentHistory> EmployeeDepartmentHistories { get; set; }

       #endregion


    }//end Department
}//end AdventureWorks.HumanResources


