using System;

namespace AdventureWorks.Production
{
    [Serializable]
    public class WorkOrderRoutingCompositeId
    {

        public virtual AdventureWorks.Production.WorkOrder WorkOrder { get; set; }

        public virtual short OperationSequence { get; set; }

        public virtual int ProductID { get; set; }

        //region Required Overrides
        public override bool Equals(object obj)
        {
            var compareTo = (WorkOrderRoutingCompositeId)obj;

            if(compareTo.WorkOrder != this.WorkOrder){return false;}

            if(compareTo.OperationSequence != this.OperationSequence){return false;}

            if(compareTo.ProductID != this.ProductID){return false;}

            return true;

        }

        public override int GetHashCode()
        {
            var sumValue = 1;

            sumValue += (WorkOrder != null ? WorkOrder.GetHashCode() : 1);

            sumValue += (OperationSequence != null ? OperationSequence.GetHashCode() : 1);

            sumValue += (ProductID != null ? ProductID.GetHashCode() : 1);

            return sumValue;
        }
        //endregion

    }//end WorkOrderRoutingCompositeId
}//end AdventureWorks.Production
