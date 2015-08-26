using System;
using System.Collections.Generic;

namespace AdventureWorks.Production
{
    [Serializable]
    public class BillOfMaterials
    {
        #region Id
        public virtual int Id { get; set; }
        #endregion

        #region RequiredOverrides

		public virtual bool Equals(BillOfMaterials other)
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
            if (obj.GetType() != typeof (BillOfMaterials)) return false;
            return this.Equals((BillOfMaterials) obj);
        }
        

        #endregion

        #region ValueTypes

       public virtual System.DateTime? EndDate { get; set; }

       public virtual short BOMLevel { get; set; }

       public virtual decimal PerAssemblyQty { get; set; }

       public virtual System.DateTime ModifiedDate { get; set; }

       #endregion

       #region ForeignKeys

        public virtual AdventureWorks.Production.Product ProductByComponentID { get; set; }

        public virtual AdventureWorks.Production.Product ProductByProductAssemblyID { get; set; }

        public virtual AdventureWorks.Production.UnitMeasure UnitMeasureByUnitMeasureCode { get; set; }

       #endregion

       #region Lists

       #endregion


    }//end BillOfMaterials
}//end AdventureWorks.Production


