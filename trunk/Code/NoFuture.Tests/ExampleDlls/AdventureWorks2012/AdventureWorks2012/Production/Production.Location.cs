using System;
using System.Collections.Generic;

namespace AdventureWorks.Production
{
    [Serializable]
    public class Location
    {
        #region Id
        public virtual short Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(Location other)
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
            if (obj.GetType() != typeof (Location)) return false;
            return this.Equals((Location) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual string Name { get; set; }

       public virtual decimal CostRate { get; set; }

       public virtual decimal Availability { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

       #endregion

       #region Lists

        public virtual IList<AdventureWorks.Production.ProductInventory> ProductInventories { get; set; }

        public virtual IList<AdventureWorks.Production.WorkOrderRouting> WorkOrderRoutings { get; set; }

       #endregion


    }//end Location
}//end AdventureWorks.Production


