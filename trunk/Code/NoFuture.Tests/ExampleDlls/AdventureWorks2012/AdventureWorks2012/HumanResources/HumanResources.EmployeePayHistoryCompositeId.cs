using System;

namespace AdventureWorks.HumanResources
{
    [Serializable]
    public class EmployeePayHistoryCompositeId
    {

        public virtual AdventureWorks.HumanResources.Employee Employee { get; set; }

        public virtual System.DateTime RateChangeDate { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (EmployeePayHistoryCompositeId)obj;

            if(compareTo.Employee != this.Employee){return false;}

            if(compareTo.RateChangeDate != this.RateChangeDate){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (Employee != null ? Employee.GetHashCode() : 1);

            sumValue += (RateChangeDate != null ? RateChangeDate.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end EmployeePayHistoryCompositeId
}//end AdventureWorks.HumanResources
