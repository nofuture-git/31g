using System;
using System.Collections.Generic;

namespace AdventureWorks.Production
{
    [Serializable]
    public class WorkOrderRouting
    {
        #region Id
        public virtual AdventureWorks.Production.WorkOrderRoutingCompositeId Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(WorkOrderRouting other)
        {
            if (System.Object.ReferenceEquals(null, other)) return false;
			return System.Object.ReferenceEquals(this, other) || Id.Equals(other.Id);
        }
		
		public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(null, obj)) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
			return obj.GetType() == typeof (WorkOrderRouting) && Equals((WorkOrderRouting) obj);
        }
		
		public override int GetHashCode()
        {
            var newRandom = new System.Random();
			return Id == null ? newRandom.Next() : Id.GetHashCode();
		}


        #endregion

        #region ValueTypes

       public virtual System.DateTime ScheduledStartDate { get; set; }

       public virtual System.DateTime ScheduledEndDate { get; set; }

       public virtual System.DateTime? ActualStartDate { get; set; }

       public virtual System.DateTime? ActualEndDate { get; set; }

       public virtual decimal? ActualResourceHrs { get; set; }

       public virtual decimal PlannedCost { get; set; }

       public virtual decimal? ActualCost { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Production.Location LocationByLocationID { get; set; }

       #endregion

       #region Lists

       #endregion


    }//end WorkOrderRouting
}//end AdventureWorks.Production


