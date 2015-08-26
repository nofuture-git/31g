using System;
using System.Collections.Generic;

namespace AdventureWorks.HumanResources
{
    [Serializable]
    public class JobCandidate
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(JobCandidate other)
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
            if (obj.GetType() != typeof (JobCandidate)) return false;
            return this.Equals((JobCandidate) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string Resume { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.HumanResources.Employee EmployeeByBusinessEntityID { get; set; }

       #endregion

       #region Lists

       #endregion


    }//end JobCandidate
}//end AdventureWorks.HumanResources


