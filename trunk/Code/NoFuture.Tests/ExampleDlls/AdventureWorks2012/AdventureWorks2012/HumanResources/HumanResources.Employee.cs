using System;
using System.Collections.Generic;

namespace AdventureWorks.HumanResources
{
    [Serializable]
    public class Employee
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(Employee other)
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
            if (obj.GetType() != typeof (Employee)) return false;
            return this.Equals((Employee) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string NationalIDNumber { get; set; }

       public virtual string LoginID { get; set; }

       public virtual string JobTitle { get; set; }

       public virtual System.DateTime BirthDate { get; set; }

       public virtual string MaritalStatus { get; set; }

       public virtual string Gender { get; set; }

       public virtual System.DateTime HireDate { get; set; }

       public virtual bool SalariedFlag { get; set; }

       public virtual short VacationHours { get; set; }

       public virtual short SickLeaveHours { get; set; }

       public virtual bool CurrentFlag { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Person.Person PersonByBusinessEntityID { get; set; }

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.HumanResources.EmployeeDepartmentHistory> EmployeeDepartmentHistories { get; set; }

        public virtual IList<AdventureWorks.HumanResources.EmployeePayHistory> EmployeePayHistories { get; set; }

        public virtual IList<AdventureWorks.HumanResources.JobCandidate> JobCandidates { get; set; }

        public virtual IList<AdventureWorks.Production.Document> Documents { get; set; }

        public virtual IList<AdventureWorks.Purchasing.PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }

        public virtual IList<AdventureWorks.Sales.SalesPerson> SalesPersons { get; set; }

       #endregion


    }//end Employee
}//end AdventureWorks.HumanResources


