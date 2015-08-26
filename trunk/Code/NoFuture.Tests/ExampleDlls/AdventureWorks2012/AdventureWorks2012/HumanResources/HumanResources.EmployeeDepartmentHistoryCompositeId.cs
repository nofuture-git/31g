using System;

namespace AdventureWorks.HumanResources
{
    [Serializable]
    public class EmployeeDepartmentHistoryCompositeId
    {

        public virtual AdventureWorks.HumanResources.Department Department { get; set; }

        public virtual AdventureWorks.HumanResources.Employee Employee { get; set; }

        public virtual AdventureWorks.HumanResources.Shift Shift { get; set; }

        public virtual System.DateTime StartDate { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (EmployeeDepartmentHistoryCompositeId)obj;

            if(compareTo.Department != this.Department){return false;}

            if(compareTo.Employee != this.Employee){return false;}

            if(compareTo.Shift != this.Shift){return false;}

            if(compareTo.StartDate != this.StartDate){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (Department != null ? Department.GetHashCode() : 1);

            sumValue += (Employee != null ? Employee.GetHashCode() : 1);

            sumValue += (Shift != null ? Shift.GetHashCode() : 1);

            sumValue += (StartDate != null ? StartDate.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end EmployeeDepartmentHistoryCompositeId
}//end AdventureWorks.HumanResources
