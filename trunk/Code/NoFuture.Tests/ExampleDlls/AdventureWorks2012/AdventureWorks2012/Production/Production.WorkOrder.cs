using System;
using System.Collections.Generic;

namespace AdventureWorks.Production
{
    [Serializable]
    public class WorkOrder
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(WorkOrder other)
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
            if (obj.GetType() != typeof (WorkOrder)) return false;
            return this.Equals((WorkOrder) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual int OrderQty { get; set; }

       public virtual short ScrappedQty { get; set; }

       public virtual System.DateTime? EndDate { get; set; }

       public virtual System.DateTime DueDate { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Production.Product ProductByProductID { get; set; }

        public virtual AdventureWorks.Production.ScrapReason ScrapReasonByScrapReasonID { get; set; }

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Production.WorkOrderRouting> WorkOrderRoutings { get; set; }

       #endregion


    }//end WorkOrder
}//end AdventureWorks.Production


